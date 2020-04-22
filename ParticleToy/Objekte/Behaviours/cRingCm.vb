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

        Dim Keys As New List(Of String)

        Using archive As New IO.Compression.ZipArchive(New IO.MemoryStream(My.Resources.ring_zip))
            For Each entry As IO.Compression.ZipArchiveEntry In archive.Entries
                Using entryStream As IO.Stream = entry.Open
                    Dim b As New Bitmap(entryStream)
                    Keys.Add(entry.Name.Replace(".PNG", ""))
                    FrameDict.Add(entry.Name.Replace(".PNG", ""), b)
                End Using
            Next
        End Using

        Keys.Remove("ring")

        Dim order As New List(Of String)
        While Keys.Count > 0
            Dim key As String = Keys(RND.Next(10000, 99999) Mod Keys.Count)
            Keys.Remove(key)
            order.Add(key)
        End While

        'Dim order As String() = {
        '    "white",
        '    "chair",
        '    "white",
        '    "deer",
        '    "white",
        '    "face",
        '    "white",
        '    "fingers",
        '    "white",
        '    "fire",
        '    "white",
        '    "her",
        '    "white",
        '    "house",
        '    "white",
        '    "mirror",
        '    "white",
        '    "moon",
        '    "white",
        '    "ring",
        '    "white",
        '    "ring",
        '    "white",
        '    "mouth",
        '    "white",
        '    "nail",
        '    "white",
        '    "noise",
        '    "white",
        '    "table",
        '    "white",
        '    "tree",
        '    "white",
        '    "water",
        '    "white",
        '    "well",
        '    "white",
        '    "white",
        '    "wind",
        '    "white",
        '    "worms"
        '}

        Dim imgL As New List(Of Bitmap)
        For Each frame In order
            Dim frameCount As Integer = (RND.Next(1000, 9999) Mod 5) + 5
            If chance(3) Then
                Dim whiteCount As Integer = (RND.Next(1000, 9999) Mod 4) + 1
                For n = 1 To whiteCount
                    imgL.Add(My.Resources.white)
                Next
            End If
            If chance(3) Then
                Dim ringCount As Integer = (RND.Next(1000, 9999) Mod 5) + 1
                Dim whiteCount As Integer = (RND.Next(1000, 9999) Mod 4) + 1
                For n = 1 To ringCount
                    imgL.Add(FrameDict("ring"))
                Next
                For n = 1 To whiteCount
                    imgL.Add(My.Resources.white)
                Next
            End If
            For n = 1 To frameCount
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
