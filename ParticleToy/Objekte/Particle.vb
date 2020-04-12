Imports ParticleToy.Behaviour

Public Class Particle

    Public Shared ReadOnly ElectricAngles As Double() = {0, 45, 90, 135, 180, 225, 270, 315}

    Friend MyIndex As Integer
    Friend Parent As Game
    Friend Ancs As Anchors
    Friend CurrentPosition As PointF = Nothing
    Friend LastPosition As PointF = Nothing
    Friend CurrentAngel As Double = 0
    Friend CurrentSpeed As Double = MIN_SPEED
    Friend CurrentColor As Pen
    Friend CurrentColorBru As SolidBrush
    Friend FontColor As Brush = Brushes.White
    Friend BlinkChar As Char = " "c
    Friend BlinkCharTimer As Integer = 0
    Public Shared Property Font As New Font("Courier New", 8)

    Public Color As Pen = Pens.White
    Public TargetAngel As Double = 0
    Public TargetSpeed As Double = MIN_SPEED
    Public Lightnight As PointF? = Nothing
    Public Glowing As Boolean = False
    'Public GlowingPen As New Pen(Drawing.Color.FromArgb(Glowing, 255, 255, 255), 1)

    Friend GolPos As Point
    Friend SilkPos As PointF
    Public Shared ReadOnly GolQuader As Integer = Math.Sqrt(Game.PARTICLE_NUMBER)
    Public GOL_MARGIN As Integer = 5
    Public Shared Gol_Matrix(GolQuader, GolQuader) As Particle
    Public GolMP As Point

    Private Const SILK_GRID_WIDTH As Integer = 122
    Private Const SILK_GRID_HEIGHT As Integer = 82
    Private Const SILK_GRID_CELL_WIDTH As Double = Game.OPT_SIZE_W / (SILK_GRID_WIDTH + 1)
    Private Const SILK_GRID_CELL_HEIGHT As Double = Game.OPT_SIZE_H / (SILK_GRID_HEIGHT + 1)

    Public Sub New(Ancs As Anchors, Position As PointF, Index As Integer, Parent As Game)
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
        If True Then
            Dim X As Integer = MyIndex Mod GolQuader
            Dim Y As Integer = (MyIndex - X) / GolQuader
            GolPos = New Point((X * GOL_MARGIN) + ((Game.OPT_SIZE_W - (GolQuader * GOL_MARGIN)) / 2),
                           (Y * GOL_MARGIN) + ((Game.OPT_SIZE_H - (GolQuader * GOL_MARGIN)) / 2))
            GolMP = New Point(X, Y)
            If Gol_Matrix(X, Y) Is Nothing Then
                Gol_Matrix(X, Y) = Me
            End If
        End If

        'Calculate Silk Position
        If True Then
            Dim X As Double = MyIndex Mod SILK_GRID_WIDTH
            Dim Y As Double = (MyIndex - X) / SILK_GRID_WIDTH
            SilkPos = New PointF((X * SILK_GRID_CELL_WIDTH) + SILK_GRID_CELL_WIDTH, (Y * SILK_GRID_CELL_HEIGHT) + SILK_GRID_CELL_HEIGHT)
        End If
    End Sub

    Public Shared AnchorCenter As PointF? = Nothing
    Public Shared SortedAnchors As New List(Of PointF)
    Public WillGlow As Boolean = False
    Friend AnchorIndex As Integer = -1
    Friend AnchorStep As Integer = 0
    Friend SpeedIsSet As Boolean = False
    Friend IsElectric As Boolean = False
    Friend IsAStar As Boolean = False
    Friend Partner As Particle = Nothing
    Friend MirrorPoint As PointF? = Nothing
    Friend MinMovementDrawLength As Double = 0
    Friend GolStatusSet As Boolean = False
    Friend GolPositionFound As Boolean = False
    Friend FoundAnt As Boolean = False
    Friend DrawLineDelta As Double = 2

    Public Sub Update(Game As GameBase, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Microsoft.VisualBasic.Devices.Keyboard, OnBehaviour As IBehaviour, Behaviour As IBehaviour, OffBehaviour As IBehaviour, OnColorManager As IColorManager, ColorManager As IColorManager)
        'Normalisierung
        '#######################################################################################
        Lightnight = Nothing

        BlinkCharTimer -= 1
        If BlinkCharTimer < 0 Then
            BlinkChar = " "c
        End If

        If OffBehaviour IsNot Nothing Then
            OffBehaviour.TurnedOff(Me, Game, Tick, MouseInfo, Keyboard)
            CurrentColor = Color
            TargetSpeed = MIN_SPEED
        End If

        If OnBehaviour IsNot Nothing Then
            OnBehaviour.TurnedOn(Me, Game, Tick, MouseInfo, Keyboard)
        End If

        'Behaviour.Normalize(Me, Game, Tick, MouseInfo, Keyboard)

        'Verhalten
        '#######################################################################################
        If Not Behaviour.Behave(Me, Game, Tick, MouseInfo, Keyboard) Then Exit Sub

        'Color Management
        '#######################################################################################
        If OnColorManager IsNot Nothing Then _
            ManageColor(OnColorManager, Game, Tick, MouseInfo, Keyboard, True)
        ManageColor(ColorManager, Game, Tick, MouseInfo, Keyboard, False)


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
        'If CurrentSpeed > 0 Then
        Dim NewPos As PointF = DegreesToXY(CurrentAngel, CurrentSpeed, CurrentPosition)
        MoveTo(NewPos)
        'End If

        DrawLineDelta = DeltaBetweed(LastPosition, CurrentPosition)

        'Positions Korrectur
        '#######################################################################################
        If CurrentPosition.X >= Game.ScreenSize.Width Then
            CurrentPosition = New PointF(0, CurrentPosition.Y)
            LastPosition = CurrentPosition
            Lightnight = Nothing
        End If
        If CurrentPosition.X < 0 Then
            CurrentPosition = New PointF(Game.ScreenSize.Width - 1, CurrentPosition.Y)
            LastPosition = CurrentPosition
            Lightnight = Nothing
        End If
        If CurrentPosition.Y >= Game.ScreenSize.Height Then
            CurrentPosition = New PointF(CurrentPosition.X, 0)
            LastPosition = CurrentPosition
            Lightnight = Nothing
        End If
        If CurrentPosition.Y < 0 Then
            CurrentPosition = New PointF(CurrentPosition.X, Game.ScreenSize.Height - 1)
            LastPosition = CurrentPosition
            Lightnight = Nothing
        End If
    End Sub

    Private Sub ManageColor(ColorManager As IColorManager, Game As GameBase, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Microsoft.VisualBasic.Devices.Keyboard, IsTurnOn As Boolean)
        Dim ManagedColor As Color? = Nothing
        If ColorManager IsNot Nothing Then
            If IsTurnOn Then
                ManagedColor = ColorManager.TurnOn(Me, Game, Tick, MouseInfo, Keyboard)
            Else
                ManagedColor = ColorManager.Manage(Me, Game, Tick, MouseInfo, Keyboard)
            End If
            If ManagedColor IsNot Nothing Then
                Select Case ColorManager.Mode
                    Case IColorManager.Modes.Additive
                        Dim a As Integer = Math.Floor((CurrentColor.Color.A + ManagedColor.Value.A) / 2)
                        Dim r As Integer = Math.Floor((CurrentColor.Color.R + ManagedColor.Value.R) / 2)
                        Dim g As Integer = Math.Floor((CurrentColor.Color.G + ManagedColor.Value.G) / 2)
                        Dim b As Integer = Math.Floor((CurrentColor.Color.B + ManagedColor.Value.B) / 2)
                        CurrentColor = New Pen(Drawing.Color.FromArgb(a, r, g, b), CurrentColor.Width)

                    Case IColorManager.Modes.AND
                        Dim a As Integer = CurrentColor.Color.A And ManagedColor.Value.A
                        Dim r As Integer = CurrentColor.Color.R And ManagedColor.Value.R
                        Dim g As Integer = CurrentColor.Color.G And ManagedColor.Value.G
                        Dim b As Integer = CurrentColor.Color.B And ManagedColor.Value.B
                        CurrentColor = New Pen(Drawing.Color.FromArgb(a, r, g, b), CurrentColor.Width)

                    Case IColorManager.Modes.Multiply
                        Dim a As Integer = Math.Floor(CurrentColor.Color.A * (ManagedColor.Value.A / 255))
                        Dim r As Integer = Math.Floor(CurrentColor.Color.R * (ManagedColor.Value.R / 255))
                        Dim g As Integer = Math.Floor(CurrentColor.Color.G * (ManagedColor.Value.G / 255))
                        Dim b As Integer = Math.Floor(CurrentColor.Color.B * (ManagedColor.Value.B / 255))
                        CurrentColor = New Pen(Drawing.Color.FromArgb(a, r, g, b), CurrentColor.Width)

                    Case IColorManager.Modes.OR
                        Dim a As Integer = CurrentColor.Color.A Or ManagedColor.Value.A
                        Dim r As Integer = CurrentColor.Color.R Or ManagedColor.Value.R
                        Dim g As Integer = CurrentColor.Color.G Or ManagedColor.Value.G
                        Dim b As Integer = CurrentColor.Color.B Or ManagedColor.Value.B
                        CurrentColor = New Pen(Drawing.Color.FromArgb(a, r, g, b), CurrentColor.Width)

                    Case IColorManager.Modes.Replace
                        CurrentColor = New Pen(ManagedColor.Value, CurrentColor.Width)

                    Case IColorManager.Modes.Subtractive
                        Dim a As Integer = Math.Floor(CurrentColor.Color.A * (1 - (ManagedColor.Value.A / 255)))
                        Dim r As Integer = Math.Floor(CurrentColor.Color.R * (1 - (ManagedColor.Value.R / 255)))
                        Dim g As Integer = Math.Floor(CurrentColor.Color.G * (1 - (ManagedColor.Value.G / 255)))
                        Dim b As Integer = Math.Floor(CurrentColor.Color.B * (1 - (ManagedColor.Value.B / 255)))
                        CurrentColor = New Pen(Drawing.Color.FromArgb(a, r, g, b), CurrentColor.Width)

                    Case Else
                        'Nothing to do

                End Select
            End If
        End If
    End Sub

    Friend Shared LightningPen As New Pen(Drawing.Color.FromArgb(64, 255, 255, 255), 1)
    Public Sub Draw(G As Graphics)
        If Lightnight IsNot Nothing Then
            G.DrawLine(LightningPen, CurrentPosition, Lightnight.Value)
        End If
        If DrawLineDelta >= MinMovementDrawLength Then
            If DrawLineDelta = 0 Then
                G.FillRectangle(CurrentColorBru, CurrentPosition.X, CurrentPosition.Y, 1, 1)
            Else
                G.DrawLine(CurrentColor, LastPosition, CurrentPosition)
            End If
            '    If Not HideNoMovement Then
            '        'G.FillRectangle(CurrentColorBru, CurrentPosition.X, CurrentPosition.Y, 1, 1)
            '    End If
            'Else
            '    G.DrawLine(CurrentColor, LastPosition, CurrentPosition)
        Else
            Dim i = 0
        End If
        If Glowing Then
            G.DrawRectangle(Pens.White, CurrentPosition.X - 1, CurrentPosition.Y - 1, 2, 2)
        End If
        If BlinkChar <> " "c Then
            G.DrawString(BlinkChar, Font, FontColor, Me.CurrentPosition.X, Me.CurrentPosition.Y)
        End If
    End Sub

    Public Sub MoveTo(NewPosition As PointF?)
        LastPosition = CurrentPosition
        CurrentPosition = NewPosition
    End Sub

    Public Shared Sub SortAnchors(Anc As Anchors)
        SortedAnchors.AddRange(Anc.Anchors.ToArray)
        Dim Center As PointF = CenterOfPoints(Anc.Anchors.ToArray)
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

