Public Class KeyBoardInfo
    Inherits Microsoft.VisualBasic.Devices.Keyboard

    Public Shared LastKeys As Keys() = {}
    Public KeysDown As Keys()

    <DebuggerHidden> _
    Public Function Pressed(Key As Keys) As Boolean
        If KeysDown.Contains(Key) And Not LastKeys.Contains(Key) Then
            Return True
        End If
        Return False
    End Function

    <DebuggerHidden> _
    Public Function IsKeyDown(Key As Keys) As Boolean
        If KeysDown.Contains(Key) Then Return True
        Return False
    End Function

End Class
