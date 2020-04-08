Imports Microsoft.VisualBasic.Devices
Imports ParticleToy
Imports ParticleToy.Behaviour

Public Class cStarsBh
    Implements IBehaviour

    Public ReadOnly Property Key As String Implements IBehaviour.Key
        Get
            Return "10"
        End Get
    End Property

    Public Sub NormalizeNot(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) Implements IBehaviour.NormalizeNot
        Particle.IsElectric = False
        Particle.CurrentColor = Particle.Color
        Particle.IsAStar = False
    End Sub

    Public Sub Normalize(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) Implements IBehaviour.Normalize
    End Sub

    Public Function Behave(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) As Boolean Implements IBehaviour.Behave
        With Particle
            If Not .IsAStar Then
                .IsAStar = True
                .TargetSpeed = MIN_SPEED + (RND.Next(1000) Mod 2)
                .TargetAngel = 0
            End If

            Dim MouseDelta As Double = DeltaBetweed(.CurrentPosition, MouseInfo.Position)
            Dim ColorPart As Integer = -1
            For Each Anc In .Ancs.Anchors
                Dim AncDelta As Double = DeltaBetweed(.CurrentPosition, Anc)
                If AncDelta < MouseDelta Then
                    MouseDelta = AncDelta
                    ColorPart = .Ancs.Anchors.IndexOf(Anc) Mod 3
                End If
            Next
            Dim RedLevel As Integer = 0
            If MouseDelta < BOUNCE_RADIUS Then
                RedLevel = (BOUNCE_RADIUS - MouseDelta) * 3
            End If
            Dim ColorVal As Integer = 255 - ((2 - .TargetSpeed) * 100)
            Select Case ColorPart
                Case 2
                    .CurrentColor = New Pen(Drawing.Color.FromArgb(255, ColorVal - RedLevel, ColorVal - RedLevel, ColorVal))
                Case 1
                    .CurrentColor = New Pen(Drawing.Color.FromArgb(255, ColorVal - RedLevel, ColorVal, ColorVal - RedLevel))
                Case 0
                    .CurrentColor = New Pen(Drawing.Color.FromArgb(255, ColorVal, ColorVal - RedLevel, ColorVal - RedLevel))
                Case Else
                    .CurrentColor = New Pen(Drawing.Color.FromArgb(255, ColorVal - (RedLevel * 0.3333), ColorVal - RedLevel, ColorVal))
            End Select
        End With
        Return True
    End Function

End Class
