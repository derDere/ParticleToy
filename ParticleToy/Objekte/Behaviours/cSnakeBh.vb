Imports Microsoft.VisualBasic.Devices
Imports ParticleToy
Imports ParticleToy.Behaviour

Public Class cSnakeBh
    Implements IBehaviour

    Private Const SCALE As Single = 0.0075

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
            Return "snake"
        End Get
    End Property

    Public ReadOnly Property Icon As Bitmap Implements IBehaviour.Icon
        Get
            Return My.Resources.snake
        End Get
    End Property

    Public ReadOnly Property Name As String Implements IBehaviour.Name
        Get
            Return "Snake"
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
        Particle.Partner = Nothing
    End Sub

    'Public Sub Normalize(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) Implements IBehaviour.Normalize
    'End Sub

    Public Sub TurnedOn(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) Implements IBehaviour.TurnedOn
    End Sub

    Public Function Behave(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) As Boolean Implements IBehaviour.Behave
        With Particle
            If .MyIndex = 0 Then
                .CurrentColor = Pens.DarkRed
                .TargetSpeed = MIN_SPEED * 2
                .TargetAngel = RndDirectedAngel(.TargetAngel, 20)

            ElseIf (.MyIndex Mod 1000) = 0 Then
                If .Partner Is Nothing Then
                    .Partner = Game.ParticleL(.MyIndex - 1000)
                    If ((.MyIndex / 1000) Mod 2) = 0 Then
                        .CurrentColor = Pens.CornflowerBlue
                    Else
                        .CurrentColor = Pens.Green
                    End If
                End If
                .TargetAngel = XYToDegreesFastest(.Partner.CurrentPosition, .CurrentPosition, Game.ScreenSize)
                Dim delta As Double = DeltaBetweedFastest(.Partner.CurrentPosition, .CurrentPosition, Game.ScreenSize)
                If delta < 40 Then
                    .TargetSpeed = 0
                Else
                    .TargetSpeed = APROACH_SPEED
                End If

            Else
                If .Partner Is Nothing Then
                    Dim rest As Integer = .MyIndex Mod 1000
                    .Partner = Game.ParticleL(.MyIndex - rest)
                    .CurrentColor = .Partner.CurrentColor.Randomize(100)
                End If
                Dim delta As Double = DeltaBetweedFastest(.Partner.CurrentPosition, .CurrentPosition, Game.ScreenSize)
                If delta < 20 Then
                    .TargetAngel = RndDegrees()
                    .TargetSpeed = MIN_SPEED * 2
                Else
                    .TargetSpeed = APROACH_SPEED
                    .TargetAngel = XYToDegreesFastest(.Partner.CurrentPosition, .CurrentPosition, Game.ScreenSize)
                End If
            End If
        End With
        Return True
    End Function

End Class
