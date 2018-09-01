Public Class Menu

    Private Const MARGIN As Integer = 20
    Private Const BTN_WIDTH As Integer = 200
    Private Const BTN_HEIGHT As Integer = 40
    Private Const BTN_SPACE As Integer = 10

    Private _IsOpen As Boolean = False
    Public ReadOnly Property IsOpen
        Get
            Return _IsOpen
        End Get
    End Property

    Public Property TitleFont As New Font("Arial", 10)

    Public Title As String = "-= PAUSED =-"

    Public MenuBtns As New List(Of MenuBtn) From {New MenuBtn("Close")}

    Private MyGame As Game

    Public Sub New(Game As Game)
        MyGame = Game
    End Sub

    Public Function UpdateAndIsOpen(Tick As Integer, MouseInfo As MouseInfo, KeyBoardInfo As KeyBoardInfo) As Boolean
        If KeyBoardInfo.Pressed(Keys.Escape) Then
            _IsOpen = Not _IsOpen
        End If
        If IsOpen Then

        End If
        Return IsOpen
    End Function

    Private Shared BgBru As New SolidBrush(Color.FromArgb(128, 0, 0, 0))

    Public Sub Draw(G As Graphics)
        If IsOpen Then
            Dim W As Integer = MARGIN + BTN_WIDTH + MARGIN
            Dim H As Integer = MARGIN + MARGIN
            Dim TitleS As SizeF = G.MeasureString(Title, TitleFont)
            H += Math.Round(TitleS.Height)
            For Each MI As MenuBtn In MenuBtns
                H += BTN_SPACE + BTN_HEIGHT
            Next
            Dim Bounds As New Rectangle((MyGame.ScreenSize.Width / 2) - (W / 2), (MyGame.ScreenSize.Height / 2) - (H / 2), W, H)

            G.FillRectangle(BgBru, Bounds)
            G.DrawRectangle(Pens.Cyan, Bounds)
            G.DrawString(Title, TitleFont, Brushes.White, Bounds.X + ((Bounds.Width / 2) - TitleS.Width / 2), Bounds.Y + MARGIN)

            For I As Integer = 0 To MenuBtns.Count - 1
                Dim MI As MenuBtn = MenuBtns(I)
                Dim BtnBounds As New Rectangle(Bounds.X + MARGIN, Bounds.Y + MARGIN + TitleS.Height + BTN_SPACE + (I * (BTN_SPACE + BTN_HEIGHT)), BTN_WIDTH, BTN_HEIGHT)
                G.DrawRectangle(Pens.Cyan, BtnBounds)
                Dim TextS As SizeF = G.MeasureString(MI.Text, MI.Font)
                G.DrawString(MI.Text, MI.Font, Brushes.White, BtnBounds.X + (BtnBounds.Width / 2) - (TextS.Width / 2), BtnBounds.Y + (BtnBounds.Height / 2) - (TextS.Height / 2))
            Next
        End If
    End Sub

End Class

Public Class MenuBtn

    Public Const PADDING As Integer = 5

    Public Property Font As New Font("Arial", 15)
    Public Property Text As String

    Public Sub New(Text As String)
        Me.Text = Text
    End Sub

End Class
