Imports Microsoft.VisualBasic.Devices
Imports ParticleToy
Imports ParticleToy.Behaviour

Public Class cRingCm
    Implements IColorManager

    Private Const FRAME_LENGTH As Integer = 10

    Public Property Images As Bitmap()

    Public ReadOnly Property Key As String Implements IColorManager.Key
        Get
            Return "ring"
        End Get
    End Property

    Public ReadOnly Property Icon As Bitmap Implements IColorManager.Icon
        Get
            Return My.Resources.ring
        End Get
    End Property

    Public ReadOnly Property Name As String Implements IColorManager.Name
        Get
            Return "The Ring"
        End Get
    End Property

    Public ReadOnly Property DefaultMode As IColorManager.Modes Implements IColorManager.DefaultMode
        Get
            Return IColorManager.Modes.Replace
        End Get
    End Property

    Public Property Mode As IColorManager.Modes = IColorManager.Modes.Replace Implements IColorManager.Mode

    Public Sub New()
        Dim FrameDict As New Dictionary(Of String, Bitmap)

        Using archive As New IO.Compression.ZipArchive(New IO.MemoryStream(My.Resources.ring_zip))
            For Each entry As IO.Compression.ZipArchiveEntry In archive.Entries
                Using entryStream As IO.Stream = entry.Open
                    Dim b As New Bitmap(entryStream)
                    FrameDict.Add(entry.Name.Replace(".PNG", ""), b)
                End Using
            Next
        End Using

        Dim order As String() = {
            "white",
            "chair",
            "white",
            "deer",
            "white",
            "face",
            "white",
            "fingers",
            "white",
            "fire",
            "white",
            "her",
            "white",
            "house",
            "white",
            "mirror",
            "white",
            "moon",
            "white",
            "ring",
            "white",
            "ring",
            "white",
            "mouth",
            "white",
            "nail",
            "white",
            "noise",
            "white",
            "table",
            "white",
            "tree",
            "white",
            "water",
            "white",
            "well",
            "white",
            "white",
            "wind",
            "white",
            "worms"
        }

        Dim imgL As New List(Of Bitmap)
        For Each frame In order
            For n = 1 To (FRAME_LENGTH + (RND.Next(1000, 9999) Mod 20))
                imgL.Add(FrameDict(frame))
            Next
        Next

        Images = imgL.ToArray
    End Sub

    Public Function TurnOn(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) As Color? Implements IColorManager.TurnOn
    End Function

    Public Function Manage(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) As Color? Implements IColorManager.Manage
        With Particle
            Return Images(Tick Mod Images.Length).GetPixel(Math.Floor(.CurrentPosition.X), Math.Floor(.CurrentPosition.Y))
        End With
    End Function

End Class
