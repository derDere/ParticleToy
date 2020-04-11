Imports Microsoft.VisualBasic.Devices
Imports ParticleToy
Imports ParticleToy.Behaviour

Public Class cSyncedBh
    Implements IBehaviour

    Public ReadOnly Property Key As String Implements IBehaviour.Key
        Get
            Return "7"
        End Get
    End Property

    Public ReadOnly Property Icon As Bitmap Implements IBehaviour.Icon
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property Name As String Implements IBehaviour.Name
        Get
            Return "Synced"
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

    'Public Sub Normalize(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) Implements IBehaviour.Normalize
    'End Sub

    Public Sub TurnedOn(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) Implements IBehaviour.TurnedOn
        Particle.TargetSpeed = MAX_SPEED
    End Sub

    Public Function Behave(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) As Boolean Implements IBehaviour.Behave
        With Particle
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
            If IsNear IsNot Nothing OrElse (Tick Mod 70) = 0 Then
                .TargetAngel = Particle.ElectricAngles(RND.Next(1000) Mod Particle.ElectricAngles.Length)
            End If
        End With
        Return True
    End Function

End Class
