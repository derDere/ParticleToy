Public Class GameConsole

    Const MAX_LINES As Integer = 40

    Public Property State As GameBase.ConsoleState = GameBase.ConsoleState.Closed

    Private Lines As New List(Of ConsoleLine)

    Public Property Padding As Integer = 10

    Public Property Font As New Font("Courier New", 8)

    Public Property Background As Brush = New SolidBrush(Drawing.Color.FromArgb(128, 0, 0, 0))

    Public Property EnteredMode As String = ""

    Private MyGame As Game

    Public Sub New(Game As GameBase)
        MyGame = Game
    End Sub

    Public Sub AddLine(ParamArray LineArr As String())
        AddLine(Brushes.White, LineArr)
    End Sub

    Public Sub AddLine(Color As Brush, ParamArray LineArr As String())
        For Each Line As String In LineArr
            For Each L As String In Line.Split({vbCr, vbLf}, StringSplitOptions.RemoveEmptyEntries)
                Lines.Add(New ConsoleLine With {
                    .Line = L,
                    .Color = Color
                })
            Next
        Next
        While Lines.Count > MAX_LINES
            Lines.RemoveAt(0)
        End While
    End Sub

    Public Sub AddLine(ParamArray LineArr As ConsoleLine())
        For Each CL As ConsoleLine In LineArr
            AddLine(CL.Color, CL.Line)
        Next
    End Sub

    Public Sub Draw(G As Graphics)
        If State = GameBase.ConsoleState.Open Then
            Dim LineSize As SizeF = G.MeasureString("W", Font)
            G.FillRectangle(Background, 0, 0, MyGame.ScreenSize.Width, CInt(Math.Round(LineSize.Height) * Lines.Count) + (2 * Padding))
            Dim y As Integer = Padding
            For Each Line As ConsoleLine In Lines
                LineSize = G.MeasureString(Line.Line, Font)
                G.DrawString(Line.Line, Font, Line.Color, Padding, y)
                y += LineSize.Height
            Next
            G.DrawLine(Pens.Gray, 0, y + Padding, MyGame.ScreenSize.Width, y + Padding)
        End If
    End Sub

    Public Structure ConsoleLine
        Public Line As String
        Public Color As Brush
    End Structure

End Class
