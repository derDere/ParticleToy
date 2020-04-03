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

    Public Sub NormalizeNot(Particle As Particle, Game As GameBase, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) Implements IBehaviour.NormalizeNot
    End Sub

    Public Sub Normalize(Particle As Particle, Game As GameBase, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) Implements IBehaviour.Normalize
    End Sub

    Public Function Behave(Particle As Particle, Game As GameBase, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) As Boolean Implements IBehaviour.Behave
        With Particle
            .TargetSpeed = MAX_SPEED
            Dim Rocks As New List(Of Point)
            If MouseInfo.Position IsNot Nothing Then _
                Rocks.Add(MouseInfo.Position)
            Rocks.AddRange(.Ancs.Anchors)
            Dim IsNear As Point? = Nothing
            For Each Rock As Point In Rocks
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
