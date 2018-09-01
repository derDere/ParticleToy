Public Class MouseInfo

    Private Shared LastLeftIsPressed As Boolean = False
    Private Shared LastRightIsPressed As Boolean = False
    Private Shared LastMiddleIsPressed As Boolean = False

    Public ReadOnly PressedLeftIsPressed As Boolean = False
    Public ReadOnly PressedRightIsPressed As Boolean = False
    Public ReadOnly PressedMiddleIsPressed As Boolean = False

    Public ReadOnly LeftIsPressed As Boolean
    Public ReadOnly RightIsPressed As Boolean
    Public ReadOnly MiddleIsPressed As Boolean
    Public ReadOnly Position As Point?
    Public ReadOnly WeelDelta As Integer
    Public ReadOnly Device As Microsoft.VisualBasic.Devices.Mouse

    Public Sub New(Lip As Boolean, Rip As Boolean, Mip As Boolean, Pos As Point?, WD As Integer, Div As Microsoft.VisualBasic.Devices.Mouse)
        Me.LeftIsPressed = Lip
        Me.RightIsPressed = Rip
        Me.MiddleIsPressed = Mip
        Me.Position = Pos
        Me.WeelDelta = WD
        Me.Device = Div

        If Me.LeftIsPressed And Not LastLeftIsPressed Then PressedLeftIsPressed = True
        If Me.RightIsPressed And Not LastRightIsPressed Then PressedRightIsPressed = True
        If Me.MiddleIsPressed And Not LastMiddleIsPressed Then PressedMiddleIsPressed = True

        LastLeftIsPressed = Me.LeftIsPressed
        LastRightIsPressed = Me.RightIsPressed
        LastMiddleIsPressed = Me.MiddleIsPressed
    End Sub

End Class
