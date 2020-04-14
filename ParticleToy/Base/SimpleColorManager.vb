Imports Microsoft.VisualBasic.Devices
Imports ParticleToy
Imports ParticleToy.Behaviour

Public Class SimpleColorManager
    Implements IColorManager

    Public Property RandomizeValue As Integer = 0
    Public Property Color As Color = Color.DeepSkyBlue

    Public ReadOnly Property Key As String Implements IColorManager.Key
        Get
            Return "Simple Color"
        End Get
    End Property

    Public ReadOnly Property Icon As Bitmap Implements IColorManager.Icon
        Get
            Return My.Resources.color
        End Get
    End Property

    Public ReadOnly Property Name As String Implements IColorManager.Name
        Get
            Return "Simple Color"
        End Get
    End Property

    Public ReadOnly Property DefaultMode As IColorManager.Modes Implements IColorManager.DefaultMode
        Get
            Return IColorManager.Modes.Multiply
        End Get
    End Property

    Public Property Mode As IColorManager.Modes = IColorManager.Modes.Multiply Implements IColorManager.Mode

    Public Sub New(Color As Color, RandomizeValue As Integer)
        Me.RandomizeValue = RandomizeValue
        Me.Color = Color
    End Sub

    Public Function TurnOn(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) As Color? Implements IColorManager.TurnOn
        Return Color.Randomize(RandomizeValue)
    End Function

    Public Function Manage(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) As Color? Implements IColorManager.Manage
    End Function

End Class
