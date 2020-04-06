Imports Microsoft.VisualBasic.Devices
Imports ParticleToy
Imports ParticleToy.Behaviour

Public Class cMirroredBh
    Implements IBehaviour

    Public ReadOnly Property Key As String Implements IBehaviour.Key
        Get
            Return "9"
        End Get
    End Property

    Public Sub NormalizeNot(Particle As Particle, Game As GameBase, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) Implements IBehaviour.NormalizeNot
        Particle.IsElectric = False
        Particle.CurrentColor = Particle.Color
        Particle.MirrorPoint = Nothing
    End Sub

    Public Sub Normalize(Particle As Particle, Game As GameBase, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) Implements IBehaviour.Normalize
    End Sub

    Public Function Behave(Particle As Particle, Game As GameBase, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) As Boolean Implements IBehaviour.Behave
        With Particle
            If (.MyIndex Mod 2) = 0 Then
                Dim p As Double = .CurrentPosition.Y / Game.ScreenSize.Height
                .TargetSpeed = MIN_SPEED + ((MAX_SPEED - MIN_SPEED) * p)
                .CurrentSpeed = .TargetSpeed
                If MouseInfo.Position IsNot Nothing AndAlso DeltaBetweed(.CurrentPosition, MouseInfo.Position) < ASIDE_RADIUS Then
                    .TargetAngel = XYToDegrees(.CurrentPosition, MouseInfo.Position)
                Else
                    If .MirrorPoint Is Nothing Then
                        .MirrorPoint = .Ancs.Anchors(RND.Next(1000) Mod .Ancs.Anchors.Count)
                    ElseIf DeltaBetweed(.CurrentPosition, .MirrorPoint.Value) < BOUNCE_RADIUS Then
                        .MirrorPoint = .Ancs.Anchors(RND.Next(1000) Mod .Ancs.Anchors.Count)
                    End If
                    .TargetAngel = RndDirectedAngel(XYToDegrees(.MirrorPoint, .CurrentPosition), 180)
                End If
                Dim G As Integer = ((((.CurrentPosition.X + .CurrentPosition.Y) / 2) + (2 * Tick)) Mod 256)
                If G < 128 Then
                    G += 128
                Else
                    G = 255 - (G - 128)
                End If
                .CurrentColor = New Pen(Drawing.Color.FromArgb(255, 255, 0, G))
            Else
                Dim Partner As Particle = .Parent.ParticleL(.MyIndex - 1)
                .LastPosition = New Point(Game.ScreenSize.Width - Partner.LastPosition.X, Partner.LastPosition.Y)
                .CurrentPosition = New Point(Game.ScreenSize.Width - Partner.CurrentPosition.X, Partner.CurrentPosition.Y)
                .CurrentColor = Partner.CurrentColor
                Return False
            End If
        End With
        Return True
    End Function

End Class
