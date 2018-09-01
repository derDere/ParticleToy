Module MathUtils

    Private KnownColorCount As Integer = [Enum].GetValues(GetType(KnownColor)).Length
    Private KnownColors As KnownColor() = [Enum].GetValues(GetType(KnownColor))
    <DebuggerHidden> _
    Public Function RandomColor() As Pen
        Dim I As Integer = RND.Next(1000000) Mod KnownColorCount
        Return New Pen(Color.FromKnownColor(KnownColors(I)))
    End Function

    <DebuggerHidden> _
    Public Function CenterOfPoints(Points As Point()) As Point
        Dim sX As Integer = 0
        Dim sY As Integer = 0
        For Each P As Point In Points
            sX += P.X
            sY += P.Y
        Next
        Dim cX As Integer = Math.Round(sX / Points.Length)
        Dim cY As Integer = Math.Round(sY / Points.Length)
        Dim Center As New Point(cX, cY)
        Return Center
    End Function

    <DebuggerHidden> _
    Public Function RndDirectedAngel(Target As Double, Randomness As Double) As Double
        Dim Result As Double = Target
        Result -= (Randomness / 2)
        Result += RND.Next(Randomness * 1000) Mod Randomness
        Result = ValidDegrees(Result)
        Return Result
    End Function

    <DebuggerHidden> _
    Public Function Offset(P As Point, xD As Integer, yD As Integer) As Point
        Return New Point(P.X + xD, P.Y + yD)
    End Function

    Public ReadOnly RND As New Random

    <DebuggerHidden> _
    Public Function RndPoint(Bounds As Rectangle) As Point
        Return New Point(Bounds.X + (RND.Next(1000000) Mod Bounds.Width), Bounds.Y + (RND.Next(1000000) Mod Bounds.Height))
    End Function
    <DebuggerHidden> _
    Public Function RndPoint(Bounds As Size) As Point
        Return RndPoint(New Rectangle(0, 0, Bounds.Width, Bounds.Height))
    End Function

    <DebuggerHidden> _
    Public Function RndDegrees() As Double
        Return (RND.Next(1000000) / 100) Mod 360
    End Function

    ''' <param name="S">Source</param>
    ''' <param name="T">Target</param>
    <DebuggerHidden> _
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

    <DebuggerHidden> _
    Public Function ValidDegrees(Degrees As Double) As Double
        Dim Result As Double = Degrees
        While Result < 0
            Result += 360
        End While
        Result = Result Mod 360
        Return Result
    End Function

    <DebuggerHidden> _
    Public Function DegreesToXY(ByVal degrees As Double, ByVal radius As Double, ByVal origin As Point) As Point
        Dim xy As PointF = New PointF()
        Dim radians As Double = degrees * Math.PI / 180.0
        xy.X = CSng(Math.Cos(radians)) * radius + origin.X
        xy.Y = CSng(Math.Sin(-radians)) * radius + origin.Y
        Return New Point(Math.Round(xy.X), Math.Round(xy.Y))
    End Function

    <DebuggerHidden> _
    Public Function XYToDegrees(ByVal xy As Point, ByVal origin As Point) As Single
        Dim deltaX As Integer = origin.X - xy.X
        Dim deltaY As Integer = origin.Y - xy.Y
        Dim radAngle As Double = Math.Atan2(deltaY, deltaX)
        Dim degreeAngle As Double = radAngle * 180.0 / Math.PI
        Return CSng((180.0 - degreeAngle))
    End Function

    <DebuggerHidden> _
    Public Function DeltaBetweed(A As Point, B As Point) As Double
        Dim minX As Integer = A.X
        Dim minY As Integer = A.Y
        Dim maxX As Integer = B.X
        Dim maxY As Integer = B.Y

        If maxX < minX Then switch(maxX, minX)
        If maxY < minY Then switch(maxY, minY)

        If maxX = minX Then Return maxY - minY
        If maxY = minY Then Return maxX - minX

        Dim xD As Double = maxX - minX
        Dim yD As Double = maxY - minY

        Return Math.Sqrt((xD * xD) + (yD * yD))
    End Function

    <DebuggerHidden> _
    Public Sub switch(Of T)(ByRef A As T, ByRef B As T)
        Dim TMP As T = A
        A = B
        B = TMP
    End Sub

End Module
