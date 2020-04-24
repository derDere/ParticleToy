Imports Microsoft.VisualBasic.Devices
Imports ParticleToy
Imports ParticleToy.Behaviour

Public Class cCollectingBh
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
            Return "3"
        End Get
    End Property

    Public ReadOnly Property Icon As Bitmap Implements IBehaviour.Icon
        Get
            Return My.Resources.anc_3
        End Get
    End Property

    Public ReadOnly Property Name As String Implements IBehaviour.Name
        Get
            Return "Center Collecting"
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
        Particle.AnchorCenter = Nothing
    End Sub

    'Public Sub Normalize(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) Implements IBehaviour.Normalize
    'End Sub

    Public Sub TurnedOn(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) Implements IBehaviour.TurnedOn
        Particle.TargetSpeed = APROACH_SPEED
    End Sub

    Public Function Behave(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) As Boolean Implements IBehaviour.Behave
        With Particle
            If Particle.AnchorCenter Is Nothing Then
                Particle.AnchorCenter = CenterOfPoints(.Ancs.Anchors.ToArray)
            End If
            Dim MouseDelta As Double
            If MouseInfo.Position IsNot Nothing Then
                MouseDelta = DeltaBetweed(.CurrentPosition, MouseInfo.Position)
            Else
                MouseDelta = 10000
            End If
            If MouseDelta < ASIDE_RADIUS Then
                .TargetAngel = RndDirectedAngel(XYToDegrees(.CurrentPosition, MouseInfo.Position), 30)
            Else
                .TargetAngel = RndDirectedAngel(XYToDegrees(Particle.AnchorCenter, .CurrentPosition), 90)
            End If
        End With
        Return True
    End Function

End Class
