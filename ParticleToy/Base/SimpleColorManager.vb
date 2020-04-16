Imports Microsoft.VisualBasic.Devices
Imports ParticleToy
Imports ParticleToy.Behaviour

Public Class SimpleColorManager
    Implements IColorManager

    Property Refresh As Boolean = False
    Property RefreshMode As Boolean = False
    Public Property RandomizeValue As Integer = 0
    Public Property Color As Color = Color.DeepSkyBlue

    Private ColorObjGuid As Guid = Guid.NewGuid

    Public ReadOnly Property Key As String Implements IColorManager.Key
        Get
            Return "Simple Color - " & ColorObjGuid.ToString
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

    Public Sub New()
        Me.RandomizeValue = 0
        Me.Color = Color.CornflowerBlue
    End Sub

    Public Sub New(Color As Color, RandomizeValue As Integer)
        Me.RandomizeValue = RandomizeValue
        Me.Color = Color
    End Sub

    Private MyColors(Game.PARTICLE_NUMBER - 1) As Color

    Public Function TurnOn(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) As Color? Implements IColorManager.TurnOn
        If RandomizeValue <= 0 Then
            MyColors(Particle.MyIndex) = Color
        Else
            MyColors(Particle.MyIndex) = Color.Randomize(RandomizeValue)
        End If
        Return MyColors(Particle.MyIndex)
    End Function

    Public Function Manage(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) As Color? Implements IColorManager.Manage
        If Refresh Then
            If RandomizeValue <= 0 Then
                MyColors(Particle.MyIndex) = Color
            Else
                MyColors(Particle.MyIndex) = Color.Randomize(RandomizeValue)
            End If
            If Particle.MyIndex = (Game.ParticleL.Count - 1) Then
                Refresh = False
            End If
        End If
        Return MyColors(Particle.MyIndex)
    End Function

End Class
