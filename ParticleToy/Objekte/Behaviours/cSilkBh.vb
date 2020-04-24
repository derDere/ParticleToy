Imports Microsoft.VisualBasic.Devices
Imports ParticleToy
Imports ParticleToy.Behaviour

Public Class cSilkBh
    Implements IBehaviour

    Private Const SCALE As Double = 0.004
    Private Const RADIUS_OFF As Double = 100000
    Private Const MAX_RADIUS As Double = 25

    Public ReadOnly Property IsSelected As String Implements IBehaviour.IsSelected
        Get
            Return Game.BehaviourKey = Key
        End Get
    End Property

    Public ReadOnly Property IsUnlocked As Boolean Implements IBehaviour.IsUnlocked
        Get
            Return Config.Unlocked.Contains(Key)
        End Get
    End Property

    Public ReadOnly Property Key As String Implements IBehaviour.Key
        Get
            Return "silk"
        End Get
    End Property

    Public ReadOnly Property Icon As Bitmap Implements IBehaviour.Icon
        Get
            Return My.Resources.silk
        End Get
    End Property

    Public ReadOnly Property Name As String Implements IBehaviour.Name
        Get
            Return "Silk"
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
        Particle.DrawHiddenLinePoint = Particle.DEFAULT_DRAW_HIDDEN_LINE_POINT
        Particle.MinMovementDrawLength = Particle.DEFAULT_MIN_MOVEMENT_DRAW_LENGTH
    End Sub

    Public Sub TurnedOn(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) Implements IBehaviour.TurnedOn
        Particle.DrawHiddenLinePoint = True
        Particle.MinMovementDrawLength = 1
    End Sub

    Public Function Behave(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) As Boolean Implements IBehaviour.Behave
        With Particle
            Dim mouseDelta As Double = 1000000
            If MouseInfo.Position IsNot Nothing Then _
                mouseDelta = DeltaBetweed(.SilkPos, MouseInfo.Position) / 10
            Dim noiseAngle As Double = SimplexNoise.Noise.CalcPixel3D(.SilkPos.X, .SilkPos.Y, Tick + mouseDelta, SCALE) / 255
            Dim noiseRadius As Double = SimplexNoise.Noise.CalcPixel3D(.SilkPos.X + RADIUS_OFF, .SilkPos.Y + RADIUS_OFF, Tick + mouseDelta + RADIUS_OFF, SCALE) / 255

            'Dim mouseDeltaX As Double = 0
            'Dim mouseDeltaY As Double = 0
            '
            'If MouseInfo.Position IsNot Nothing Then
            '    mouseDeltaX = (MouseInfo.Position.Value.X - .SilkPos.X)
            '    mouseDeltaY = (MouseInfo.Position.Value.Y - .SilkPos.Y)
            'End If
            '
            'Dim noiseAngle As Double = SimplexNoise.Noise.CalcPixel3D(mouseDeltaX, mouseDeltaY, Tick, SCALE) / 255
            'Dim noiseRadius As Double = SimplexNoise.Noise.CalcPixel3D(mouseDeltaX + RADIUS_OFF, mouseDeltaY + RADIUS_OFF, Tick + RADIUS_OFF, SCALE) / 255

            Dim targetPoint As PointF = DegreesToXY(360 * noiseAngle, MAX_RADIUS * noiseRadius, .SilkPos)
            Dim delta As Double = DeltaBetweedFastest(targetPoint, .CurrentPosition, Game.ScreenSize)

            .TargetAngel = XYToDegreesFastest(targetPoint, .CurrentPosition, Game.ScreenSize)
            .CurrentAngel = .TargetAngel

            If delta >= MAX_SPEED Then
                .TargetSpeed = MAX_SPEED
            ElseIf delta >= APROACH_SPEED Then
                .TargetSpeed = APROACH_SPEED
            ElseIf delta >= ANTS_SPEED Then
                .TargetSpeed = ANTS_SPEED
            ElseIf delta >= MIN_SPEED Then
                .TargetSpeed = MIN_SPEED
            Else
                .TargetSpeed = delta
            End If
        End With
        Return True
    End Function

End Class
