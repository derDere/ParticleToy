Imports Microsoft.VisualBasic.Devices
Imports ParticleToy
Imports ParticleToy.Behaviour

Public Class cImageCm
    Implements IColorManager

    Public Image As Bitmap = My.Resources.b2

    Public ReadOnly Property Key As String Implements IColorManager.Key
        Get
            Return "img"
        End Get
    End Property

    Public ReadOnly Property Icon As Bitmap Implements IColorManager.Icon
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property Name As String Implements IColorManager.Name
        Get
            Return "Image"
        End Get
    End Property

    Public ReadOnly Property DefaultMode As IColorManager.Modes Implements IColorManager.DefaultMode
        Get
            Return IColorManager.Modes.Replace
        End Get
    End Property

    Public Property Mode As IColorManager.Modes = IColorManager.Modes.Replace Implements IColorManager.Mode

    Public Function TurnOn(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) As Color? Implements IColorManager.TurnOn
    End Function

    Public Function Manage(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) As Color? Implements IColorManager.Manage
        If Particle.CurrentPosition.X >= 0 AndAlso Particle.CurrentPosition.X < Image.Width AndAlso Particle.CurrentPosition.Y >= 0 AndAlso Particle.CurrentPosition.Y < Image.Height Then
            Return Image.GetPixel(Math.Floor(Particle.CurrentPosition.X), Math.Floor(Particle.CurrentPosition.Y)).Randomize(50)
        Else
            Return Color.Black
        End If
    End Function

End Class
