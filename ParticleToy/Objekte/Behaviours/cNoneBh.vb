Imports Microsoft.VisualBasic.Devices
Imports ParticleToy
Imports ParticleToy.Behaviour

Public Class cNoneBh
    Implements IBehaviour

    Public ReadOnly Property Key As String Implements IBehaviour.Key
        Get
            Return "-1"
        End Get
    End Property

    Public Sub NormalizeNot(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) Implements IBehaviour.NormalizeNot
        Particle.CurrentColor = Particle.Color
        Particle.FontColor = New SolidBrush(Particle.Color.Color)
    End Sub

    Public Sub Normalize(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) Implements IBehaviour.Normalize
        Particle.FontColor = New SolidBrush(Particle.CurrentColor.Color)
    End Sub

    Public Function Behave(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) As Boolean Implements IBehaviour.Behave
        With Particle
            If .TargetAngel = 0 Then .TargetAngel = RndDegrees()
            .TargetSpeed = MIN_SPEED
            .CurrentColor = RandomColor()
            If TypeOf Game Is Game Then
                Dim Game1 As Game = Game
                If (RND.Next(1000000, 9000000) Mod 50000) = 0 Then
                    Game1.BlinkL.Add(New Blink(Particle.CurrentPosition, Game, .CurrentColor.Color))
                End If
            End If
            If (RND.Next(1000000, 9000000) Mod 5000) = 0 Then
                Particle.BlinkCharTimer = 10
                Particle.BlinkChar = RndChar()
            End If
        End With
        Return True
    End Function

End Class
