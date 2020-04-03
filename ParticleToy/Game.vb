Imports ParticleToy

Public Class Game
    Inherits GameBase

    Public ClearColor1 As Color = Color.FromArgb(44, 9, 32, 0)
    Public ClearColor2 As Color = Color.FromArgb(44, 0, 0, 0)
    Public ClearColor3 As Color = Color.FromArgb(44, 36, 0, 61)
    Public ClearBru As New Drawing2D.LinearGradientBrush(New Point(0, 0), New Point(OPT_SIZE_W, OPT_SIZE_H), ClearColor2, ClearColor2) With {
        .InterpolationColors = New Drawing2D.ColorBlend() With {
            .Colors = {
                ClearColor1,
                ClearColor2,
                ClearColor3
            },
            .Positions = {
                0,
                0.5,
                1
            }
        }
    }
    ' OLD: Public ClearBru As New SolidBrush(Color.FromArgb(44, 0, 0, 0))

    Public InfoBgBru As New SolidBrush(Color.FromArgb(192, 0, 0, 0))
    Public InfoLinePen As New Pen(Brushes.White, 3)
    Public Font As New Font("Arial", 15)

    Private MousePos As Point? = Nothing
    Private MouseDown As Boolean = False
    Private DrawHint As Boolean = True

    Public Ancs As New Anchors
    Public FixedAncors As New Anchors
    Public DrawInfo As Boolean = False

    Public PL As New List(Of Particle)
    Public Menu As New Menu(Me)
    Public Console As New GameConsole(Me)

    Public Const OPT_SIZE_W As Integer = 800
    Public Const OPT_SIZE_H As Integer = 600
    Public Const PARTICLE_NUMBER As Integer = 10000
    Public Const HINT_TICK_LENGTH As Integer = 50

    Public Behaviours As New Dictionary(Of String, IBehaviour)

    Private LastBehaviourKey As String = "-1"

    Public Overrides Sub Init()
        MyBase._ScreenSize = New Size(OPT_SIZE_W, OPT_SIZE_H) ' My.Computer.Screen.Bounds.Size
        For index = 1 To PARTICLE_NUMBER
            PL.Add(New Particle(Ancs, RndPoint(ScreenSize), index - 1, Me))
        Next
        For Each ItmType As Type In Reflection.Assembly.GetExecutingAssembly.GetTypes
            If ItmType.GetInterfaces.Any(Function(I) I.Name = "IBehaviour") Then
                Dim IBObj As IBehaviour = Activator.CreateInstance(ItmType)
                Behaviours.Add(IBObj.Key, IBObj)
            End If
        Next
    End Sub

    Public Overrides Sub Update(Tick As Integer, MouseInfo As MouseInfo, Keyboard As KeyBoardInfo)
        If Tick > HINT_TICK_LENGTH Then
            DrawHint = False
        End If
        If Console.State = ConsoleState.Open Then Exit Sub
        If Menu.UpdateAndIsOpen(Tick, MouseInfo, Keyboard) Then
        Else
            DrawInfo = Keyboard.CtrlKeyDown
            MousePos = MouseInfo.Position
            MouseDown = MouseInfo.LeftIsPressed
            Ancs.Anchors.Clear()
            If MouseInfo.LeftIsPressed Then
                If Keyboard.CtrlKeyDown And MouseInfo.PressedLeftIsPressed Then
                    FixedAncors.Anchors.Add(MouseInfo.Position)
                    Debug.WriteLine("Fixed")
                ElseIf Not Keyboard.CtrlKeyDown Then
                    Ancs.Anchors.Add(MouseInfo.Position)
                    FixedAncors.Anchors.Clear()
                    Debug.WriteLine("clear")
                End If
            End If
            Dim FunctionKeySetAncorCount As Integer = 0
            If Keyboard.Pressed(Keys.F1) Then FunctionKeySetAncorCount = 1
            If Keyboard.Pressed(Keys.F2) Then FunctionKeySetAncorCount = 2
            If Keyboard.Pressed(Keys.F3) Then FunctionKeySetAncorCount = 3
            If Keyboard.Pressed(Keys.F4) Then FunctionKeySetAncorCount = 4
            If Keyboard.Pressed(Keys.F5) Then FunctionKeySetAncorCount = 5
            If Keyboard.Pressed(Keys.F6) Then FunctionKeySetAncorCount = 6
            If Keyboard.Pressed(Keys.F7) Then FunctionKeySetAncorCount = 7
            If Keyboard.Pressed(Keys.F8) Then FunctionKeySetAncorCount = 8
            If Keyboard.Pressed(Keys.F9) Then FunctionKeySetAncorCount = 9
            If Keyboard.Pressed(Keys.F10) Then FunctionKeySetAncorCount = 10
            If Keyboard.Pressed(Keys.F11) Then FunctionKeySetAncorCount = 11
            If Keyboard.Pressed(Keys.F12) Then FunctionKeySetAncorCount = 12
            If FunctionKeySetAncorCount <> 0 Then
                FixedAncors.Anchors.Clear()
                For n = 1 To FunctionKeySetAncorCount
                    FixedAncors.Anchors.Add(RndPoint(ScreenSize))
                Next
            End If
            Ancs.Anchors.AddRange(FixedAncors.Anchors.ToArray)
            If Not DrawInfo Then
                Dim BehaviourKey As String = ""
                If String.IsNullOrEmpty(Console.EnteredMode) Or String.IsNullOrWhiteSpace(Console.EnteredMode) Then
                    BehaviourKey = Ancs.Anchors.Count
                Else
                    BehaviourKey = Console.EnteredMode
                End If
                If Not Behaviours.ContainsKey(BehaviourKey) Then
                    BehaviourKey = "-1"
                End If
                Dim CurrentBehaviour As IBehaviour = Behaviours(BehaviourKey)

                Dim NotBehaviour As IBehaviour
                If LastBehaviourKey <> BehaviourKey Then
                    NotBehaviour = Behaviours(LastBehaviourKey)
                End If
                LastBehaviourKey = BehaviourKey

                For Each P In PL
                    P.Update(Me, Tick, MouseInfo, Keyboard, CurrentBehaviour, NotBehaviour)
                Next

                For Each P In PL
                    If P.WillGlow Then
                        P.Glowing = True
                    Else
                        P.Glowing = 0
                    End If
                Next
            End If
        End If
    End Sub

    Public Overrides Sub Draw(G As Graphics)
        'Clear
        G.FillRectangle(ClearBru, 0, 0, ScreenSize.Width, ScreenSize.Height)

        'Setup
        'G.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias

        'Draw Particles
        For Each P In PL
            P.Draw(G)
        Next

        If DrawInfo Then
            Dim s As SizeF = G.MeasureString(Ancs.Anchors.Count, Font)
            G.FillRectangle(InfoBgBru, 8, 8, s.Width + 4, s.Height + 4)
            For Each P As Point In Ancs.Anchors
                G.FillEllipse(InfoBgBru, P.X - 8, P.Y - 8, 16, 16)
                G.DrawLine(InfoLinePen, Offset(P, -6, 0), Offset(P, 6, 0))
                G.DrawLine(InfoLinePen, Offset(P, 0, -6), Offset(P, 0, 6))
                G.DrawLine(Pens.Blue, Offset(P, -5, 0), Offset(P, 5, 0))
                G.DrawLine(Pens.Blue, Offset(P, 0, -5), Offset(P, 0, 5))
            Next
            G.DrawString(Ancs.Anchors.Count, Font, Brushes.LightBlue, 10, 10)
        End If

        If DrawHint Then
            G.DrawString("Press ESC for some infos", Font, Brushes.White, 100, 100)
        End If

        'Draw Menu
        Menu.Draw(G)

        'Draw Console
        Console.Draw(G)
    End Sub

    Public Overrides Sub ConsoleToggle(State As ConsoleState)
        Console.State = State
    End Sub

    Public Overrides Sub ExecuteCommand(Command As String)
        If String.IsNullOrEmpty(Command) Or String.IsNullOrWhiteSpace(Command) Then Exit Sub
        Console.AddLine(Brushes.Yellow, ": " & Command)
    End Sub

End Class
