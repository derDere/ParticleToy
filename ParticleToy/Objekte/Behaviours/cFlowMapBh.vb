Imports Microsoft.VisualBasic.Devices
Imports ParticleToy
Imports ParticleToy.Behaviour

Public Class cFlowMapBh
    Implements IBehaviour

    Private Const SCALE As Single = 0.0075

    Public ReadOnly Property Key As String Implements IBehaviour.Key
        Get
            Return "wind"
        End Get
    End Property

    Public ReadOnly Property Icon As Bitmap Implements IBehaviour.Icon
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property Name As String Implements IBehaviour.Name
        Get
            Return "Wind"
        End Get
    End Property

    Public ReadOnly Property ColorManager As IColorManager Implements IBehaviour.ColorManager
        Get
            Return Nothing
        End Get
    End Property

    Public ReadOnly Property OverwriteColorManager As Boolean Implements IBehaviour.OverwriteColorManager
        Get
            Return False
        End Get
    End Property

    Public Sub TurnedOff(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) Implements IBehaviour.TurnedOff
        Game.BG_Alpha = Game.DEFAULT_BG_ALPHA
    End Sub

    'Public Sub Normalize(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) Implements IBehaviour.Normalize
    '    Game.BG_Alpha = 5
    'End Sub

    Public Sub TurnedOn(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) Implements IBehaviour.TurnedOn
        Game.BG_Alpha = 5
        Particle.TargetSpeed = APROACH_SPEED
    End Sub

    Public Function Behave(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) As Boolean Implements IBehaviour.Behave
        With Particle
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
