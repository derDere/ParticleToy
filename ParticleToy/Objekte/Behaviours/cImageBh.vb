Imports Microsoft.VisualBasic.Devices
Imports ParticleToy
Imports ParticleToy.Behaviour

Public Class cImageBh
    Implements IBehaviour

    Friend Image As Bitmap = My.Resources.b2

    Public ReadOnly Property Key As String Implements IBehaviour.Key
        Get
            Return "img"
        End Get
    End Property

    Public Sub NormalizeNot(Particle As Particle, Game As GameBase, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) Implements IBehaviour.NormalizeNot
        Particle.CurrentColor = Particle.Color
    End Sub

    Public Sub Normalize(Particle As Particle, Game As GameBase, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) Implements IBehaviour.Normalize
    End Sub

    Public Function Behave(Particle As Particle, Game As GameBase, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) As Boolean Implements IBehaviour.Behave
        With Particle
            .CurrentColor = New Pen(Image.GetPixel(.CurrentPosition.X, .CurrentPosition.Y).Randomize(50))
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
