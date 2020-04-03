Imports Microsoft.VisualBasic.Devices
Imports ParticleToy
Imports ParticleToy.Behaviour

Public Class cCollectingBh
    Implements IBehaviour

    Public ReadOnly Property Key As String Implements IBehaviour.Key
        Get
            Return "3"
        End Get
    End Property

    Public Sub NormalizeNot(Particle As Particle, Game As GameBase, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) Implements IBehaviour.NormalizeNot
        Particle.AnchorCenter = Nothing
    End Sub

    Public Sub Normalize(Particle As Particle, Game As GameBase, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) Implements IBehaviour.Normalize
    End Sub

    Public Function Behave(Particle As Particle, Game As GameBase, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) As Boolean Implements IBehaviour.Behave
        With Particle
            .TargetSpeed = APROACH_SPEED
            If Particle.AnchorCenter Is Nothing Then
                Particle.AnchorCenter = CenterOfPoints(.Ancs.Anchors.ToArray)
            End If
            Dim MouseDelta As Double
            If MouseInfo.Position IsNot Nothing Then
                MouseDelta = DeltaBetweed(.CurrentPosition, MouseInfo.Position)
            Else
                MouseDelta = 10000
            End If
            If MouseDelta < ASIDE_RADIUS Then
                .TargetAngel = RndDirectedAngel(XYToDegrees(.CurrentPosition, MouseInfo.Position), 30)
            Else
                .TargetAngel = RndDirectedAngel(XYToDegrees(Particle.AnchorCenter, .CurrentPosition), 90)
            End If
        End With
        Return True
    End Function

End Class
