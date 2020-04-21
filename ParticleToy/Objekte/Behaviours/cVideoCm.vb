Imports Microsoft.VisualBasic.Devices
Imports ParticleToy
Imports ParticleToy.Behaviour

Public Class cVideoCm
    Implements IColorManager

    Public Property Images As Bitmap()

    Public ReadOnly Property Key As String Implements IColorManager.Key
        Get
            Return "vid"
        End Get
    End Property

    Public ReadOnly Property Icon As Bitmap Implements IColorManager.Icon
        Get
            Return My.Resources.vid
        End Get
    End Property

    Public ReadOnly Property Name As String Implements IColorManager.Name
        Get
            Return "Video"
        End Get
    End Property

    Public ReadOnly Property DefaultMode As IColorManager.Modes Implements IColorManager.DefaultMode
        Get
            Return IColorManager.Modes.Replace
        End Get
    End Property

    Public Property Mode As IColorManager.Modes = IColorManager.Modes.Replace Implements IColorManager.Mode

    Public Sub New(empty As Boolean)
        Dim imgL As New List(Of Bitmap)
        If Not empty Then
            Using archive As New IO.Compression.ZipArchive(New IO.MemoryStream(My.Resources.Clownfishes_in_Anemone_800x600_muted_frames_jpg))
                For Each entry As IO.Compression.ZipArchiveEntry In archive.Entries
                    Using entryStream As IO.Stream = entry.Open
                        Dim b As New Bitmap(entryStream)
                        imgL.AddRange({b}) ', b, b, b, b, b})
                    End Using
                Next
            End Using
        End If
        Images = imgL.ToArray
    End Sub

    Public Sub New()
        Me.New(False)
    End Sub

    Public Function TurnOn(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) As Color? Implements IColorManager.TurnOn
    End Function

    Public Function Manage(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) As Color? Implements IColorManager.Manage
        With Particle
            Return Images(Tick Mod Images.Length).GetPixel(Math.Floor(.CurrentPosition.X), Math.Floor(.CurrentPosition.Y)).Randomize(50)
        End With
    End Function

End Class
