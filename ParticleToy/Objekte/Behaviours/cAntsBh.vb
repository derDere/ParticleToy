Imports Microsoft.VisualBasic.Devices
Imports ParticleToy
Imports ParticleToy.Behaviour

Public Class cAntsBh
    Implements IBehaviour

    Private ColMan As New SimpleColorManager(Color.Chocolate, 100) With {.Mode = IColorManager.Modes.Replace}

    Public ReadOnly Property Key As String Implements IBehaviour.Key
        Get
            Return "ants"
        End Get
    End Property

    Public ReadOnly Property Icon As Bitmap Implements IBehaviour.Icon
        Get
            Return My.Resources.ants
        End Get
    End Property

    Public ReadOnly Property Name As String Implements IBehaviour.Name
        Get
            Return "Ant Colony"
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
        Particle.Partner = Nothing
        Particle.FoundAnt = False
        Particle.SpeedIsSet = False
    End Sub

    'Public Sub Normalize(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) Implements IBehaviour.Normalize
    '    If Not Particle.SpeedIsSet Then
    '        Particle.SpeedIsSet = True
    '        Particle.Curre ntColor = New Pen(Color.Chocolate.Randomize(100))
    '    End If
    'End Sub

    Public Sub TurnedOn(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) Implements IBehaviour.TurnedOn
    End Sub

    Public Function Behave(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) As Boolean Implements IBehaviour.Behave
        With Particle
            If .MyIndex = 0 Then
                .TargetSpeed = 2
                If (RND.Next(1000, 9999) Mod 5) = 0 Then
                    Dim m As Integer = 1
                    If (RND.Next(1000, 9999) Mod 2) = 0 Then
                        m = -1
                    End If
                    .TargetAngel += 10 * m
                End If
            Else
                .Partner = .Parent.ParticleL(.MyIndex - 1)
                Dim delta As Double = DeltaBetweedFastest(.Partner.CurrentPosition, .CurrentPosition, Game.ScreenSize)
                If delta > 5 Then
                    If delta > 300 Then
                        .TargetSpeed = MAX_SPEED
                    ElseIf delta > 100 Then
                        .TargetSpeed = 7
                    Else
                        .TargetSpeed = 4
                    End If
                    .FoundAnt = True
                    .TargetAngel = RndDirectedAngel(XYToDegreesFastest(.Partner.CurrentPosition, .CurrentPosition, Game.ScreenSize), 45)
                Else
                    .TargetSpeed = 0
                    .CurrentSpeed = 0
                End If
            End If
            If (Tick Mod Game.ParticleL.Count) = .MyIndex Then
                '.FontColor = Brushes.Lime
                '.BlinkChar = "+"
                '.BlinkCharTimer = 1
                Game.BlinkL.Add(New Blink(.CurrentPosition, Game, Drawing.Color.Lime, 10, 0, 1))
            End If
        End With
        Return True
    End Function

End Class
