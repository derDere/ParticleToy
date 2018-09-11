Public Class Menu

    Private _IsOpen As Boolean = False
    Public ReadOnly Property IsOpen
        Get
            Return _IsOpen
        End Get
    End Property

    Public Property Padding As Integer = 20

    Public Property Font As New Font("Courier New", 10)

    Public Property Color As Brush = Brushes.White

    Public Property Background As Brush = New SolidBrush(Drawing.Color.FromArgb(128, 0, 0, 0))

    Private MyGame As Game

    Public Sub New(Game As Game)
        MyGame = Game
    End Sub

    Public Function UpdateAndIsOpen(Tick As Integer, MouseInfo As MouseInfo, KeyBoardInfo As KeyBoardInfo) As Boolean
        If KeyBoardInfo.Pressed(Keys.Escape) Then
            _IsOpen = Not _IsOpen
        End If

        Return IsOpen
    End Function

    Public Sub Draw(G As Graphics)
        If IsOpen Then
            G.FillRectangle(Background, 0, 0, MyGame.ScreenSize.Width, MyGame.ScreenSize.Height)
            G.DrawString(My.Resources.README, Font, Color, Padding, Padding)
        End If
    End Sub

End Class
