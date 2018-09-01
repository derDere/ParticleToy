﻿Public Class frmMain

    Const Title As String = "Tesla Toy"

    Public Shared ReadOnly CursorDefault As Cursor
    Public Shared ReadOnly CursorRed As Cursor

    Private RealMP As Point? = Nothing
    Private MouseL As Boolean = False
    Private MouseR As Boolean = False
    Private MouseM As Boolean = False
    Private MouseW As Integer = 0
    Private PressedKeys As New List(Of Keys)

    Private Game As GameBase = New Game
    Private Tick As Integer = 0
    Private B As Bitmap

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

    Public Sub New()
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
    End Sub

    Private Sub Ticker_Tick(sender As Object, e As EventArgs) Handles Ticker.Tick
        Ticker.Stop()

        Dim SW As New Stopwatch
        SW.Start()

        Dim mi As MouseInfo
        If RealMP IsNot Nothing Then
            Dim pX As Double = RealMP.Value.X / (ScreenImgBox.Width + 0.00001D) 'Verhindert Division durch 0 falls Fenster Minimiert
            If pX > My.Computer.Screen.WorkingArea.Width Then pX = My.Computer.Screen.WorkingArea.Width
            Dim mouseX As Integer = Math.Round(pX * Game.ScreenSize.Width)
            Dim pY As Double = RealMP.Value.Y / (ScreenImgBox.Height + 0.00001D) 'Verhindert Division durch 0 falls Fenster Minimiert
            If pY > My.Computer.Screen.WorkingArea.Height Then pY = My.Computer.Screen.WorkingArea.Height
            Dim mouseY As Integer = Math.Round(pY * Game.ScreenSize.Height)
            mi = New MouseInfo(MouseL, MouseR, MouseM, New Point(mouseX, mouseY), MouseW, My.Computer.Mouse)
        Else
            mi = New MouseInfo(MouseL, MouseR, MouseM, Nothing, MouseW, My.Computer.Mouse)
        End If

        Dim KeyInf As New KeyBoardInfo
        KeyInf.KeysDown = PressedKeys.ToArray

        Game.Update(Tick, mi, KeyInf)

        Using G As Graphics = Graphics.FromImage(B)
            Game.Draw(G)
        End Using

        ScreenImgBox.Image = B

        MouseW = 0

        Tick += 1
        SW.Stop()
        Dim TickDelay As Integer = 16 - SW.ElapsedMilliseconds
        If TickDelay < 1 Then TickDelay = 1
        Ticker.Interval = TickDelay
        Dim ts As Integer = SW.ElapsedMilliseconds
        If ts < 16 Then ts = 16
        Me.Text = Math.Round(1000 / ts, 1) & " FPS"

        KeyBoardInfo.LastKeys = KeyInf.KeysDown

        Ticker.Start()
    End Sub

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
        RealMP = New Point(e.X, e.Y)
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
        If e.KeyCode = Keys.F11 Then
            FullScreen = Not FullScreen
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

End Class