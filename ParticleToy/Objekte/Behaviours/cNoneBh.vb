Imports Microsoft.VisualBasic.Devices
Imports ParticleToy
Imports ParticleToy.Behaviour

Public Class cNoneBh
    Implements IBehaviour

    Public ReadOnly Property Key As String Implements IBehaviour.Key
        Get
            Return "-1"
        End Get
    End Property

    Public Sub NormalizeNot(Particle As Particle, Game As GameBase, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) Implements IBehaviour.NormalizeNot
    End Sub

    Public Sub Normalize(Particle As Particle, Game As GameBase, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) Implements IBehaviour.Normalize
    End Sub

    Public Function Behave(Particle As Particle, Game As GameBase, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) As Boolean Implements IBehaviour.Behave
        With Particle
            If .TargetAngel = 0 Then .TargetAngel = RndDegrees()
            .TargetSpeed = MIN_SPEED
            .CurrentColor = RandomColor()
        End With
        Return True
    End Function

End Class
