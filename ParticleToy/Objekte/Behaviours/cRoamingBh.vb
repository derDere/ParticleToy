Imports Microsoft.VisualBasic.Devices
Imports ParticleToy
Imports ParticleToy.Behaviour

Public Class cRoamingBh
    Implements IBehaviour

    Public ReadOnly Property Key As String Implements IBehaviour.Key
        Get
            Return "0"
        End Get
    End Property

    Public Sub NormalizeNot(Particle As Particle, Game As GameBase, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) Implements IBehaviour.NormalizeNot
    End Sub

    Public Sub Normalize(Particle As Particle, Game As GameBase, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) Implements IBehaviour.Normalize
    End Sub

    Public Function Behave(Particle As Particle, Game As GameBase, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) As Boolean Implements IBehaviour.Behave
        With Particle
            .TargetSpeed = MIN_SPEED
            If MouseInfo.Position IsNot Nothing AndAlso DeltaBetweed(.CurrentPosition, MouseInfo.Position) < ASIDE_RADIUS Then
                .TargetAngel = XYToDegrees(.CurrentPosition, MouseInfo.Position)
            Else
                .TargetAngel = RndDegrees()
            End If
        End With
        Return True
    End Function

End Class
