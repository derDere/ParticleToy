Imports Microsoft.VisualBasic.Devices
Imports ParticleToy
Imports ParticleToy.Behaviour

Public Class cMatrixBh
    Implements IBehaviour

    Private ColMan As New SimpleColorManager(Color.FromArgb(255, 0, 205, 0), 100) With {.Mode = IColorManager.Modes.Replace}

    Public ReadOnly Property Key As String Implements IBehaviour.Key
        Get
            Return "matrix"
        End Get
    End Property

    Public ReadOnly Property Icon As Bitmap Implements IBehaviour.Icon
        Get
            Return My.Resources.matrix
        End Get
    End Property

    Public ReadOnly Property Name As String Implements IBehaviour.Name
        Get
            Return "The Matrix"
        End Get
    End Property

    Public ReadOnly Property ColorManager As IColorManager Implements IBehaviour.ColorManager
        Get
            Return ColMan
        End Get
    End Property

    Public ReadOnly Property OverwriteColorManager As Boolean Implements IBehaviour.OverwriteColorManager
        Get
            Return True
        End Get
    End Property

    Public Sub TurnedOff(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) Implements IBehaviour.TurnedOff
    End Sub

    'Public Sub Normalize(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) Implements IBehaviour.Normalize
    'End Sub

    Public Sub TurnedOn(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) Implements IBehaviour.TurnedOn
        Particle.TargetSpeed = RndSpeed()
    End Sub

    Public Function Behave(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) As Boolean Implements IBehaviour.Behave

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
