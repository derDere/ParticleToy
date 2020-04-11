Imports Microsoft.VisualBasic.Devices
Imports ParticleToy
Imports ParticleToy.Behaviour

Public Class cRainbowCm
    Implements IColorManager

    Private Const SCALE As Single = 0.0075

    Public ReadOnly Property Key As String Implements IColorManager.Key
        Get
            Return "rainbow"
        End Get
    End Property

    Public ReadOnly Property Icon As Bitmap Implements IColorManager.Icon
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property Name As String Implements IColorManager.Name
        Get
            Return "Rainbow"
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
        With Particle
            Dim colorNoise As Single = SimplexNoise.Noise.CalcPixel3D(.CurrentPosition.X, .CurrentPosition.Y, Tick * 2, SCALE) * 2
            Return HsvToRgb(colorNoise, 1, 1)
        End With
    End Function

End Class
