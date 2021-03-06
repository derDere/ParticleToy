﻿Imports Microsoft.VisualBasic.Devices
Imports ParticleToy
Imports ParticleToy.Behaviour

Public Class cStarsBh
    Implements IBehaviour

    Public ReadOnly Property IsSelected As String Implements IBehaviour.IsSelected
        Get
            Return Game.BehaviourKey = Key
        End Get
    End Property

    Public ReadOnly Property IsUnlocked As Boolean Implements IBehaviour.IsUnlocked
        Get
            Return Config.Unlocked.Contains(Key)
        End Get
    End Property

    Public ReadOnly Property Key As String Implements IBehaviour.Key
        Get
            Return "10"
        End Get
    End Property

    Public ReadOnly Property Icon As Bitmap Implements IBehaviour.Icon
        Get
            Return My.Resources.anc_10
        End Get
    End Property

    Public ReadOnly Property Name As String Implements IBehaviour.Name
        Get
            Return "Stars"
        End Get
    End Property

    Public ReadOnly Property ColorManager As IColorManager Implements IBehaviour.ColorManager
        Get
            Return Nothing
        End Get
    End Property

    Public ReadOnly Property OverwriteColorManager As Boolean Implements IBehaviour.OverwriteColorManager
        Get
            Return False
        End Get
    End Property

    Public Sub TurnedOff(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) Implements IBehaviour.TurnedOff
    End Sub

    'Public Sub Normalize(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) Implements IBehaviour.Normalize
    'End Sub

    Public Sub TurnedOn(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) Implements IBehaviour.TurnedOn
        Particle.TargetSpeed = MIN_SPEED + (RND.Next(1000) Mod 2)
        Particle.TargetAngel = 0
    End Sub

    Public Function Behave(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Keyboard) As Boolean Implements IBehaviour.Behave
        With Particle
            Dim MouseDelta As Double = 10000000
            If MouseInfo.Position IsNot Nothing Then _
                MouseDelta = DeltaBetweed(.CurrentPosition, MouseInfo.Position)
            Dim ColorPart As Integer = -1
            For Each Anc In .Ancs.Anchors
                Dim AncDelta As Double = DeltaBetweed(.CurrentPosition, Anc)
                If AncDelta < MouseDelta Then
                    MouseDelta = AncDelta
                    ColorPart = .Ancs.Anchors.IndexOf(Anc) Mod 3
                End If
            Next
            Dim RedLevel As Integer = 0
            If MouseDelta < BOUNCE_RADIUS Then
                RedLevel = (BOUNCE_RADIUS - MouseDelta) * 3
            End If
            Dim ColorVal As Integer = 255 - ((2 - .TargetSpeed) * 100)
            Select Case ColorPart
                Case 2
                    .CurrentColor = Color.FromArgb(255, ColorVal - RedLevel, ColorVal - RedLevel, ColorVal).ToPen
                Case 1
                    .CurrentColor = Color.FromArgb(255, ColorVal - RedLevel, ColorVal, ColorVal - RedLevel).ToPen
                Case 0
                    .CurrentColor = Color.FromArgb(255, ColorVal, ColorVal - RedLevel, ColorVal - RedLevel).ToPen
                Case Else
                    .CurrentColor = Color.FromArgb(255, ColorVal - (RedLevel * 0.3333), ColorVal - RedLevel, ColorVal).ToPen
            End Select
        End With
        Return True
    End Function

End Class
