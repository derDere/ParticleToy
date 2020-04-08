Imports Microsoft.VisualBasic.Devices
Imports ParticleToy
Imports ParticleToy.Behaviour

Public Class cMatrixBh
    Implements IBehaviour

    Public ReadOnly Property Key As String Implements IBehaviour.Key
        Get
            Return "matrix"
        End Get
    End Property

    Public Sub NormalizeNot(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) Implements IBehaviour.NormalizeNot
        Particle.CurrentColor = Particle.Color
        Particle.SpeedIsSet = False
    End Sub

    Public Sub Normalize(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) Implements IBehaviour.Normalize
    End Sub

    Public Function Behave(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) As Boolean Implements IBehaviour.Behave

        If Not Particle.SpeedIsSet Then
            Particle.SpeedIsSet = True
            Particle.TargetSpeed = RndSpeed()
            Dim m As Double = (Particle.TargetSpeed) / (MAX_SPEED)
            Particle.CurrentColor = New Pen(Color.FromArgb(255, 0, Math.Round(255 * m), 0))
        End If
        Particle.TargetAngel = 270

        If (Particle.MyIndex Mod 25) = 0 Then
            If (Particle.CurrentPosition.Y Mod 10) = 0 Then
                Particle.BlinkCharTimer = 5
                Particle.FontColor = Brushes.White
                Particle.BlinkChar = RndChar()
            End If
        End If

        Return True
    End Function

End Class
