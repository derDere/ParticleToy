Imports Microsoft.VisualBasic.Devices
Imports ParticleToy
Imports ParticleToy.Behaviour

Public Class cFlowMapBh
    Implements IBehaviour

    Private Const SCALE As Single = 0.005

    Public ReadOnly Property Key As String Implements IBehaviour.Key
        Get
            Return "wind"
        End Get
    End Property

    Public Sub NormalizeNot(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) Implements IBehaviour.NormalizeNot
        Game.BG_Alpha = Game.DEFAULT_BG_ALPHA
        Particle.TargetSpeed = MIN_SPEED
        Particle.CurrentColor = Particle.Color
        Particle.SpeedIsSet = False
    End Sub

    Public Sub Normalize(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) Implements IBehaviour.Normalize
        Game.BG_Alpha = 5
    End Sub

    Public Function Behave(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) As Boolean Implements IBehaviour.Behave
        With Particle
            If Not .SpeedIsSet Then
                .TargetSpeed = APROACH_SPEED
                .SpeedIsSet = True
            End If
            Dim noise As Single = SimplexNoise.Noise.CalcPixel3D(.CurrentPosition.X, .CurrentPosition.Y, Tick, SCALE)
            Dim m As Double = noise / 255
            Dim angle As Double = 360 * m
            '.CurrentAngel = angle 'RndDirectedAngel(angle, 90)
            '.TargetAngel = .CurrentAngel
            .TargetAngel = RndDirectedAngel(angle, 90)
        End With
        Return True
    End Function

End Class
