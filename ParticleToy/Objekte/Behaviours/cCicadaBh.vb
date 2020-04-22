Imports Microsoft.VisualBasic.Devices
Imports ParticleToy
Imports ParticleToy.Behaviour

Public Class cCicadaBh
    Implements IBehaviour

    Private Const ALPHA As Integer = 32
    Private CicadaImg As Bitmap = My.Resources.b1
    Private P1 As New PointF(150, 250)
    Private P2 As New PointF(400, 250)
    Private P3 As New PointF(650, 250)
    Private ColMan As New SimpleColorManager(Color.White, 100) With {.Mode = IColorManager.Modes.Multiply}

    Public ReadOnly Property Key As String Implements IBehaviour.Key
        Get
            Return "cicada"
        End Get
    End Property

    Public ReadOnly Property Icon As Bitmap Implements IBehaviour.Icon
        Get
            Return My.Resources.cicada
        End Get
    End Property

    Public ReadOnly Property Name As String Implements IBehaviour.Name
        Get
            Return "Cicada 3301"
        End Get
    End Property

    Public ReadOnly Property ColorManager As IColorManager Implements IBehaviour.ColorManager
        Get
            Return ColMan
        End Get
    End Property

    Public ReadOnly Property OverwriteColorManager As Boolean Implements IBehaviour.OverwriteColorManager
        Get
            Return False
        End Get
    End Property

    Public Sub TurnedOff(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) Implements IBehaviour.TurnedOff
        Particle.FoundAnt = False
    End Sub

    'Public Sub Normalize(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) Implements IBehaviour.Normalize
    'End Sub

    Public Sub TurnedOn(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) Implements IBehaviour.TurnedOn
        Particle.CurrentColor = Pens.Silver.SetAlpha(ALPHA)
    End Sub

    Public Function Behave(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) As Boolean Implements IBehaviour.Behave
        With Particle
            If .FoundAnt Then
                .CurrentColor = Pens.White.SetAlpha(ALPHA)
            End If
            Dim targetPoint As PointF = P1
            If (.MyIndex Mod 3) = 1 Then targetPoint = P2
            If (.MyIndex Mod 3) = 2 Then targetPoint = P3
            If CicadaImg.GetPixel(Math.Floor(.CurrentPosition.X), Math.Floor(.CurrentPosition.Y)).R < 128 Then
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
