Imports Microsoft.VisualBasic.Devices
Imports ParticleToy
Imports ParticleToy.Behaviour

Public Class cElectricBh
    Implements IBehaviour

    Public ReadOnly Property Key As String Implements IBehaviour.Key
        Get
            Return "6"
        End Get
    End Property

    Public Sub NormalizeNot(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) Implements IBehaviour.NormalizeNot
        Particle.IsElectric = False
        Particle.CurrentColor = Particle.Color
    End Sub

    Public Sub Normalize(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) Implements IBehaviour.Normalize
    End Sub

    Public Function Behave(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) As Boolean Implements IBehaviour.Behave
        With Particle
            If Not .IsElectric Then
                .IsElectric = True
                .TargetSpeed = RndSpeed()
            End If
            'If MouseInfo.Position IsNot Nothing Then
            'Dim MouseDelta As Double = DeltaBetweed(CurrentPosition, MouseInfo.Position)
            'End If
            .TargetAngel = Particle.ElectricAngles(RND.Next(1000) Mod Particle.ElectricAngles.Length)
            .CurrentAngel = .TargetAngel
            Dim G As Integer = ((((.CurrentPosition.X + .CurrentPosition.Y) / 2) + (2 * Tick)) Mod 256)
            If G < 128 Then
                G += 128
            Else
                G = 255 - (G - 128)
            End If
            .CurrentColor = New Pen(Drawing.Color.FromArgb(255, 0, G, 192))
        End With
        Return True
    End Function

End Class
