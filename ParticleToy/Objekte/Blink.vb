Public Class Blink

    Public Property Position As Point
    Public Property KillMe As Boolean = False

    Private Parent As Game
    Private Pen As Pen
    Private MaxRadius As Integer
    Private MinRadius As Integer
    Private Radius As Integer

    Public Sub New(Position As Point, Parent As Game, Color As Color, Optional MaxRadius As Integer = 10, Optional MinRadius As Integer = 0, Optional Width As Integer = 1)
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
            G.DrawEllipse(Me.Pen, Me.Position.X - Me.Radius, Me.Position.Y - Me.Radius, Me.Radius * 2 + 1, Me.Radius * 2 + 1)
        End If
    End Sub

End Class
