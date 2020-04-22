Imports Microsoft.VisualBasic.Devices
Imports ParticleToy
Imports ParticleToy.Behaviour

Public Class cRingBh
    Implements IBehaviour

    Private Const ALPHA As Integer = 32
    Private Const SCALE As Single = 0.075
    Private Const APPROACH_ANGLE As Integer = 10
    Private Const FLEE_ANGLE As Double = 2.5
    Private Const ANGLE_RANDOMIZATION As Integer = 20
    Private Const JUMP_ANGLE As Integer = 60

    Private Center As New PointF(Game.OPT_SIZE_W / 2, Game.OPT_SIZE_H / 2)
    Private Radius As Double = IIf(Game.OPT_SIZE_H < Game.OPT_SIZE_W, Game.OPT_SIZE_H / 4, Game.OPT_SIZE_W / 4)

    Public ReadOnly Property Key As String Implements IBehaviour.Key
        Get
            Return "ring"
        End Get
    End Property

    Public ReadOnly Property Icon As Bitmap Implements IBehaviour.Icon
        Get
            Return My.Resources.ring
        End Get
    End Property

    Public ReadOnly Property Name As String Implements IBehaviour.Name
        Get
            Return "The Ring"
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
    End Sub

    Public Sub TurnedOn(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) Implements IBehaviour.TurnedOn
        Particle.CurrentAngel = XYToDegrees(Center, Particle.CurrentPosition)
        Particle.CurrentSpeed = MAX_SPEED * (RND.Next(1000, 9999) Mod 5) * 2.5
        Particle.TargetSpeed = 4 + (RND.Next(1000, 9999) Mod 5)
    End Sub

    Public Function Behave(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) As Boolean Implements IBehaviour.Behave
        With Particle
            Dim delta As Double = DeltaBetweedFastest(Center, .CurrentPosition, Game.ScreenSize)
            Dim direction As Integer = IIf((.MyIndex Mod 2) = 0, 1, -1)
            If delta > Radius Then
                .TargetAngel = RndDirectedAngel(ValidDegrees(XYToDegreesFastest(Center, .CurrentPosition, Game.ScreenSize) + ((90 - APPROACH_ANGLE) * direction)), ANGLE_RANDOMIZATION)
            Else
                .TargetAngel = RndDirectedAngel(ValidDegrees(XYToDegreesFastest(Center, .CurrentPosition, Game.ScreenSize) + ((90 + FLEE_ANGLE) * direction)), ANGLE_RANDOMIZATION)
            End If
            If chance(50) Then
                .CurrentAngel += ((RND.Next(1000, 9999) Mod JUMP_ANGLE) - (JUMP_ANGLE / 8))
                .CurrentSpeed = MAX_SPEED * (((RND.Next(100000, 999999) Mod 2000D) + 500D) / 1000D)
            End If
            If chance(10000) Then
                Dim x As Integer
                If chance(2) Then
                    x = -10000
                Else
                    x = 10000
                End If
                .Lightnight = New Point(x, .CurrentPosition.Y)
            End If
            Dim noise As Single = SimplexNoise.Noise.CalcPixel1D(Tick, SCALE)
            If Math.Round(noise) Mod 50 = 0 Then
                .CurrentColor = Pens.Black
            Else
                .CurrentColor = Color.FromArgb(ALPHA, 3, 234, 255).Randomize(100).ToPen
            End If
        End With
        Return True
    End Function

End Class
