Imports Microsoft.VisualBasic.Devices
Imports ParticleToy
Imports ParticleToy.Behaviour

Public Class cPixelBh
    Implements IBehaviour

    Private Const SCALE As Double = 0.0075
    Private Const SCALE_1 As Double = 2 * SCALE
    Private Const SCALE_2 As Double = 4 * SCALE
    Private Const SCALE_3 As Double = 10 * SCALE
    Private Const SPEED As Double = 2

    Public ReadOnly Property Key As String Implements IBehaviour.Key
        Get
            Return "pixel"
        End Get
    End Property

    Public ReadOnly Property Icon As Bitmap Implements IBehaviour.Icon
        Get
            Return My.Resources.pixel
        End Get
    End Property

    Public ReadOnly Property Name As String Implements IBehaviour.Name
        Get
            Return "Pixel"
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
        Particle.AnchorIndex = 0
    End Sub

    Public Sub TurnedOn(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) Implements IBehaviour.TurnedOn
        Particle.AnchorIndex = RND.Next(1000, 9999) Mod 4
    End Sub

    Public Const XOff As Double = (Particle.SILK_GRID_CELL_WIDTH * 0.5)
    Public Const YOff As Double = (Particle.SILK_GRID_CELL_HEIGHT * 0.5)
    Public Const PixW As Double = (Particle.SILK_GRID_CELL_WIDTH * 0.75)
    Public Const PixH As Double = (Particle.SILK_GRID_CELL_HEIGHT * 0.75)

    Public Function Behave(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) As Boolean Implements IBehaviour.Behave
        With Particle

            Dim targetPoint As PointF

            Dim m As Integer = 1
            If .MyIndex Mod 2 = 0 Then m = -1
            .AnchorIndex += m
            If .AnchorIndex < 0 Then .AnchorIndex += 4
            .AnchorIndex = .AnchorIndex Mod 4

            Select Case .AnchorIndex
                Case 3
                    targetPoint = New PointF(.SilkPos.X - XOff, .SilkPos.Y + PixH - YOff)
                Case 2
                    targetPoint = New PointF(.SilkPos.X - XOff + PixW, .SilkPos.Y + PixH - YOff)
                Case 1
                    targetPoint = New PointF(.SilkPos.X - XOff + PixW, .SilkPos.Y - YOff)
                Case Else
                    targetPoint = New PointF(.SilkPos.X - XOff, .SilkPos.Y - YOff)
            End Select

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

            Dim n1 As Double = SimplexNoise.Noise.CalcPixel3D(.SilkPos.X, .SilkPos.Y, Tick * SPEED, SCALE_1)
            Dim n2 As Double = SimplexNoise.Noise.CalcPixel3D(.SilkPos.X, .SilkPos.Y, Tick * SPEED, SCALE_2)
            Dim n3 As Double = SimplexNoise.Noise.CalcPixel3D(.SilkPos.X, .SilkPos.Y, Tick * SPEED, SCALE_3)
            Dim noise As Integer = Math.Floor((n1 + n2 + n3) / 3)
            .CurrentColor = Color.FromArgb(255, noise, noise, noise).ToPen

        End With
        Return True
    End Function

End Class
