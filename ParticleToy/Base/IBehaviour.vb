Public Interface IBehaviour

    ReadOnly Property Key As String

    Sub NormalizeNot(Particle As Particle, Game As GameBase, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Microsoft.VisualBasic.Devices.Keyboard)

    Sub Normalize(Particle As Particle, Game As GameBase, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Microsoft.VisualBasic.Devices.Keyboard)

    Function Behave(Particle As Particle, Game As GameBase, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Microsoft.VisualBasic.Devices.Keyboard) As Boolean

End Interface

Namespace Behaviour
    Public Module BehaviourValues

        Public Const MAX_SPEED As Double = 10
        Public Const MIN_SPEED As Double = 1
        Public Const APROACH_SPEED As Double = 4
        Public Const TURN_SPEED As Double = 20
        Public Const BOUNCE_RADIUS As Double = 50
        Public Const ASIDE_RADIUS As Double = 20
        Public Const GROUP_SIZE As Integer = 100

        <DebuggerHidden>
        Public Function RndSpeed() As Double
            Return (RND.Next(1000) Mod ((MAX_SPEED - MIN_SPEED) * 0.75)) + MIN_SPEED
        End Function

        Public Class ExitSub
            Inherits Exception
        End Class

    End Module
End Namespace
