Imports Microsoft.VisualBasic.Devices
Imports ParticleToy
Imports ParticleToy.Behaviour

Public Class cStaddyBh
    Implements IBehaviour

    Public ReadOnly Property Key As String Implements IBehaviour.Key
        Get
            Return "8"
        End Get
    End Property

    Public ReadOnly Property Icon As Bitmap Implements IBehaviour.Icon
        Get
            Return My.Resources.anc_8
        End Get
    End Property

    Public ReadOnly Property Name As String Implements IBehaviour.Name
        Get
            Return "Staddy"
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
        Particle.MinMovementDrawLength = 0
    End Sub

    'Public Sub Normalize(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) Implements IBehaviour.Normalize
    'End Sub

    Public Sub TurnedOn(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) Implements IBehaviour.TurnedOn
        Particle.MinMovementDrawLength = 1
    End Sub

    Public Function Behave(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) As Boolean Implements IBehaviour.Behave
        With Particle
            .TargetSpeed = 0
            .TargetAngel = RndDegrees()
            Dim Rocks As New List(Of PointF)
            If MouseInfo.Position IsNot Nothing Then _
                Rocks.Add(MouseInfo.Position)
            Rocks.AddRange(.Ancs.Anchors)
            Dim IsNear As PointF? = Nothing
            For Each Rock As PointF In Rocks
                Dim delta As Double = DeltaBetweed(Rock, .CurrentPosition)
                If delta < ASIDE_RADIUS Then
                    IsNear = Rock
                    Exit For
                End If
            Next
            If IsNear IsNot Nothing Then
                .CurrentSpeed = RndSpeed()
            End If
            If (RND.Next(1000000) Mod 1000) = 0 Then
                .CurrentPosition = DegreesToXY(RndDegrees, ASIDE_RADIUS, .Ancs.Anchors(RND.Next(1000) Mod .Ancs.Anchors.Count))
                .LastPosition = .CurrentPosition
            End If
            If (RND.Next(1000000) Mod 2000) = 0 Then
                .CurrentSpeed = APROACH_SPEED
            End If
            Dim G As Integer = 255 - (RND.Next(1000) Mod 64)
            .CurrentColor = New Pen(Drawing.Color.FromArgb(255, 255, G, 192))
        End With
        Return True
    End Function

End Class
