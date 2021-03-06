﻿Imports ParticleToy
Imports ParticleToy.Behaviour

Public Class Game
    Inherits GameBase

    Public Const DEFAULT_BG_ALPHA As Integer = 44
    Public Property BG_Alpha As Integer = DEFAULT_BG_ALPHA
    'Public ReadOnly ClearColor1 As Color = Color.FromArgb(DEFAULT_BG_ALPHA, 9, 32, 0)
    'Public ReadOnly ClearColor2 As Color = Color.FromArgb(DEFAULT_BG_ALPHA, 0, 0, 0)
    'Public ReadOnly ClearColor3 As Color = Color.FromArgb(DEFAULT_BG_ALPHA, 36, 0, 61)
    'Public ReadOnly ClearBru As New Drawing2D.LinearGradientBrush(New Point(0, 0), New Point(OPT_SIZE_W, OPT_SIZE_H), ClearColor2, ClearColor2) With {
    '    .InterpolationColors = New Drawing2D.ColorBlend() With {
    '        .Colors = {
    '            ClearColor1,
    '            ClearColor2,
    '            ClearColor3
    '        },
    '        .Positions = {
    '            0,
    '            0.5,
    '            1
    '        }
    '    }
    '}
    Public ClearBru As New SolidBrush(Color.FromArgb(BG_Alpha, 0, 0, 0))

    Public InfoBgBru As New SolidBrush(Color.FromArgb(192, 0, 0, 0))
    Public InfoLinePen As New Pen(Color.White, 3)
    Public Font As New Font("Arial", 15)

    Private MousePos As PointF? = Nothing
    Private MouseDown As Boolean = False
    Private DrawHint As Boolean = True

    Public Ancs As New Anchors
    Public FixedAncors As New Anchors
    Public DrawInfo As Boolean = False

    Public ParticleL As New List(Of Particle)
    Public BlinkL As New List(Of Blink)
    Public Menu As New Menu(Me)
    Public Console As New GameConsole(Me)

    Public Const OPT_SIZE_W As Integer = 800
    Public Const OPT_SIZE_H As Integer = 600
    Public Const PARTICLE_NUMBER As Integer = 10000
    Public Const HINT_TICK_LENGTH As Integer = 50

    Public Shared ColorManager As IColorManager = Nothing

    Private LastColorManager As IColorManager = Nothing

    Private LastBehaviourKey As String = "unknown"
    Public Shared BehaviourKey As String = ""

    Private _CurrentTick As Integer = 0
    Public ReadOnly Property CurrentTick As Integer
        Get
            Return _CurrentTick
        End Get
    End Property

    Public Overrides Sub Init()
        MyBase._ScreenSize = New Size(OPT_SIZE_W, OPT_SIZE_H) ' My.Computer.Screen.Bounds.Size
        For index = 1 To PARTICLE_NUMBER
            ParticleL.Add(New Particle(Ancs, RndPoint(ScreenSize), index - 1, Me))
        Next
    End Sub

    Public Overrides Sub Update(Tick As Integer, MouseInfo As MouseInfo, Keyboard As KeyBoardInfo)
        _CurrentTick = Tick
        If Tick > HINT_TICK_LENGTH Then
            DrawHint = False
        End If
        If Menu.UpdateAndIsOpen(Tick, MouseInfo, Keyboard) Then
        Else
            DrawInfo = Keyboard.CtrlKeyDown
            MousePos = MouseInfo.Position
            MouseDown = MouseInfo.LeftIsPressed
            Ancs.Anchors.Clear()
            If MouseInfo.LeftIsPressed Then
                Console.EnteredMode = ""
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
            If Keyboard.Pressed(Keys.D1) Then FunctionKeySetAncorCount = 1
            If Keyboard.Pressed(Keys.D2) Then FunctionKeySetAncorCount = 2
            If Keyboard.Pressed(Keys.D3) Then FunctionKeySetAncorCount = 3
            If Keyboard.Pressed(Keys.D4) Then FunctionKeySetAncorCount = 4
            If Keyboard.Pressed(Keys.D5) Then FunctionKeySetAncorCount = 5
            If Keyboard.Pressed(Keys.D6) Then FunctionKeySetAncorCount = 6
            If Keyboard.Pressed(Keys.D7) Then FunctionKeySetAncorCount = 7
            If Keyboard.Pressed(Keys.D8) Then FunctionKeySetAncorCount = 8
            If Keyboard.Pressed(Keys.D9) Then FunctionKeySetAncorCount = 9
            If Keyboard.Pressed(Keys.D0) Then FunctionKeySetAncorCount = 10
            If Keyboard.Pressed(Keys.Oem4) Then FunctionKeySetAncorCount = 11
            If Keyboard.Pressed(Keys.Oem6) Then FunctionKeySetAncorCount = 12
            If FunctionKeySetAncorCount <> 0 Then
                If Config.Unlocked.Contains(FunctionKeySetAncorCount.ToString) Then
                    SetAnchorCount(FunctionKeySetAncorCount)
                End If
            End If
            Ancs.Anchors.AddRange(FixedAncors.Anchors.ToArray)
            If Not DrawInfo Then
                Console.Update(Me, Tick, MouseInfo, Keyboard)
                BehaviourKey = ""
                If String.IsNullOrEmpty(Console.EnteredMode) Or String.IsNullOrWhiteSpace(Console.EnteredMode) Then
                    BehaviourKey = Ancs.Anchors.Count
                Else
                    BehaviourKey = Console.EnteredMode
                End If
                If Not Controls.Behaviours.ContainsKey(BehaviourKey) Then
                    BehaviourKey = "unknown"
                End If
                If Not Config.Unlocked.Contains(BehaviourKey) Then
                    Console.AddLine("Unloked mode: " & Controls.Behaviours(BehaviourKey).Name)
                    Config.Unlocked.Add(BehaviourKey)
                    Controls.Update()
                End If
                Dim CurrentBehaviour As IBehaviour = Controls.Behaviours(BehaviourKey)

                Dim OffBehaviour As IBehaviour = Nothing
                Dim OnBehaviour As IBehaviour = Nothing
                If LastBehaviourKey <> BehaviourKey Then
                    Controls.Update()
                    OffBehaviour = Controls.Behaviours(LastBehaviourKey)
                    OnBehaviour = Controls.Behaviours(BehaviourKey)
                    LastBehaviourKey = BehaviourKey
                    If OnBehaviour.OverwriteColorManager Then
                        ColorManager = OnBehaviour.ColorManager
                    ElseIf ColorManager Is Nothing Then
                        ColorManager = OnBehaviour.ColorManager
                    End If
                End If

                Dim OnColorManager As IColorManager = Nothing
                If LastColorManager IsNot ColorManager Then
                    Controls.Update()
                    OnColorManager = ColorManager
                    LastColorManager = ColorManager
                End If

                For Each ParticleItm In ParticleL
                    ParticleItm.Update(Me, Tick, MouseInfo, Keyboard, OnBehaviour, CurrentBehaviour, OffBehaviour, OnColorManager, ColorManager)
                Next

                If TypeOf ColorManager Is NoColorManager Then
                    ColorManager = Nothing
                End If

                For Each ParticleItm In ParticleL
                    If ParticleItm.WillGlow Then
                        ParticleItm.Glowing = True
                    Else
                        ParticleItm.Glowing = 0
                    End If
                Next

                Dim KillList As New List(Of Blink)
                For Each BlinkItm As Blink In BlinkL
                    BlinkItm.Update(Me, Tick, MouseInfo, Keyboard)
                    If BlinkItm.KillMe Then KillList.Add(BlinkItm)
                Next
                For Each BlinkItm As Blink In KillList
                    If BlinkL.Contains(BlinkItm) Then
                        BlinkL.Remove(BlinkItm)
                    End If
                Next

                ClearBru = New SolidBrush(Color.FromArgb(BG_Alpha, 0, 0, 0))
            End If
        End If
    End Sub

    Public Sub SetAnchorCount(Count As Integer)
        Console.EnteredMode = ""
        FixedAncors.Anchors.Clear()
        For n = 1 To Count
            FixedAncors.Anchors.Add(RndPoint(ScreenSize))
        Next
    End Sub

    Public Overrides Sub Draw(G As Graphics)
        'Clear
        G.FillRectangle(ClearBru, 0, 0, ScreenSize.Width, ScreenSize.Height)

        'Setup
        'G.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias

        'Draw Particles
        For Each ParticleItm In ParticleL
            ParticleItm.Draw(G)
        Next

        'Draw Blinks
        For Each BlinkItm As Blink In BlinkL
            BlinkItm.Draw(G)
        Next

        If DrawInfo Then
            Dim s As SizeF = G.MeasureString(Ancs.Anchors.Count, Font)
            G.FillRectangle(InfoBgBru, 8, 8, s.Width + 4, s.Height + 4)
            For Each P As PointF In Ancs.Anchors
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

    Public Sub HandleCommand(Command As String)
        Command = Command.ToLower
        Console.AddLine(Command)
    End Sub

End Class
