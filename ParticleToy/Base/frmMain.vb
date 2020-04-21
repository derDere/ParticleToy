Imports System.ComponentModel
Imports System.Runtime.InteropServices

Public Class frmMain

    Const Title As String = "Particle Toy"

    Public Shared ReadOnly CursorDefault As Cursor
    Public Shared ReadOnly CursorRed As Cursor

    Private RealMP As PointF? = Nothing
    Private MouseL As Boolean = False
    Private MouseR As Boolean = False
    Private MouseM As Boolean = False
    Private MouseW As Integer = 0
    Private PressedKeys As New List(Of Keys)

    Private Recording As Boolean = False
    Private RecordingFrame As Integer = 0
    Private RecordingDir As IO.DirectoryInfo = Nothing

    Private Game As Game = New Game
    Private Tick As Integer = 0
    Private B As Bitmap
    Private ModeForm As New frmModes

    Shared Sub New()
        Dim defaultCurB As New Bitmap(32, 32)
        Using g As Graphics = Graphics.FromImage(defaultCurB)
            g.Clear(Color.Transparent)
            g.DrawEllipse(Pens.White, 10, 10, 11, 11)
        End Using
        CursorDefault = New Cursor(defaultCurB.GetHicon)

        Dim redCurB As New Bitmap(32, 32)
        Using g As Graphics = Graphics.FromImage(redCurB)
            g.Clear(Color.Transparent)
            g.DrawEllipse(Pens.Red, 10, 10, 11, 11)
        End Using
        CursorRed = New Cursor(redCurB.GetHicon)
    End Sub

    <FlagsAttribute>
    Public Enum EXECUTION_STATE : uint
        ES_SYSTEM_REQUIRED = &H1
        ES_DISPLAY_REQUIRED = &H2
        ' Legacy flag, should Not be used.
        ' ES_USER_PRESENT   = &H00000004
        ES_AWAYMODE_REQUIRED = &H40
        ES_CONTINUOUS = &H80000000
    End Enum

    <DllImport("kernel32.dll", CharSet:=CharSet.Auto, SetLastError:=True)>
    Public Shared Function SetThreadExecutionState(esFlags As EXECUTION_STATE) As EXECUTION_STATE
    End Function

    Public Sub New()
        SetThreadExecutionState(EXECUTION_STATE.ES_DISPLAY_REQUIRED)

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
        ScreenImgBox.Cursor = CursorDefault
        LastWinState = Me.WindowState
        LastBounds = Me.Bounds

        Game.Init()

        B = New Bitmap(Game.ScreenSize.Width, Game.ScreenSize.Height)
        Using g As Graphics = Graphics.FromImage(B)
            g.Clear(Color.Black)
        End Using

        Ticker_Tick(Nothing, Nothing)

        Ticker.Start()

        ModeForm.Owner = Me
        AddHandler ModeForm.ControlsEle.ModeSelect, AddressOf ModeForm_ModeSelected
        AddHandler ModeForm.ControlsEle.ColorSelect, AddressOf ModeForm_ColorSelected
        AddHandler ModeForm.ControlsEle.CommandEntered, AddressOf ModeForm_CommandEntered

        ModeForm.Show()
    End Sub

    Private Sub ModeForm_CommandEntered(Command As String)
        Game.HandleCommand(Command)
    End Sub

    Private Sub ModeForm_ColorSelected(CM As IColorManager)
        Game.ColorManager = CM
    End Sub

    Private Sub ModeForm_ModeSelected(Mode As String)
        If IsNumeric(Mode) Then
            Game.SetAnchorCount(CInt(Mode))
        Else
            Game.Console.EnteredMode = Mode
        End If
    End Sub

    Private Sub Ticker_Tick(sender As Object, e As EventArgs) Handles Ticker.Tick
        Ticker.Stop()

        Dim SW As New Stopwatch
        SW.Start()

        Dim mi As MouseInfo
        If RealMP IsNot Nothing Then
            Dim pX As Double = RealMP.Value.X / (ScreenImgBox.Width + 0.00001D) 'Verhindert Division durch 0 falls Fenster Minimiert
            If pX > My.Computer.Screen.WorkingArea.Width Then pX = My.Computer.Screen.WorkingArea.Width
            Dim mouseX As Double = (pX * Game.ScreenSize.Width)
            Dim pY As Double = RealMP.Value.Y / (ScreenImgBox.Height + 0.00001D) 'Verhindert Division durch 0 falls Fenster Minimiert
            If pY > My.Computer.Screen.WorkingArea.Height Then pY = My.Computer.Screen.WorkingArea.Height
            Dim mouseY As Double = (pY * Game.ScreenSize.Height)
            mi = New MouseInfo(MouseL, MouseR, MouseM, New PointF(mouseX, mouseY), MouseW, My.Computer.Mouse)
        Else
            mi = New MouseInfo(MouseL, MouseR, MouseM, Nothing, MouseW, My.Computer.Mouse)
        End If

        Dim KeyInf As New KeyBoardInfo
        KeyInf.KeysDown = PressedKeys.ToArray

        Game.Update(Tick, mi, KeyInf)

        Using G As Graphics = Graphics.FromImage(B)
            Game.Draw(G)
        End Using

        If Recording Then
            RecordingFrame += 1
            Dim Path As String = RecordingDir.FullName
            If Not Path.EndsWith("\") Then Path &= "\"
            Path &= RecordingFrame & ".png"
            B.Save(Path, Imaging.ImageFormat.Png)
        End If

        ScreenImgBox.Image = B

        MouseW = 0

        Tick += 1S
        SW.Stop()
        Dim TickDelay As Integer = 16 - SW.ElapsedMilliseconds
        If TickDelay < 1 Then TickDelay = 1
        Ticker.Interval = TickDelay
        Dim ts As Integer = SW.ElapsedMilliseconds
        If ts < 16 Then ts = 16
        Me.Text = Title & "  ‒  " & Math.Round(1000 / ts).ToString("00") &
                  " FPS  ‒  " & DirectCast(Game, Game).Ancs.Anchors.Count &
                  " Anchors" &
                  IIf(String.IsNullOrEmpty(Game.Console.EnteredMode), "", " ‒ Mode: " & ModeDisplay(Game.Console.EnteredMode)) &
                  IIf(Recording, " ‒ Recording", "")

        KeyBoardInfo.LastKeys = KeyInf.KeysDown

        Ticker.Start()
    End Sub

    Public Function ModeDisplay(modekey As String) As String
        If ParticleToy.Controls.Behaviours.ContainsKey(modekey) Then
            Return ParticleToy.Controls.Behaviours(modekey).Name
        End If
        Return ParticleToy.Controls.Behaviours("unknown").Name
    End Function

    Private Sub ScreenImgBox_MouseDown(sender As Object, e As MouseEventArgs) Handles ScreenImgBox.MouseDown
        ScreenImgBox.Cursor = CursorRed
        If e.Button = Windows.Forms.MouseButtons.Left Then
            MouseL = True
        ElseIf e.Button = Windows.Forms.MouseButtons.Right Then
            MouseR = True
        ElseIf e.Button = Windows.Forms.MouseButtons.Middle Then
            MouseM = True
        End If
    End Sub

    Private Sub ScreenImgBox_MouseLeave(sender As Object, e As EventArgs) Handles ScreenImgBox.MouseLeave
        MouseL = False
        MouseR = False
        MouseM = False
    End Sub

    Private Sub ScreenImgBox_MouseMove(sender As Object, e As MouseEventArgs) Handles ScreenImgBox.MouseMove
        RealMP = New PointF(e.X, e.Y)
    End Sub

    Private Sub ScreenImgBox_MouseUp(sender As Object, e As MouseEventArgs) Handles ScreenImgBox.MouseUp
        ScreenImgBox.Cursor = CursorDefault
        If e.Button = Windows.Forms.MouseButtons.Left Then
            MouseL = False
        ElseIf e.Button = Windows.Forms.MouseButtons.Right Then
            MouseR = False
        ElseIf e.Button = Windows.Forms.MouseButtons.Middle Then
            MouseM = False
        End If
    End Sub

    Private Sub ScreenImgBox_MouseWheel(sender As Object, e As MouseEventArgs) Handles ScreenImgBox.MouseWheel
        MouseW = e.Delta
    End Sub

    Private Sub frmMain_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If Not PressedKeys.Contains(e.KeyCode) Then _
            PressedKeys.Add(e.KeyCode)
        If e.KeyCode = Keys.F11 AndAlso e.Modifiers = Keys.Control Then
            FullScreen = Not FullScreen
        End If
        If e.KeyCode = Keys.R Then
            If Recording Then
                Recording = False
                RecordingDir = Nothing
                RecordingFrame = 0
            Else
                Ticker.Enabled = False
                If SaveDirDialog.ShowDialog = DialogResult.OK Then
                    Dim di As New IO.DirectoryInfo(SaveDirDialog.FileName)
                    If Not IO.Directory.Exists(di.FullName) Then
                        Try
                            IO.Directory.CreateDirectory(di.FullName)
                            RecordingDir = di
                        Catch ex As Exception
                            If Debugger.IsAttached Then
                                Throw ex
                            End If
                            MsgBox("Couldn't create frame folder!", MsgBoxStyle.OkOnly, "Canceled!")
                            Ticker.Enabled = True
                            Exit Sub
                        End Try
                    Else
                        MsgBox("The choosen folder already exists!", vbOKOnly, "Error")
                        Ticker.Enabled = True
                        Exit Sub
                    End If
                    If RecordingDir IsNot Nothing Then
                        MsgBox("All frames will now be saved to the selected folder until you press [R] again! Whatch your drive space ;)", vbOKOnly, "Attention")
                        Recording = True
                        RecordingFrame = 0
                    End If
                End If
                Ticker.Enabled = True
            End If
        End If
    End Sub
    Private Sub frmMain_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If PressedKeys.Contains(e.KeyCode) Then _
            PressedKeys.Remove(e.KeyCode)
    End Sub

    Private LastWinState As FormWindowState
    Private LastBounds As Rectangle
    Private _FullScreen As Boolean
    Public Property FullScreen() As Boolean
        Get
            Return _FullScreen
        End Get
        Set(ByVal value As Boolean)
            If value = _FullScreen Then Exit Property
            _FullScreen = value
            If _FullScreen Then
                LastWinState = Me.WindowState
                LastBounds = Me.Bounds
                Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
                Me.TopMost = True
                Me.WindowState = FormWindowState.Normal
                Me.Bounds = My.Computer.Screen.Bounds
            Else
                Me.FormBorderStyle = Windows.Forms.FormBorderStyle.Sizable
                Me.TopMost = False
                Me.Bounds = LastBounds
                Me.WindowState = LastWinState
            End If
        End Set
    End Property

    Private Sub ActivityTicker_Tick(sender As Object, e As EventArgs) Handles ActivityTicker.Tick
        SendKeys.Send("{NUMLOCK}{NUMLOCK}")
    End Sub

    Private Sub frmMain_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing

    End Sub

    Private Sub frmMain_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        ModeForm.Close()
    End Sub

End Class
