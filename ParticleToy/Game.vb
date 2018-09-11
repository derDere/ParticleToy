﻿Public Class Game
    Inherits GameBase

    Public ClearBru As New SolidBrush(Color.FromArgb(44, 0, 0, 0))
    Public InfoBgBru As New SolidBrush(Color.FromArgb(192, 0, 0, 0))
    Public InfoLinePen As New Pen(Brushes.White, 3)
    Public Font As New Font("Arial", 15)

    Private MousePos As Point? = Nothing
    Private MouseDown As Boolean = False

    Public Ancs As New Anchors
    Public FixedAncors As New Anchors
    Public DrawInfo As Boolean = False

    Public PL As New List(Of Particle)
    Public Menu As New Menu(Me)

    Public Const OPT_SIZE_W As Integer = 800
    Public Const OPT_SIZE_H As Integer = 600
    Public Const PARTICLE_NUMBER As Integer = 10000

    Public Overrides Sub Init()
        MyBase._ScreenSize = New Size(OPT_SIZE_W, OPT_SIZE_H) ' My.Computer.Screen.Bounds.Size
        For index = 1 To PARTICLE_NUMBER
            PL.Add(New Particle(Ancs, RndPoint(ScreenSize), index - 1, Me))
        Next
    End Sub

    Public Overrides Sub Update(Tick As Integer, MouseInfo As MouseInfo, Keyboard As KeyBoardInfo)
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
                For Each P In PL
                    P.Update(Me, Tick, MouseInfo, Keyboard)
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

        'Draw Menu
        Menu.Draw(G)
    End Sub

End Class
