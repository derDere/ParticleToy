Imports Microsoft.VisualBasic.Devices
Imports ParticleToy
Imports ParticleToy.Behaviour

Public Class cCyclingBh
    Implements IBehaviour

    Public ReadOnly Property Key As String Implements IBehaviour.Key
        Get
            Return "4"
        End Get
    End Property

    Public ReadOnly Property Icon As Bitmap Implements IBehaviour.Icon
        Get
            Return My.Resources.anc_4
        End Get
    End Property

    Public ReadOnly Property Name As String Implements IBehaviour.Name
        Get
            Return "Cycling"
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
        With Particle
            .AnchorIndex = -1
            .AnchorStep = 0
            If Particle.SortedAnchors.Count > 0 Then _
                Particle.SortedAnchors.Clear()
        End With
    End Sub

    'Public Sub Normalize(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) Implements IBehaviour.Normalize
    'End Sub

    Public Sub TurnedOn(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) Implements IBehaviour.TurnedOn
        Particle.TargetSpeed = RndSpeed()
    End Sub

    Public Function Behave(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) As Boolean Implements IBehaviour.Behave
        With Particle
            If Particle.SortedAnchors.Count = 0 Then
                Particle.SortAnchors(.Ancs)
            End If
            If .AnchorIndex = -1 Then
                .AnchorStep = IIf((RND.Next(1000) Mod 2) = 0, 1, -1)
                Dim MinD As Double = Double.MaxValue
                For i As Integer = 0 To Particle.SortedAnchors.Count - 1
                    Dim D As Double = DeltaBetweed(.CurrentPosition, Particle.SortedAnchors(i))
                    If D < MinD Then
                        MinD = D
                        .AnchorIndex = i
                    End If
                Next
            Else
                Dim AnchorDelta As Double = DeltaBetweed(.CurrentPosition, Particle.SortedAnchors(.AnchorIndex))
                If AnchorDelta < BOUNCE_RADIUS Then
                    .Lightnight = Particle.SortedAnchors(.AnchorIndex)
                    .AnchorIndex += .AnchorStep
                    If .AnchorIndex >= Particle.SortedAnchors.Count Then .AnchorIndex = 0
                    If .AnchorIndex < 0 Then .AnchorIndex = Particle.SortedAnchors.Count - 1
                End If
                .TargetAngel = RndDirectedAngel(XYToDegrees(Particle.SortedAnchors(.AnchorIndex), .CurrentPosition), 135)
            End If
        End With
        Return True
    End Function

End Class
