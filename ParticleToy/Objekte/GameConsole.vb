Public Class GameConsole

    Const MAX_LINES As Integer = 40
    Const MAX_LINE_LIFE_TIME As Integer = 300

    Public Property Padding As Integer = 10
    Public Property Font As New Font("Courier New", 8)
    Public Property Background As Brush = New SolidBrush(Drawing.Color.FromArgb(128, 0, 0, 0))
    Public Property EnteredMode As String = ""

    Private AllLines As New List(Of ConsoleLine)
    Private DrawLines As ConsoleLine() = {}
    Private MyGame As Game
    Private LineLength As Integer = 0
    Private LineDrawCount As Integer = 1

    Public Sub New(Game As GameBase)
        MyGame = Game
    End Sub

    Public Sub AddLine(ParamArray LineArr As String())
        AddLine(Brushes.White, LineArr)
    End Sub

    Public Sub AddLine(Color As Brush, ParamArray LineArr As String())
        For Each Line As String In LineArr
            For Each L As String In Line.Split({vbCr, vbLf}, StringSplitOptions.RemoveEmptyEntries)
                AllLines.Add(New ConsoleLine With {
                    .Line = L,
                    .Color = Color,
                    .Tick = MyGame.CurrentTick
                })
            Next
        Next
        While AllLines.Count > MAX_LINES
            AllLines.RemoveAt(0)
            LineDrawCount -= 1
        End While
    End Sub

    Public Sub AddLine(ParamArray LineArr As ConsoleLine())
        For Each CL As ConsoleLine In LineArr
            AddLine(CL.Color, CL.Line)
        Next
    End Sub

    Public Sub Update(Game As GameBase, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Microsoft.VisualBasic.Devices.Keyboard)
        While AllLines.Count > 0 AndAlso AllLines.First.Tick < (MyGame.CurrentTick - MAX_LINE_LIFE_TIME - AllLines.First.Line.Length)
            AllLines.RemoveAt(0)
            LineDrawCount -= 1
        End While
        If AllLines.Count > 0 Then
            Dim lineIndex As Integer = LineDrawCount - 1
            If lineIndex >= 0 And lineIndex < AllLines.Count Then
                If LineLength <= AllLines(lineIndex).Line.Length Then
                    LineLength += 1
                End If
                If LineLength > AllLines(lineIndex).Line.Length Then
                    LineLength = 0
                    LineDrawCount += 1
                End If
                DrawLines = AllLines.Take(LineDrawCount).ToArray
            End If
        Else
            DrawLines = {}
        End If
    End Sub

    Public Sub Draw(G As Graphics)
        Dim LineSize As SizeF = G.MeasureString("W", Font)
        For I As Integer = 0 To LineDrawCount - 2
            If I < DrawLines.Count Then
                Dim X As Integer = Padding
                Dim Y As Integer = MyGame.ScreenSize.Height - Padding - ((DrawLines.Count - I) * LineSize.Height)
                G.DrawString(DrawLines(I).Line, Font, DrawLines(I).Color, X, Y)
            End If
        Next
        If DrawLines.Count > 0 Then
            G.DrawString(DrawLines.Last.Line.Substring(0, LineLength), Font, DrawLines.Last.Color, Padding, MyGame.ScreenSize.Height - Padding - LineSize.Height)
        End If
        'G.FillRectangle(Background, 0, 0, MyGame.ScreenSize.Width, CInt(Math.Round(LineSize.Height) * Lines.Count) + (2 * Padding))
        'Dim y As Integer = Padding
        'For Each Line As ConsoleLine In Lines
        '    LineSize = G.MeasureString(Line.Line, Font)
        '    G.DrawString(Line.Line, Font, Line.Color, Padding, y)
        '    y += LineSize.Height
        'Next
        'G.DrawLine(Pens.Gray, 0, y + Padding, MyGame.ScreenSize.Width, y + Padding)
    End Sub

    Public Structure ConsoleLine
        Public Line As String
        Public Color As Brush
        Public Tick As Integer
    End Structure

End Class
