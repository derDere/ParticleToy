Imports Microsoft.VisualBasic.Devices
Imports ParticleToy
Imports ParticleToy.Behaviour

Public Class cNoneBh
    Implements IBehaviour

    Public ReadOnly Property Key As String Implements IBehaviour.Key
        Get
            Return "unknown"
        End Get
    End Property

    Public ReadOnly Property Icon As Bitmap Implements IBehaviour.Icon
        Get
            Return My.Resources.unknown
        End Get
    End Property

    Public ReadOnly Property Name As String Implements IBehaviour.Name
        Get
            Return "Unknown"
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
        Particle.FontColor = New SolidBrush(Particle.Color.Color)
    End Sub

    'Public Sub Normalize(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) Implements IBehaviour.Normalize
    'End Sub

    Public Sub TurnedOn(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) Implements IBehaviour.TurnedOn
    End Sub

    Public Function Behave(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) As Boolean Implements IBehaviour.Behave
        With Particle
            .FontColor = New SolidBrush(Particle.CurrentColor.Color)
            If .TargetAngel = 0 Then .TargetAngel = RndDegrees()
            .TargetSpeed = MIN_SPEED
            .CurrentColor = RandomColor()
            If TypeOf Game Is Game Then
                Dim Game1 As Game = Game
                If chance(50000) Then
                    Game1.BlinkL.Add(New Blink(Particle.CurrentPosition, Game, .CurrentColor.Color))
                End If
            End If
            If chance(5000) Then
                Particle.BlinkCharTimer = 10
                Particle.BlinkChar = RndChar()
            End If
        End With
        Return True
    End Function

End Class
