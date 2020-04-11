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

    Public ReadOnly Property Icon As Bitmap Implements IBehaviour.Icon
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property Name As String Implements IBehaviour.Name
        Get
            Return "Mirrored"
        End Get
    End Property

    Public ReadOnly Property ColorManager As IColorManager Implements IBehaviour.ColorManager
        Get
            Return Nothing
        End Get
    End Property

    Public ReadOnly Property OverwriteColorManager As Boolean Implements IBehaviour.OverwriteColorManager
        Get
            Return False
        End Get
    End Property

    Public Sub TurnedOff(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) Implements IBehaviour.TurnedOff
        Particle.MirrorPoint = Nothing
        Particle.Partner = Nothing
    End Sub

    'Public Sub Normalize(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) Implements IBehaviour.Normalize
    'End Sub

    Public Sub TurnedOn(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) Implements IBehaviour.TurnedOn
        If (Particle.MyIndex Mod 2) <> 0 Then
            Particle.Partner = Game.ParticleL(Particle.MyIndex - 1)
        End If
    End Sub

    Public Function Behave(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) As Boolean Implements IBehaviour.Behave
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
                Dim G As Integer = (((.CurrentPosition.Y) + (2 * Tick)) Mod 256)
                If G < 128 Then
                    G += 128
                Else
                    G = 255 - (G - 128)
                End If
                .CurrentColor = New Pen(Drawing.Color.FromArgb(255, 255, 0, G))
            Else
                .LastPosition = New PointF(Game.ScreenSize.Width - .Partner.LastPosition.X, .Partner.LastPosition.Y)
                .CurrentPosition = New PointF(Game.ScreenSize.Width - .Partner.CurrentPosition.X, .Partner.CurrentPosition.Y)
                .CurrentColor = .Partner.CurrentColor
                .DrawLineDelta = DeltaBetweed(.LastPosition, .CurrentPosition)
                Return False
            End If
        End With
        Return True
    End Function

End Class
