Imports Microsoft.VisualBasic.Devices
Imports ParticleToy
Imports ParticleToy.Behaviour

Public Class cCicadaBh
    Implements IBehaviour

    Private CicadaImg As Bitmap = My.Resources.b1
    Private P1 As New Point(150, 250)
    Private P2 As New Point(400, 250)
    Private P3 As New Point(650, 250)

    Public ReadOnly Property Key As String Implements IBehaviour.Key
        Get
            Return "cicada"
        End Get
    End Property

    Public Sub NormalizeNot(Particle As Particle, Game As GameBase, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) Implements IBehaviour.NormalizeNot
        Particle.FoundAnt = False
        Particle.CurrentColor = Particle.Color
    End Sub

    Public Sub Normalize(Particle As Particle, Game As GameBase, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) Implements IBehaviour.Normalize
    End Sub

    Public Function Behave(Particle As Particle, Game As GameBase, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) As Boolean Implements IBehaviour.Behave
        With Particle
            If Not .FoundAnt Then
                .CurrentColor = New Pen(Color.Silver.Randomize(100))
            End If
            Dim targetPoint As Point = P1
            If (.MyIndex Mod 3) = 1 Then targetPoint = P2
            If (.MyIndex Mod 3) = 2 Then targetPoint = P3
            If CicadaImg.GetPixel(.CurrentPosition.X, .CurrentPosition.Y).R < 128 Then
                '.TargetAngel = RndDegrees()
                .TargetAngel = RndDirectedAngel(XYToDegrees(.CurrentPosition, targetPoint), 220)
                .FoundAnt = True
            Else
                .TargetAngel = RndDirectedAngel(XYToDegrees(targetPoint, .CurrentPosition), 220)
            End If
            If .FoundAnt Then
                .TargetSpeed = MIN_SPEED
            Else
                .TargetSpeed = APROACH_SPEED
            End If
        End With
        Return True
    End Function

End Class
