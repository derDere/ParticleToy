Imports Microsoft.VisualBasic.Devices
Imports ParticleToy
Imports ParticleToy.Behaviour

Public Class cGroupedBh
    Implements IBehaviour

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
            Return "11"
        End Get
    End Property

    Public ReadOnly Property Icon As Bitmap Implements IBehaviour.Icon
        Get
            Return My.Resources.anc_11
        End Get
    End Property

    Public ReadOnly Property Name As String Implements IBehaviour.Name
        Get
            Return "Grouped"
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

            If .Partner Is Nothing OrElse .Partner Is Me Then
                If (.MyIndex Mod GROUP_SIZE) = 0 Then
                    .Partner = .Parent.ParticleL((.MyIndex + 1) Mod .Parent.ParticleL.Count)
                Else
                    .Partner = .Parent.ParticleL(.MyIndex - (.MyIndex Mod GROUP_SIZE))
                End If
                .CurrentColor = RandomColor()
            Else
                If (.MyIndex Mod GROUP_SIZE) = 0 Then
                    .TargetSpeed = MIN_SPEED * 2
                    .TargetAngel = RndDegrees()
                    If (RND.Next(1000) Mod 100) = 0 Then
                        .CurrentColor = RandomColor()
                    End If
                Else
                    Dim PartnerDelta As Double = DeltaBetweed(.CurrentPosition, .Partner.CurrentPosition)
                    If PartnerDelta > ASIDE_RADIUS Then
                        .TargetAngel = RndDirectedAngel(XYToDegrees(.Partner.CurrentPosition, .CurrentPosition), 45)
                        .TargetSpeed = APROACH_SPEED
                    Else
                        .TargetAngel = RndDegrees()
                        .TargetSpeed = MIN_SPEED * 2
                    End If
                    If (RND.Next(1000) Mod 20) = 0 Then
                        .CurrentColor = .Partner.CurrentColor
                    End If
                End If
                If MouseInfo.Position IsNot Nothing AndAlso DeltaBetweed(.CurrentPosition, MouseInfo.Position) < ASIDE_RADIUS Then
                    .TargetAngel = XYToDegrees(.CurrentPosition, MouseInfo.Position)
                End If
                For Each Anc As PointF In .Ancs.Anchors
                    If DeltaBetweed(.CurrentPosition, Anc) < BOUNCE_RADIUS Then
                        .TargetAngel = XYToDegrees(.CurrentPosition, Anc)
                    End If
                Next
            End If
        End With
        Return True
    End Function

End Class
