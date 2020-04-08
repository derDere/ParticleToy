Public Class Blink

    Public Property Position As PointF
    Public Property KillMe As Boolean = False

    Private Parent As Game
    Private Pen As Pen
    Private MaxRadius As Double
    Private MinRadius As Double
    Private Radius As Double

    Public Sub New(Position As PointF, Parent As Game, Color As Color, Optional MaxRadius As Double = 10, Optional MinRadius As Double = 0, Optional Width As Integer = 1)
        Me.Position = Position
        Me.Parent = Parent
        Me.Pen = New Pen(Color, Width)
        Me.MaxRadius = MaxRadius
        Me.MinRadius = MinRadius
        Me.Radius = MinRadius
    End Sub

    Public Sub Update(Game As GameBase, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Microsoft.VisualBasic.Devices.Keyboard)
        Radius += 1
        If Radius = MaxRadius Then
            KillMe = True
        End If
    End Sub

    Public Sub Draw(G As Graphics)
        If Not KillMe Then
            G.DrawEllipse(Me.Pen, CInt(Math.Round(Me.Position.X - Me.Radius)), CInt(Math.Round(Me.Position.Y - Me.Radius)), CInt(Math.Round(Me.Radius * 2 + 1)), CInt(Math.Round(Me.Radius * 2 + 1)))
        End If
    End Sub

End Class
