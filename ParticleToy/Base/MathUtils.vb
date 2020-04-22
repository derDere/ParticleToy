Imports System.Runtime.CompilerServices
Imports ParticleToy.Behaviour

Module MathUtils

    <DebuggerHidden>
    Public Function chance(Value As Integer, Base As Integer) As Boolean
        If (RND.Next(1000000, 9999999) Mod Base) <= (Value - 1) Then
            Return True
        End If
        Return False
    End Function

    <DebuggerHidden>
    Public Function chance(Base As Integer) As Boolean
        Return chance(1, Base)
    End Function

    <Extension>
    Public Function Add(P As PointF, X As Double, Y As Double) As PointF
        Return New PointF(P.X + X, P.Y + Y)
    End Function

    <Extension>
    Public Function Add(P1 As PointF, P2 As PointF) As PointF
        Return New PointF(P1.X + P2.X, P1.Y + P2.Y)
    End Function

    Public OutSides As Integer()() = {
        New Integer() {-1, -1},
        New Integer() {-1, 0},
        New Integer() {0, -1},
        New Integer() {1, -1},
        New Integer() {-1, 1},
        New Integer() {1, 0},
        New Integer() {0, 1},
        New Integer() {1, 1}
    }

    Private KnownColorCount As Integer = [Enum].GetValues(GetType(KnownColor)).Length
    Private KnownColors As KnownColor() = [Enum].GetValues(GetType(KnownColor))
    <DebuggerHidden>
    Public Function RandomColor() As Pen
        Dim I As Integer = RND.Next(1000000) Mod KnownColorCount
        Return Color.FromKnownColor(KnownColors(I)).ToPen
    End Function

    <DebuggerHidden>
    Public Function CenterOfPoints(Points As PointF()) As PointF
        Dim sX As Double = 0
        Dim sY As Double = 0
        For Each P As PointF In Points
            sX += P.X
            sY += P.Y
        Next
        Dim cX As Double = sX / Points.Length
        Dim cY As Double = sY / Points.Length
        Dim Center As New PointF(cX, cY)
        Return Center
    End Function

    <DebuggerHidden>
    Public Function RndDirectedAngel(Target As Double, Randomness As Double) As Double
        If Randomness = 0 Then Return Target
        Dim Result As Double = Target
        Result -= (Randomness / 2)
        Result += RND.Next(Randomness * 1000) Mod Randomness
        Result = ValidDegrees(Result)
        Return Result
    End Function

    <DebuggerHidden>
    Public Function Offset(P As PointF, xD As Double, yD As Double) As PointF
        Return New PointF(P.X + xD, P.Y + yD)
    End Function

    Public ReadOnly RND As New Random

    <DebuggerHidden>
    Public Function RndPoint(Bounds As Rectangle) As PointF
        Return New PointF(Bounds.X + (RND.Next(1000000) Mod Bounds.Width), Bounds.Y + (RND.Next(1000000) Mod Bounds.Height))
    End Function
    <DebuggerHidden>
    Public Function RndPoint(Bounds As Size) As PointF
        Return RndPoint(New Rectangle(0, 0, Bounds.Width, Bounds.Height))
    End Function

    <DebuggerHidden>
    Public Function RndDegrees() As Double
        Return (RND.Next(1000000) / 100) Mod 360
    End Function

    ''' <param name="S">Source</param>
    ''' <param name="T">Target</param>
    <DebuggerHidden>
    Public Function DegreeDirection(S As Double, T As Double) As Double
        If S = T Then Return 0
        If S > T Then
            If (S - T) < (T + (360 - S)) Then
                Return -1
            Else
                Return 1
            End If
        Else
            If (T - S) < (S + (360 - T)) Then
                Return 1
            Else
                Return -1
            End If
        End If
    End Function

    <DebuggerHidden>
    Public Function ValidDegrees(Degrees As Double) As Double
        Dim Result As Double = Degrees
        While Result < 0
            Result += 360
        End While
        Result = Result Mod 360
        Return Result
    End Function

    <DebuggerHidden>
    Public Function DegreesToXY(ByVal degrees As Double, ByVal radius As Double, ByVal origin As PointF) As PointF
        Dim xy As PointF = New PointF()
        Dim radians As Double = degrees * Math.PI / 180.0
        xy.X = CSng(Math.Cos(radians)) * radius + origin.X
        xy.Y = CSng(Math.Sin(-radians)) * radius + origin.Y
        Return New PointF(xy.X, xy.Y)
    End Function

    <DebuggerHidden>
    Public Function XYToDegrees(ByVal xy As PointF, ByVal origin As PointF) As Single
        Dim deltaX As Double = origin.X - xy.X
        Dim deltaY As Double = origin.Y - xy.Y
        Dim radAngle As Double = Math.Atan2(deltaY, deltaX)
        Dim degreeAngle As Double = radAngle * 180.0 / Math.PI
        Return CSng((180.0 - degreeAngle))
    End Function

    <DebuggerHidden>
    Public Function XYToDegreesFastest(ByVal xy As PointF, ByVal origin As PointF, ScreenSize As Size) As Double
        Dim Angle As Double = XYToDegrees(xy, origin)
        Dim Delta As Double = DeltaBetweed(xy, origin)
        For Each Side As Integer() In OutSides
            Dim P As New PointF(xy.X + (Side(0) * ScreenSize.Width), xy.Y + (Side(1) * ScreenSize.Height))
            Dim d As Double = DeltaBetweed(origin, P)
            If d < Delta Then
                Delta = d
                Angle = XYToDegrees(P, origin)
            End If
        Next
        Return Angle
    End Function

    <DebuggerHidden>
    Public Function DeltaBetweedFastest(xy As PointF, origin As PointF, ScreenSize As Size) As Double
        Dim Delta As Double = DeltaBetweed(xy, origin)
        For Each Side As Integer() In OutSides
            Dim P As New PointF(xy.X + (Side(0) * ScreenSize.Width), xy.Y + (Side(1) * ScreenSize.Height))
            Dim d As Double = DeltaBetweed(origin, P)
            If d < Delta Then
                Delta = d
            End If
        Next
        Return Delta
    End Function

    <DebuggerHidden>
    Public Function DeltaBetweed(A As PointF, B As PointF) As Double
        Dim minX As Double = A.X
        Dim minY As Double = A.Y
        Dim maxX As Double = B.X
        Dim maxY As Double = B.Y

        If maxX < minX Then switch(maxX, minX)
        If maxY < minY Then switch(maxY, minY)

        If maxX = minX Then Return maxY - minY
        If maxY = minY Then Return maxX - minX

        Dim xD As Double = maxX - minX
        Dim yD As Double = maxY - minY

        Return Math.Sqrt((xD * xD) + (yD * yD))
    End Function

    <DebuggerHidden>
    Public Sub switch(Of T)(ByRef A As T, ByRef B As T)
        Dim TMP As T = A
        A = B
        B = TMP
    End Sub

    Public Function HsvToRgb(ByVal h As Double, ByVal S As Double, ByVal V As Double) As Color

        While h < 0
            h += 360
        End While

        While h >= 360
            h -= 360
        End While

        Dim R, G, B As Double

        If V <= 0 Then
            R = 0
            G = 0
            B = 0
        ElseIf S <= 0 Then
            R = V
            G = V
            B = V
        Else
            Dim hf As Double = h / 60.0
            Dim i As Integer = CInt(Math.Floor(hf))
            Dim f As Double = hf - i
            Dim pv As Double = V * (1 - S)
            Dim qv As Double = V * (1 - S * f)
            Dim tv As Double = V * (1 - S * (1 - f))

            Select Case i
                Case 0
                    R = V
                    G = tv
                    B = pv
                Case 1
                    R = qv
                    G = V
                    B = pv
                Case 2
                    R = pv
                    G = V
                    B = tv
                Case 3
                    R = pv
                    G = qv
                    B = V
                Case 4
                    R = tv
                    G = pv
                    B = V
                Case 5
                    R = V
                    G = pv
                    B = qv
                Case 6
                    R = V
                    G = tv
                    B = pv
                Case -1
                    R = V
                    G = pv
                    B = qv
                Case Else
                    R = V
                    G = V
                    B = V
            End Select
        End If

        R = Clamp(CInt((R * 255.0)))
        G = Clamp(CInt((G * 255.0)))
        B = Clamp(CInt((B * 255.0)))

        Return Color.FromArgb(255, R, G, B)
    End Function

    Public Function Clamp(ByVal i As Integer) As Integer
        If i < 0 Then Return 0
        If i > 255 Then Return 255
        Return i
    End Function

End Module
