Imports Microsoft.VisualBasic.Devices
Imports ParticleToy
Imports ParticleToy.Behaviour

Public Class cGameOfLifeBh
    Implements IBehaviour

    Public ReadOnly Property Key As String Implements IBehaviour.Key
        Get
            Return "12"
        End Get
    End Property

    Public ReadOnly Property Icon As Bitmap Implements IBehaviour.Icon
        Get
            Return My.Resources.game_of_life
        End Get
    End Property

    Public ReadOnly Property Name As String Implements IBehaviour.Name
        Get
            Return "Game of Life"
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
        'Particle.GolStatusSet = False
        Particle.WillGlow = False
        Particle.Glowing = False
        Particle.GolPositionFound = False
        Particle.DrawLineDelta = 0
    End Sub

    'Public Sub Normalize(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) Implements IBehaviour.Normalize
    '    Particle.HideNoMovement = False
    'End Sub

    Public Sub TurnedOn(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) Implements IBehaviour.TurnedOn
        Particle.DrawLineDelta = 0
        Particle.WillGlow = (RND.Next(1000) Mod 5) = 0
        Particle.Glowing = IIf(Particle.WillGlow, 100, 0)
    End Sub

    Public Function Behave(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) As Boolean Implements IBehaviour.Behave
        With Particle
            'If Not .GolStatusSet Then
            '    .WillGlow = (RND.Next(1000) Mod 5) = 0
            '    .Glowing = IIf(.WillGlow, 100, 0)
            '    .GolStatusSet = True
            'Else
            If (Tick Mod 2) = 0 Then
                    Dim Sum As Integer = 0

                    If .GolMN(.GolMP.X - 1, .GolMP.Y - 1).Glowing Then Sum += 1
                    If .GolMN(.GolMP.X - 0, .GolMP.Y - 1).Glowing Then Sum += 1
                    If .GolMN(.GolMP.X + 1, .GolMP.Y - 1).Glowing Then Sum += 1
                    If .GolMN(.GolMP.X - 1, .GolMP.Y - 0).Glowing Then Sum += 1
                    If .GolMN(.GolMP.X + 1, .GolMP.Y - 0).Glowing Then Sum += 1
                    If .GolMN(.GolMP.X - 1, .GolMP.Y + 1).Glowing Then Sum += 1
                    If .GolMN(.GolMP.X - 0, .GolMP.Y + 1).Glowing Then Sum += 1
                    If .GolMN(.GolMP.X + 1, .GolMP.Y + 1).Glowing Then Sum += 1

                    If Sum < 2 Then .WillGlow = False
                    If Sum = 3 Then .WillGlow = True
                    If Sum > 3 Then .WillGlow = False

                    If MouseInfo.Position IsNot Nothing Then
                        Dim MouseDelta As Double = DeltaBetweed(.CurrentPosition, MouseInfo.Position)
                        If MouseDelta < ASIDE_RADIUS Then
                            If (RND.Next(1000) Mod 20) = 0 Then
                                .WillGlow = True
                            End If
                        End If
                    End If
                End If
                'End If
                .TargetSpeed = MAX_SPEED
            Dim delta As Double = DeltaBetweed(.CurrentPosition, .GolPos)
            If delta < BOUNCE_RADIUS * 2 Then
                .TargetSpeed = APROACH_SPEED
            End If
            If delta < BOUNCE_RADIUS Then
                .TargetSpeed = MIN_SPEED
                .CurrentSpeed = MIN_SPEED
            End If
            If delta < APROACH_SPEED Then
                .CurrentPosition = .GolPos
                .CurrentSpeed = 0
                .TargetSpeed = 0
                .GolPositionFound = True
            Else
                .TargetAngel = XYToDegrees(.GolPos, .CurrentPosition)
            End If
            If .GolPositionFound Then
                Dim AncDelta As Double = Double.MaxValue
                For Each Anc As PointF In .Ancs.Anchors
                    Dim d As Double = DeltaBetweed(.GolPos, Anc)
                    If d < AncDelta Then AncDelta = d
                Next
                Dim m As Double = Math.Cos((AncDelta) / 10 + (-Tick / 10))
                .CurrentPosition = New PointF(.GolPos.X, .GolPos.Y - (5 * m))
            End If
        End With
        Return True
    End Function

End Class
