Imports Microsoft.VisualBasic.Devices
Imports ParticleToy
Imports ParticleToy.Behaviour

Public Class cTeleportBh
    Implements IBehaviour

    Public ReadOnly Property Key As String Implements IBehaviour.Key
        Get
            Return "2"
        End Get
    End Property

    Public Sub NormalizeNot(Particle As Particle, Game As GameBase, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) Implements IBehaviour.NormalizeNot
    End Sub

    Public Sub Normalize(Particle As Particle, Game As GameBase, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) Implements IBehaviour.Normalize
    End Sub

    Public Function Behave(Particle As Particle, Game As GameBase, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) As Boolean Implements IBehaviour.Behave
        With Particle
            .TargetSpeed = APROACH_SPEED
            .TargetAngel = RndDirectedAngel(XYToDegrees(.Ancs.Anchors(0), .CurrentPosition), 180)
            If DeltaBetweed(.CurrentPosition, .Ancs.Anchors(0)) < ASIDE_RADIUS Then
                .Lightnight = .Ancs.Anchors(0)
                .CurrentSpeed = MAX_SPEED
                .CurrentAngel = RndDegrees()
                .CurrentPosition = DegreesToXY(.CurrentAngel, BOUNCE_RADIUS, .Ancs.Anchors(1))
                .CurrentAngel = RndDegrees()
                .LastPosition = .CurrentPosition
            End If
        End With
        Return True
    End Function

End Class
