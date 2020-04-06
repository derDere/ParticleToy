Public Class ZipSystemManager

    Public Sub ExecuteCommand(Console As GameConsole, Command As String)
        If String.IsNullOrEmpty(Command) Or String.IsNullOrWhiteSpace(Command) Then Exit Sub
        Command = Command.ToLower
        Console.AddLine(Brushes.Yellow, ": " & Command)
        Console.AddLine(Brushes.White, "Mode Set")
        Console.EnteredMode = Command
    End Sub

End Class
