Imports Microsoft.VisualBasic.Devices
Imports ParticleToy
Imports ParticleToy.Behaviour

Public Class cAntsBh
    Implements IBehaviour

    Public ReadOnly Property Key As String Implements IBehaviour.Key
        Get
            Return "ants"
        End Get
    End Property

    Public Sub NormalizeNot(Particle As Particle, Game As GameBase, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) Implements IBehaviour.NormalizeNot
        Particle.Partner = Nothing
        Particle.FoundAnt = False
        Particle.SpeedIsSet = False
        Particle.CurrentColor = Particle.Color
    End Sub

    Public Sub Normalize(Particle As Particle, Game As GameBase, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) Implements IBehaviour.Normalize
        If Not Particle.SpeedIsSet Then
            Particle.SpeedIsSet = True
            Particle.TargetSpeed = ANTS_SPEED
            Particle.CurrentColor = New Pen(Color.Chocolate.Randomize(100))
        End If
    End Sub

    Public Function Behave(Particle As Particle, Game As GameBase, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) As Boolean Implements IBehaviour.Behave
        With Particle
            If Particle.MyIndex = 0 Then
                If MouseInfo.LeftIsPressed Then
                    .TargetAngel = RndDirectedAngel(XYToDegrees(MouseInfo.Position, .CurrentPosition), 90)
                Else
                    If (RND.Next(1000, 9999) Mod 5) = 0 Then
                        Dim m As Integer = 1
                        If (RND.Next(1000, 9999) Mod 2) = 0 Then
                            m = -1
                        End If
                        .TargetAngel += 10 * m
                    End If
                End If
            Else
                .Partner = .Parent.ParticleL(.MyIndex - 1)
                Dim delta As Integer = DeltaBetweed(.CurrentPosition, .Partner.CurrentPosition)
                If delta < (.Parent.ScreenSize.Height - 50) Or (Not .FoundAnt) Then
                    If delta > 5 Then
                        .FoundAnt = True
                        .TargetAngel = XYToDegrees(.Partner.CurrentPosition, .CurrentPosition)
                    Else
                        .TargetAngel = RndDegrees()
                    End If
                End If
            End If
        End With
        Return True
    End Function

End Class
