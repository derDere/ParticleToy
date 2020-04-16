Imports Microsoft.VisualBasic.Devices
Imports ParticleToy

Public Class NoColorManager
    Implements IColorManager

    Public ReadOnly Property Key As String Implements IColorManager.Key
        Get
            Return "0"
        End Get
    End Property

    Public ReadOnly Property Icon As Bitmap Implements IColorManager.Icon
        Get
            Return My.Resources.anc_0
        End Get
    End Property

    Public ReadOnly Property Name As String Implements IColorManager.Name
        Get
            Return "No Color Manager"
        End Get
    End Property

    Public ReadOnly Property DefaultMode As IColorManager.Modes Implements IColorManager.DefaultMode
        Get
            Return IColorManager.Modes.Replace
        End Get
    End Property

    Public Property Mode As IColorManager.Modes = IColorManager.Modes.Replace Implements IColorManager.Mode

    Public Function TurnOn(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) As Color? Implements IColorManager.TurnOn
        Return Particle.Color.Color
    End Function

    Public Function Manage(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) As Color? Implements IColorManager.Manage
    End Function

End Class
