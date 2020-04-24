Imports System.Runtime.CompilerServices

Public Interface IBehaviour

    ReadOnly Property Key As String

    ReadOnly Property Icon As Bitmap

    ReadOnly Property Name As String

    ReadOnly Property ColorManager As IColorManager

    ReadOnly Property OverwriteColorManager As Boolean

    ReadOnly Property IsSelected As String

    ReadOnly Property IsUnlocked As Boolean

    Sub TurnedOff(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Microsoft.VisualBasic.Devices.Keyboard)

    Sub TurnedOn(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Microsoft.VisualBasic.Devices.Keyboard)

    'Sub Normalize(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Microsoft.VisualBasic.Devices.Keyboard)

    Function Behave(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Microsoft.VisualBasic.Devices.Keyboard) As Boolean

End Interface

Namespace Behaviour
    Public Module BehaviourValues

        Public Const MAX_SPEED As Double = 10
        Public Const MIN_SPEED As Double = 1
        Public Const APROACH_SPEED As Double = 4
        Public Const TURN_SPEED As Double = 20
        Public Const BOUNCE_RADIUS As Double = 50
        Public Const ASIDE_RADIUS As Double = 20
        Public Const GROUP_SIZE As Integer = 100
        Public Const ANTS_SPEED As Integer = 2

        <DebuggerHidden>
        Public Function RndSpeed() As Double
            Return (RND.Next(1000) Mod ((MAX_SPEED - MIN_SPEED) * 0.75)) + MIN_SPEED
        End Function

        <DebuggerHidden>
        Public Function RndChar() As Char
            Const AllChars As String = "#+*-:;?=)(/&%$§abcdefghijklmnopqrstuvwxyzАБбВГДЖИЛПФЦЧШЩЪЬЭЮЯABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789αβϐΓγΔδεϵζηθϑικϰΛλμνΞξοΠπϖπρϱΣστυϒεουΦφϕχΨΩω"
            Return AllChars(RND.Next(10000, 99999) Mod AllChars.Length)
        End Function

        <DebuggerHidden>
        <Extension>
        Public Function Randomize(Color As Color, Amount As Integer) As Color
            Dim R As Integer = Color.R + ((RND.Next(1000, 9999) Mod Amount) - Math.Round(Amount / 2))
            Dim G As Integer = Color.G + ((RND.Next(1000, 9999) Mod Amount) - Math.Round(Amount / 2))
            Dim B As Integer = Color.B + ((RND.Next(1000, 9999) Mod Amount) - Math.Round(Amount / 2))

            If R < 0 Then R = 0
            If G < 0 Then G = 0
            If B < 0 Then B = 0

            If R > 255 Then R = 255
            If G > 255 Then G = 255
            If B > 255 Then B = 255

            Return Color.FromArgb(Color.A, R, G, B)
        End Function

        <DebuggerHidden>
        <Extension>
        Public Function Randomize(P As Pen, Amount As Integer) As Pen
            Dim p2 As Pen = P.Clone
            p2.Color = P.Color.Randomize(Amount)
            Return p2
        End Function

        <DebuggerHidden>
        <Extension>
        Public Function SetAlpha(P As Pen, Alpha As Integer) As Pen
            Dim p2 As Pen = P.Clone
            p2.Color = Color.FromArgb(Alpha, P.Color)
            Return p2
        End Function

        <DebuggerHidden>
        <Extension>
        Public Function ToPen(C As Drawing.Color, Optional Width As Integer = 1) As Pen
            Return New Pen(C, Width)
        End Function

    End Module
End Namespace
