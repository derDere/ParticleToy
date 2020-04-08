Imports Microsoft.VisualBasic.Devices
Imports ParticleToy
Imports ParticleToy.Behaviour

Public Class cVideoBh
    Implements IBehaviour

    Friend Images As Bitmap()

    Public ReadOnly Property Key As String Implements IBehaviour.Key
        Get
            Return "vid"
        End Get
    End Property

    Public Sub New()
        Dim imgL As New List(Of Bitmap)
        Using archive As New IO.Compression.ZipArchive(New IO.MemoryStream(My.Resources.Clownfishes_in_Anemone_800x600_muted_frames_jpg))
            For Each entry As IO.Compression.ZipArchiveEntry In archive.Entries
                Using entryStream As IO.Stream = entry.Open
                    Dim b As New Bitmap(entryStream)
                    imgL.AddRange({b}) ', b, b, b, b, b})
                End Using
            Next
        End Using
        Images = imgL.ToArray
    End Sub

    Public Sub NormalizeNot(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) Implements IBehaviour.NormalizeNot
        Particle.CurrentColor = Particle.Color
    End Sub

    Public Sub Normalize(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) Implements IBehaviour.Normalize
    End Sub

    Public Function Behave(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) As Boolean Implements IBehaviour.Behave
        With Particle
            .CurrentColor = New Pen(Images(Tick Mod Images.Length).GetPixel(Math.Floor(.CurrentPosition.X), Math.Floor(.CurrentPosition.Y)).Randomize(50))
            .TargetSpeed = MIN_SPEED
            If MouseInfo.Position IsNot Nothing AndAlso DeltaBetweed(.CurrentPosition, MouseInfo.Position) < ASIDE_RADIUS Then
                .TargetAngel = XYToDegrees(.CurrentPosition, MouseInfo.Position)
            Else
                .TargetAngel = RndDegrees()
            End If
        End With
        Return True
    End Function

End Class
