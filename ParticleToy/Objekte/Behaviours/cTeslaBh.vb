Imports Microsoft.VisualBasic.Devices
Imports ParticleToy
Imports ParticleToy.Behaviour

Public Class cTeslaBh
    Implements IBehaviour

    Public ReadOnly Property Key As String Implements IBehaviour.Key
        Get
            Return "1"
        End Get
    End Property

    Public Sub NormalizeNot(Particle As Particle, Game As GameBase, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) Implements IBehaviour.NormalizeNot
    End Sub

    Public Sub Normalize(Particle As Particle, Game As GameBase, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) Implements IBehaviour.Normalize
    End Sub

    Public Function Behave(Particle As Particle, Game As GameBase, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) As Boolean Implements IBehaviour.Behave
        With Particle
            .TargetSpeed = APROACH_SPEED
            Dim DeltaToAnc As Double = DeltaBetweed(.CurrentPosition, .Ancs.Anchors(0))
            If DeltaToAnc < (BOUNCE_RADIUS + 20) Then
                If DeltaToAnc < BOUNCE_RADIUS Then
                    .CurrentSpeed = MAX_SPEED
                    .TargetAngel = XYToDegrees(.CurrentPosition, .Ancs.Anchors(0))
                    .CurrentAngel = .TargetAngel
                    .Lightnight = .Ancs.Anchors(0)
                Else
                    .TargetAngel = RndDegrees()
                End If
            Else
                .TargetAngel = RndDirectedAngel(XYToDegrees(.Ancs.Anchors(0), .CurrentPosition), 45)
            End If
        End With
        Return True
    End Function

End Class
