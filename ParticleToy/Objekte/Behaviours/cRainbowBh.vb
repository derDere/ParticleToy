Imports Microsoft.VisualBasic.Devices
Imports ParticleToy
Imports ParticleToy.Behaviour

Public Class cRainbowBh
    Implements IBehaviour

    Private Const SCALE As Single = 0.0075

    Public ReadOnly Property Key As String Implements IBehaviour.Key
        Get
            Return "rainbow"
        End Get
    End Property

    Public Sub NormalizeNot(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) Implements IBehaviour.NormalizeNot
        Particle.CurrentColor = Particle.Color
    End Sub

    Public Sub Normalize(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) Implements IBehaviour.Normalize
    End Sub

    Public Function Behave(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) As Boolean Implements IBehaviour.Behave
        With Particle
            Dim colorNoise As Single = SimplexNoise.Noise.CalcPixel3D(.CurrentPosition.X, .CurrentPosition.Y, Tick * 2, SCALE) * 2
            .CurrentColor = New Pen(HsvToRgb(colorNoise, 1, 1))

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
