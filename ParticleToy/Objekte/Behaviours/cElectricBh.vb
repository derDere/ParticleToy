Imports Microsoft.VisualBasic.Devices
Imports ParticleToy
Imports ParticleToy.Behaviour

Public Class cElectricBh
    Implements IBehaviour
    Implements IColorManager

    Public ReadOnly Property Key As String Implements IBehaviour.Key, IColorManager.Key
        Get
            Return "6"
        End Get
    End Property

    Public ReadOnly Property Icon As Bitmap Implements IBehaviour.Icon, IColorManager.Icon
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property Name As String Implements IBehaviour.Name, IColorManager.Name
        Get
            Return "Electric"
        End Get
    End Property

    Public ReadOnly Property ColorManager As IColorManager Implements IBehaviour.ColorManager
        Get
            Return Me
        End Get
    End Property

    Public ReadOnly Property OverwriteColorManager As Boolean Implements IBehaviour.OverwriteColorManager
        Get
            Return True
        End Get
    End Property

    Public ReadOnly Property DefaultMode As IColorManager.Modes Implements IColorManager.DefaultMode
        Get
            Return IColorManager.Modes.Replace
        End Get
    End Property

    Public Property Mode As IColorManager.Modes = IColorManager.Modes.Replace Implements IColorManager.Mode

    Public Sub TurnedOff(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) Implements IBehaviour.TurnedOff
        If Game.ColorManager Is Me Then _
            Game.ColorManager = Nothing
    End Sub

    'Public Sub Normalize(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) Implements IBehaviour.Normalize
    'End Sub

    Public Sub TurnedOn(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) Implements IBehaviour.TurnedOn
        Particle.TargetSpeed = RndSpeed()
    End Sub

    Public Function Behave(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) As Boolean Implements IBehaviour.Behave
        With Particle
            'If MouseInfo.Position IsNot Nothing Then
            'Dim MouseDelta As Double = DeltaBetweed(CurrentPosition, MouseInfo.Position)
            'End If
            .TargetAngel = Particle.ElectricAngles(RND.Next(1000) Mod Particle.ElectricAngles.Length)
            .CurrentAngel = .TargetAngel
        End With
        Return True
    End Function

    Public Function TurnOn(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) As Color? Implements IColorManager.TurnOn
    End Function

    Public Function Manage(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) As Color? Implements IColorManager.Manage
        Dim G As Integer = ((((Particle.CurrentPosition.X + Particle.CurrentPosition.Y) / 2) + (2 * Tick)) Mod 256)
        If G < 128 Then
            G += 128
        Else
            G = 255 - (G - 128)
        End If
        Return Drawing.Color.FromArgb(255, 0, G, 192)
    End Function

End Class
