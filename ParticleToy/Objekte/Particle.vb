Imports ParticleToy.Behaviour

Public Class Particle

    Public Shared ReadOnly ElectricAngles As Double() = {0, 45, 90, 135, 180, 225, 270, 315}

    'Const ROAMING As Integer = 0
    'Const TESLA As Integer = 1
    'Const TELEPORT As Integer = 2
    'Const COLLECTING As Integer = 3
    'Const CYCLING As Integer = 4
    'Const FLOWING As Integer = 5
    'Const ELECTRIC As Integer = 6
    'Const SYNCED As Integer = 7
    'Const STADDY As Integer = 8
    'Const MIRRORED As Integer = 9
    'Const STARS As Integer = 10
    'Const GROUPED As Integer = 11
    'Const GAME_OF_LIFE As Integer = 12

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

    Public Sub Update(Game As GameBase, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Microsoft.VisualBasic.Devices.Keyboard, Behaviour As IBehaviour, NotBehaviour As IBehaviour)
        'Normalisierung
        '#######################################################################################
        Lightnight = Nothing

        If NotBehaviour IsNot Nothing Then
            NotBehaviour.NormalizeNot(Me, Game, Tick, MouseInfo, Keyboard)
        End If
        Behaviour.Normalize(Me, Game, Tick, MouseInfo, Keyboard)

        'Verhalten
        '#######################################################################################
        If Not Behaviour.Behave(Me, Game, Tick, MouseInfo, Keyboard) Then Exit Sub

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

