Public Class Particle

    Public Shared ReadOnly ElectricAngles As Double() = {0, 45, 90, 135, 180, 225, 270, 315}

    Const ROAMING As Integer = 0
    Const TESLA As Integer = 1
    Const TELEPORT As Integer = 2
    Const COLLECTING As Integer = 3
    Const CYCLING As Integer = 4
    Const FLOWING As Integer = 5
    Const ELECTRIC As Integer = 6
    Const SYNCED As Integer = 7
    Const STADDY As Integer = 8
    Const MIRRORED As Integer = 9
    Const STARS As Integer = 10
    Const GROUPED As Integer = 11
    Const GAME_OF_LIFE As Integer = 12

    Const MAX_SPEED As Double = 10
    Const MIN_SPEED As Double = 1
    Const APROACH_SPEED As Double = 4
    Const TURN_SPEED As Double = 20
    Const BOUNCE_RADIUS As Double = 50
    Const ASIDE_RADIUS As Double = 20
    Const GROUP_SIZE As Integer = 100

    Friend MyIndex As Integer
    Friend Parent As Game
    Friend Ancs As Anchors
    Friend CurrentPosition As Point = Nothing
    Friend LastPosition As Point = Nothing
    Friend CurrentAngel As Double = 0
    Friend CurrentSpeed As Double = MIN_SPEED
    Friend CurrentColor As Pen
    Friend CurrentColorBru As SolidBrush

    Public Color As Pen = Pens.White
    Public TargetAngel As Double = 0
    Public TargetSpeed As Double = MIN_SPEED
    Public Lightnight As Point? = Nothing
    Public Glowing As Boolean = False
    'Public GlowingPen As New Pen(Drawing.Color.FromArgb(Glowing, 255, 255, 255), 1)

    Friend GolPos As Point
    Public Shared ReadOnly GolQuader As Integer = Math.Sqrt(Game.PARTICLE_NUMBER)
    Public GOL_MARGIN As Integer = 5
    Public Shared Gol_Matrix(GolQuader, GolQuader) As Particle
    Public GolMP As Point

    Public Sub New(Ancs As Anchors, Position As Point, Index As Integer, Parent As Game)
        Me.Parent = Parent
        MyIndex = Index
        Dim G As Integer = 128
        G += RND.Next(1000) Mod 128
        Color = New Pen(Drawing.Color.FromArgb(255, 0, G, 255))
        CurrentColor = Color
        CurrentAngel = RndDegrees()
        TargetAngel = CurrentAngel
        CurrentPosition = Position
        LastPosition = Position
        Me.Ancs = Ancs
        'Calculate GameOfLife Position
        Dim X As Integer = MyIndex Mod GolQuader
        Dim Y As Integer = (MyIndex - (MyIndex Mod GolQuader)) / GolQuader
        GolPos = New Point((X * GOL_MARGIN) + ((Game.OPT_SIZE_W - (GolQuader * GOL_MARGIN)) / 2),
                           (Y * GOL_MARGIN) + ((Game.OPT_SIZE_H - (GolQuader * GOL_MARGIN)) / 2))
        GolMP = New Point(X, Y)
        If Gol_Matrix(X, Y) Is Nothing Then
            Gol_Matrix(X, Y) = Me
        Else
            Stop
        End If
    End Sub

    Public Shared AnchorCenter As Point? = Nothing
    Public Shared SortedAnchors As New List(Of Point)
    Public WillGlow As Boolean = False
    Friend AnchorIndex As Integer = -1
    Friend AnchorStep As Integer = 0
    Friend SpeedIsSet As Boolean = False
    Friend IsElectric As Boolean = False
    Friend IsAStar As Boolean = False
    Friend Partner As Particle = Nothing
    Friend MirrorPoint As Point? = Nothing
    Friend HideNoMovement As Boolean = True
    Friend GolStatusSet As Boolean = False
    Friend GolPositionFound As Boolean = False

    Public Sub Update(Game As GameBase, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Microsoft.VisualBasic.Devices.Keyboard)
        'Normalisierung
        '#######################################################################################
        Lightnight = Nothing
        If Ancs.Anchors.Count <> CYCLING Then
            AnchorIndex = -1
            AnchorStep = 0
            If SortedAnchors.Count > 0 Then _
                SortedAnchors.Clear()
        End If
        If Ancs.Anchors.Count <> COLLECTING Then
            AnchorCenter = Nothing
        End If
        If Not (New Integer() {FLOWING, CYCLING}).Contains(Ancs.Anchors.Count) Then
            SpeedIsSet = False
        End If
        If Not (New Integer() {ELECTRIC, MIRRORED, STARS, STADDY, GROUPED}).Contains(Ancs.Anchors.Count) Then
            IsElectric = False
            CurrentColor = Color
        End If
        If Ancs.Anchors.Count <> STARS Then
            IsAStar = False
        End If
        If Ancs.Anchors.Count <> GROUPED Then
            Partner = Nothing
        End If
        If Ancs.Anchors.Count <> MIRRORED Then
            MirrorPoint = Nothing
        End If
        If Ancs.Anchors.Count <> GAME_OF_LIFE Then
            GolStatusSet = False
            WillGlow = False
            Glowing = False
            GolPositionFound = False
        End If
        HideNoMovement = Ancs.Anchors.Count <> GAME_OF_LIFE

        'Verhalten
        '#######################################################################################
        If Ancs.Anchors.Count = ROAMING Then 'Freilaufend 0 =============================================
            TargetSpeed = MIN_SPEED
            If MouseInfo.Position IsNot Nothing AndAlso DeltaBetweed(CurrentPosition, MouseInfo.Position) < ASIDE_RADIUS Then
                TargetAngel = XYToDegrees(CurrentPosition, MouseInfo.Position)
            Else
                TargetAngel = RndDegrees()
            End If

        ElseIf Ancs.Anchors.Count = TESLA Then 'Tesla 1 =============================================
            TargetSpeed = APROACH_SPEED
            Dim DeltaToAnc As Double = DeltaBetweed(CurrentPosition, Ancs.Anchors(0))
            If DeltaToAnc < (BOUNCE_RADIUS + 20) Then
                If DeltaToAnc < BOUNCE_RADIUS Then
                    CurrentSpeed = MAX_SPEED
                    TargetAngel = XYToDegrees(CurrentPosition, Ancs.Anchors(0))
                    CurrentAngel = TargetAngel
                    Lightnight = Ancs.Anchors(0)
                Else
                    TargetAngel = RndDegrees()
                End If
            Else
                TargetAngel = RndDirectedAngel(XYToDegrees(Ancs.Anchors(0), CurrentPosition), 45)
            End If

        ElseIf Ancs.Anchors.Count = TELEPORT Then 'Teleportierend 2 =============================================
            TargetSpeed = APROACH_SPEED
            TargetAngel = RndDirectedAngel(XYToDegrees(Ancs.Anchors(0), CurrentPosition), 180)
            If DeltaBetweed(CurrentPosition, Ancs.Anchors(0)) < ASIDE_RADIUS Then
                Lightnight = Ancs.Anchors(0)
                CurrentSpeed = MAX_SPEED
                CurrentAngel = RndDegrees()
                CurrentPosition = DegreesToXY(CurrentAngel, BOUNCE_RADIUS, Ancs.Anchors(1))
                CurrentAngel = RndDegrees()
                LastPosition = CurrentPosition
            End If

        ElseIf Ancs.Anchors.Count = COLLECTING Then 'Sammelnd 3 =============================================
            TargetSpeed = APROACH_SPEED
            If AnchorCenter Is Nothing Then
                AnchorCenter = CenterOfPoints(Ancs.Anchors.ToArray)
            End If
            Dim MouseDelta As Double
            If MouseInfo.Position IsNot Nothing Then
                MouseDelta = DeltaBetweed(CurrentPosition, MouseInfo.Position)
            Else
                MouseDelta = 10000
            End If
            If MouseDelta < ASIDE_RADIUS Then
                TargetAngel = RndDirectedAngel(XYToDegrees(CurrentPosition, MouseInfo.Position), 30)
            Else
                TargetAngel = RndDirectedAngel(XYToDegrees(AnchorCenter, CurrentPosition), 90)
            End If

        ElseIf Ancs.Anchors.Count = CYCLING Then 'Keisend 4 =============================================
            If Not SpeedIsSet Then
                SpeedIsSet = True
                TargetSpeed = RndSpeed()
            End If
            If SortedAnchors.Count = 0 Then
                SortAnchors(Ancs)
            End If
            If AnchorIndex = -1 Then
                AnchorStep = IIf((RND.Next(1000) Mod 2) = 0, 1, -1)
                Dim MinD As Double = Double.MaxValue
                For i As Integer = 0 To SortedAnchors.Count - 1
                    Dim D As Double = DeltaBetweed(CurrentPosition, SortedAnchors(i))
                    If D < MinD Then
                        MinD = D
                        AnchorIndex = i
                    End If
                Next
            Else
                Dim AnchorDelta As Double = DeltaBetweed(CurrentPosition, SortedAnchors(AnchorIndex))
                If AnchorDelta < BOUNCE_RADIUS Then
                    Lightnight = SortedAnchors(AnchorIndex)
                    AnchorIndex += AnchorStep
                    If AnchorIndex >= SortedAnchors.Count Then AnchorIndex = 0
                    If AnchorIndex < 0 Then AnchorIndex = SortedAnchors.Count - 1
                End If
                TargetAngel = RndDirectedAngel(XYToDegrees(SortedAnchors(AnchorIndex), CurrentPosition), 135)
            End If

        ElseIf Ancs.Anchors.Count = FLOWING Then 'Fliesend 5 =============================================
            If Not SpeedIsSet Then
                SpeedIsSet = True
            End If
            TargetSpeed = RndSpeed()
            Dim Rocks As New List(Of Point)
            Rocks.Add(MouseInfo.Position)
            Rocks.AddRange(Ancs.Anchors)
            Dim IsNear As Point? = Nothing
            For Each Rock As Point In Rocks
                Dim delta As Double = DeltaBetweed(Rock, CurrentPosition)
                If delta < ASIDE_RADIUS Then
                    IsNear = Rock
                    Exit For
                End If
            Next
            If IsNear IsNot Nothing Then
                TargetAngel = RndDirectedAngel(XYToDegrees(CurrentPosition, IsNear), 45)
            Else
                TargetAngel = RndDirectedAngel(315, 90)
            End If

        ElseIf Ancs.Anchors.Count = ELECTRIC Then 'Electrich 6 =============================================
            If Not IsElectric Then
                IsElectric = True
                TargetSpeed = RndSpeed()
            End If
            'If MouseInfo.Position IsNot Nothing Then
            'Dim MouseDelta As Double = DeltaBetweed(CurrentPosition, MouseInfo.Position)
            'End If
            TargetAngel = ElectricAngles(RND.Next(1000) Mod ElectricAngles.Length)
            CurrentAngel = TargetAngel
            Dim G As Integer = ((((CurrentPosition.X + CurrentPosition.Y) / 2) + (2 * Tick)) Mod 256)
            If G < 128 Then
                G += 128
            Else
                G = 255 - (G - 128)
            End If
            CurrentColor = New Pen(Drawing.Color.FromArgb(255, 0, G, 192))

        ElseIf Ancs.Anchors.Count = SYNCED Then 'Syncron 7 =============================================
            TargetSpeed = MAX_SPEED
            Dim Rocks As New List(Of Point)
            If MouseInfo.Position IsNot Nothing Then _
                Rocks.Add(MouseInfo.Position)
            Rocks.AddRange(Ancs.Anchors)
            Dim IsNear As Point? = Nothing
            For Each Rock As Point In Rocks
                Dim delta As Double = DeltaBetweed(Rock, CurrentPosition)
                If delta < ASIDE_RADIUS Then
                    IsNear = Rock
                    Exit For
                End If
            Next
            If IsNear IsNot Nothing OrElse (Tick Mod 70) = 0 Then
                TargetAngel = ElectricAngles(RND.Next(1000) Mod ElectricAngles.Length)
            End If

        ElseIf Ancs.Anchors.Count = STADDY Then 'Unbeweglich 8 =============================================
            TargetSpeed = 0
            TargetAngel = RndDegrees()
            Dim Rocks As New List(Of Point)
            Rocks.Add(MouseInfo.Position)
            Rocks.AddRange(Ancs.Anchors)
            Dim IsNear As Point? = Nothing
            For Each Rock As Point In Rocks
                Dim delta As Double = DeltaBetweed(Rock, CurrentPosition)
                If delta < ASIDE_RADIUS Then
                    IsNear = Rock
                    Exit For
                End If
            Next
            If IsNear IsNot Nothing Then
                CurrentSpeed = RndSpeed()
            End If
            If (RND.Next(1000000) Mod 1000) = 0 Then
                CurrentPosition = DegreesToXY(RndDegrees, ASIDE_RADIUS, Ancs.Anchors(RND.Next(1000) Mod Ancs.Anchors.Count))
                LastPosition = CurrentPosition
            End If
            If (RND.Next(1000000) Mod 2000) = 0 Then
                CurrentSpeed = APROACH_SPEED
            End If
            Dim G As Integer = 255 - (RND.Next(1000) Mod 64)
            CurrentColor = New Pen(Drawing.Color.FromArgb(255, 255, G, 192))

        ElseIf Ancs.Anchors.Count = MIRRORED Then 'Gespiegelt 9 =============================================
            If (MyIndex Mod 2) = 0 Then
                Dim p As Double = CurrentPosition.Y / Game.ScreenSize.Height
                TargetSpeed = MIN_SPEED + ((MAX_SPEED - MIN_SPEED) * p)
                CurrentSpeed = TargetSpeed
                If MouseInfo.Position IsNot Nothing AndAlso DeltaBetweed(CurrentPosition, MouseInfo.Position) < ASIDE_RADIUS Then
                    TargetAngel = XYToDegrees(CurrentPosition, MouseInfo.Position)
                Else
                    If MirrorPoint Is Nothing Then
                        MirrorPoint = Ancs.Anchors(RND.Next(1000) Mod Ancs.Anchors.Count)
                    ElseIf DeltaBetweed(CurrentPosition, MirrorPoint.Value) < BOUNCE_RADIUS Then
                        MirrorPoint = Ancs.Anchors(RND.Next(1000) Mod Ancs.Anchors.Count)
                    End If
                    TargetAngel = RndDirectedAngel(XYToDegrees(MirrorPoint, CurrentPosition), 180)
                End If
                Dim G As Integer = ((((CurrentPosition.X + CurrentPosition.Y) / 2) + (2 * Tick)) Mod 256)
                If G < 128 Then
                    G += 128
                Else
                    G = 255 - (G - 128)
                End If
                CurrentColor = New Pen(Drawing.Color.FromArgb(255, 255, 0, G))
            Else
                Dim Partner As Particle = Parent.PL(MyIndex - 1)
                Me.LastPosition = New Point(Game.ScreenSize.Width - Partner.LastPosition.X, Partner.LastPosition.Y)
                Me.CurrentPosition = New Point(Game.ScreenSize.Width - Partner.CurrentPosition.X, Partner.CurrentPosition.Y)
                Me.CurrentColor = Partner.CurrentColor
                Exit Sub
            End If

        ElseIf Ancs.Anchors.Count = STARS Then 'Sterne 10 =============================================
            If Not IsAStar Then
                IsAStar = True
                TargetSpeed = MIN_SPEED + (RND.Next(1000) Mod 2)
                TargetAngel = 0
            End If

            Dim MouseDelta As Double = DeltaBetweed(CurrentPosition, MouseInfo.Position)
            Dim ColorPart As Integer = -1
            For Each Anc In Ancs.Anchors
                Dim AncDelta As Double = DeltaBetweed(CurrentPosition, Anc)
                If AncDelta < MouseDelta Then
                    MouseDelta = AncDelta
                    ColorPart = Ancs.Anchors.IndexOf(Anc) Mod 3
                End If
            Next
            Dim RedLevel As Integer = 0
            If MouseDelta < BOUNCE_RADIUS Then
                RedLevel = (BOUNCE_RADIUS - MouseDelta) * 3
            End If
            Dim ColorVal As Integer = 255 - ((2 - TargetSpeed) * 100)
            Select Case ColorPart
                Case 2
                    CurrentColor = New Pen(Drawing.Color.FromArgb(255, ColorVal - RedLevel, ColorVal - RedLevel, ColorVal))
                Case 1
                    CurrentColor = New Pen(Drawing.Color.FromArgb(255, ColorVal - RedLevel, ColorVal, ColorVal - RedLevel))
                Case 0
                    CurrentColor = New Pen(Drawing.Color.FromArgb(255, ColorVal, ColorVal - RedLevel, ColorVal - RedLevel))
                Case Else
                    CurrentColor = New Pen(Drawing.Color.FromArgb(255, ColorVal - (RedLevel * 0.3333), ColorVal - RedLevel, ColorVal))
            End Select

        ElseIf Ancs.Anchors.Count = GROUPED Then 'Gruppierd 11 =============================================
            If Partner Is Nothing OrElse Partner Is Me Then
                If (MyIndex Mod GROUP_SIZE) = 0 Then
                    Partner = Parent.PL((MyIndex + 1) Mod Parent.PL.Count)
                Else
                    Partner = Parent.PL(MyIndex - (MyIndex Mod GROUP_SIZE))
                End If
                CurrentColor = RandomColor()
            Else
                If (MyIndex Mod GROUP_SIZE) = 0 Then
                    TargetAngel = RndDegrees()
                    If (RND.Next(1000) Mod 100) = 0 Then
                        CurrentColor = RandomColor()
                    End If
                Else
                    Dim PartnerDelta As Double = DeltaBetweed(CurrentPosition, Partner.CurrentPosition)
                    If PartnerDelta > ASIDE_RADIUS Then
                        TargetAngel = RndDirectedAngel(XYToDegrees(Partner.CurrentPosition, CurrentPosition), 45)
                        TargetSpeed = APROACH_SPEED
                    Else
                        TargetAngel = RndDegrees()
                        TargetSpeed = MIN_SPEED * 2
                    End If
                    If (RND.Next(1000) Mod 20) = 0 Then
                        CurrentColor = Partner.CurrentColor
                    End If
                End If
                If MouseInfo.Position IsNot Nothing AndAlso DeltaBetweed(CurrentPosition, MouseInfo.Position) < ASIDE_RADIUS Then
                    TargetAngel = XYToDegrees(CurrentPosition, MouseInfo.Position)
                End If
                For Each Anc As Point In Ancs.Anchors
                    If DeltaBetweed(CurrentPosition, Anc) < BOUNCE_RADIUS Then
                        TargetAngel = XYToDegrees(CurrentPosition, Anc)
                    End If
                Next
            End If

        ElseIf Ancs.Anchors.Count = GAME_OF_LIFE Then 'Game of Life 12 =============================================
            If Not GolStatusSet Then
                WillGlow = (RND.Next(1000) Mod 5) = 0
                Glowing = IIf(WillGlow, 100, 0)
                GolStatusSet = True
            Else
                If (Tick Mod 2) = 0 Then
                    Dim Sum As Integer = 0

                    If GolMN(GolMP.X - 1, GolMP.Y - 1).Glowing Then Sum += 1
                    If GolMN(GolMP.X - 0, GolMP.Y - 1).Glowing Then Sum += 1
                    If GolMN(GolMP.X + 1, GolMP.Y - 1).Glowing Then Sum += 1
                    If GolMN(GolMP.X - 1, GolMP.Y - 0).Glowing Then Sum += 1
                    If GolMN(GolMP.X + 1, GolMP.Y - 0).Glowing Then Sum += 1
                    If GolMN(GolMP.X - 1, GolMP.Y + 1).Glowing Then Sum += 1
                    If GolMN(GolMP.X - 0, GolMP.Y + 1).Glowing Then Sum += 1
                    If GolMN(GolMP.X + 1, GolMP.Y + 1).Glowing Then Sum += 1

                    If Sum < 2 Then WillGlow = False
                    If Sum = 3 Then WillGlow = True
                    If Sum > 3 Then WillGlow = False

                    If MouseInfo.Position IsNot Nothing Then
                        Dim MouseDelta As Double = DeltaBetweed(CurrentPosition, MouseInfo.Position)
                        If MouseDelta < ASIDE_RADIUS Then
                            If (RND.Next(1000) Mod 20) = 0 Then
                                WillGlow = True
                            End If
                        End If
                    End If
                End If
            End If
            TargetSpeed = MAX_SPEED
            Dim delta As Integer = DeltaBetweed(CurrentPosition, GolPos)
            If delta < BOUNCE_RADIUS * 2 Then
                TargetSpeed = APROACH_SPEED
            End If
            If delta < BOUNCE_RADIUS Then
                TargetSpeed = MIN_SPEED
                CurrentSpeed = MIN_SPEED
            End If
            If delta < APROACH_SPEED Then
                CurrentPosition = GolPos
                CurrentSpeed = 0
                TargetSpeed = 0
                GolPositionFound = True
            Else
                TargetAngel = XYToDegrees(GolPos, CurrentPosition)
            End If
            If GolPositionFound Then
                Dim AncDelta As Double = Double.MaxValue
                For Each Anc As Point In Ancs.Anchors
                    Dim d As Double = DeltaBetweed(GolPos, Anc)
                    If d < AncDelta Then AncDelta = d
                Next
                Dim m As Double = Math.Cos((AncDelta) / 10 + (-Tick / 10))
                CurrentPosition = New Point(GolPos.X, GolPos.Y - (5 * m))
            End If

        Else
            If TargetAngel = 0 Then TargetAngel = RndDegrees()
            TargetSpeed = MIN_SPEED
            CurrentColor = RandomColor()

        End If

        'Farb Gleichsetzung
        '#######################################################################################
        CurrentColorBru = New SolidBrush(CurrentColor.Color)

        'Steuerung
        '#######################################################################################
        If CurrentSpeed < TargetSpeed Then CurrentSpeed += IIf(CurrentSpeed > MAX_SPEED, 1, 0.5)
        If CurrentSpeed > TargetSpeed Then CurrentSpeed -= IIf(CurrentSpeed > MAX_SPEED, 1, 0.5)
        Dim TurnDirection As Double = DegreeDirection(CurrentAngel, TargetAngel)
        CurrentAngel += (TURN_SPEED * TurnDirection)
        CurrentAngel = ValidDegrees(CurrentAngel)
        If TurnDirection <> DegreeDirection(CurrentAngel, TargetAngel) Then
            CurrentAngel = TargetAngel
        End If

        'Bewegung
        '#######################################################################################
        Dim NewPos As Point = DegreesToXY(CurrentAngel, CurrentSpeed, CurrentPosition)
        MoveTo(NewPos)

        'Positions Korrectur
        '#######################################################################################
        If CurrentPosition.X >= Game.ScreenSize.Width Then
            CurrentPosition = New Point(0, CurrentPosition.Y)
            LastPosition = CurrentPosition
            Lightnight = Nothing
        End If
        If CurrentPosition.X < 0 Then
            CurrentPosition = New Point(Game.ScreenSize.Width - 1, CurrentPosition.Y)
            LastPosition = CurrentPosition
            Lightnight = Nothing
        End If
        If CurrentPosition.Y >= Game.ScreenSize.Height Then
            CurrentPosition = New Point(CurrentPosition.X, 0)
            LastPosition = CurrentPosition
            Lightnight = Nothing
        End If
        If CurrentPosition.Y < 0 Then
            CurrentPosition = New Point(CurrentPosition.X, Game.ScreenSize.Height - 1)
            LastPosition = CurrentPosition
            Lightnight = Nothing
        End If

        'Glowing Pen Update
        '#######################################################################################
        'If Ancs.Anchors.Count <> GAME_OF_LIFE Then
        '    Glowing -= 2
        '    If Glowing < 0 Then Glowing = 0
        'End If
        'GlowingPen = New Pen(Drawing.Color.FromArgb(Glowing, 255, 255, 255))
    End Sub

    Friend Shared LightningPen As New Pen(Drawing.Color.FromArgb(64, 255, 255, 255), 1)
    Public Sub Draw(G As Graphics)
        If Lightnight IsNot Nothing Then
            G.DrawLine(LightningPen, CurrentPosition, Lightnight.Value)
        End If
        If LastPosition <> CurrentPosition Then
            G.DrawLine(CurrentColor, LastPosition, CurrentPosition)
        ElseIf Not HideNoMovement Then
            G.FillRectangle(CurrentColorBru, CurrentPosition.X, CurrentPosition.Y, 1, 1)
        End If
        If Glowing Then
            G.DrawRectangle(Pens.White, CurrentPosition.X - 1, CurrentPosition.Y - 1, 2, 2)
            'G.FillRectangle(Brushes.White, CurrentPosition.X, CurrentPosition.Y, 1, 1)
        End If
    End Sub

    Public Sub MoveTo(NewPosition As Point?)
        LastPosition = CurrentPosition
        CurrentPosition = NewPosition
    End Sub

    <DebuggerHidden>
    Public Function RndSpeed() As Double
        Return (RND.Next(1000) Mod ((MAX_SPEED - MIN_SPEED) * 0.75)) + MIN_SPEED
    End Function

    Public Shared Sub SortAnchors(Anc As Anchors)
        SortedAnchors.AddRange(Anc.Anchors.ToArray)
        Dim Center As Point = CenterOfPoints(Anc.Anchors.ToArray)
        SortedAnchors.Sort(Function(A, B)
                               Dim aD As Double = XYToDegrees(A, Center)
                               Dim bD As Double = XYToDegrees(B, Center)
                               If aD = bD Then Return 0
                               If aD > bD Then Return 1
                               Return -1
                           End Function)
    End Sub

    <DebuggerHidden>
    Friend Shared Function GolMN(X As Integer, Y As Integer) As Particle
        If X < 0 Then X += GolQuader
        If Y < 0 Then Y += GolQuader
        If X >= GolQuader Then X = X Mod GolQuader
        If Y >= GolQuader Then Y = Y Mod GolQuader
        Return Gol_Matrix(X, Y)
    End Function

End Class

