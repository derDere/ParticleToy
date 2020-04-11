Imports Microsoft.VisualBasic.Devices
Imports ParticleToy
Imports ParticleToy.Behaviour

Public Class cTeslaBh
    Implements IBehaviour

    Public ReadOnly Property Key As String Implements IBehaviour.Key
        Get
            Return "1"
        End Get
    End Property

    Public ReadOnly Property Icon As Bitmap Implements IBehaviour.Icon
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property Name As String Implements IBehaviour.Name
        Get
            Return "Tesla"
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
        Particle.TargetSpeed = APROACH_SPEED
    End Sub

    Public Function Behave(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) As Boolean Implements IBehaviour.Behave
        With Particle
            Dim DeltaToAnc As Double = DeltaBetweed(.CurrentPosition, .Ancs.Anchors(0))
            If DeltaToAnc < (BOUNCE_RADIUS + 20) Then
                If DeltaToAnc < BOUNCE_RADIUS Then
                    .CurrentSpeed = MAX_SPEED
                    .TargetAngel = XYToDegrees(.CurrentPosition, .Ancs.Anchors(0))
                    .CurrentAngel = .TargetAngel
                    .Lightnight = .Ancs.Anchors(0)
                Else
                    .TargetAngel = RndDegrees()
                End If
            Else
                .TargetAngel = RndDirectedAngel(XYToDegrees(.Ancs.Anchors(0), .CurrentPosition), 45)
            End If
        End With
        Return True
    End Function

End Class
