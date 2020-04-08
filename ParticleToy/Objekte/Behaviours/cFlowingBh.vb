Imports Microsoft.VisualBasic.Devices
Imports ParticleToy
Imports ParticleToy.Behaviour

Public Class cFlowingBh
    Implements IBehaviour

    Public ReadOnly Property Key As String Implements IBehaviour.Key
        Get
            Return "5"
        End Get
    End Property

    Public Sub NormalizeNot(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) Implements IBehaviour.NormalizeNot
        Particle.SpeedIsSet = False
    End Sub

    Public Sub Normalize(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) Implements IBehaviour.Normalize
    End Sub

    Public Function Behave(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) As Boolean Implements IBehaviour.Behave
        With Particle
            If Not .SpeedIsSet Then
                .SpeedIsSet = True
            End If
            .TargetSpeed = RndSpeed()
            Dim Rocks As New List(Of PointF)
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
                .TargetAngel = RndDirectedAngel(XYToDegrees(.CurrentPosition, IsNear), 45)
            Else
                .TargetAngel = RndDirectedAngel(315, 90)
            End If
        End With
        Return True
    End Function

End Class
