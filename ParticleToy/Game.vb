Public Class Game
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

    Public Overrides Sub Init()
        MyBase._ScreenSize = New Size(800, 600) ' My.Computer.Screen.Bounds.Size
        For index = 1 To 10000
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
            Ancs.Anchors.AddRange(FixedAncors.Anchors.ToArray)
            For Each P In PL
                P.Update(Me, Tick, MouseInfo, Keyboard)
            Next
        End If
    End Sub

    Public Overrides Sub Draw(G As Graphics)
        'Clear
        G.FillRectangle(ClearBru, 0, 0, ScreenSize.Width, ScreenSize.Height)

        'Setup
        G.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias

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
