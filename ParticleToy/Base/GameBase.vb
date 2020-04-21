﻿Public MustInherit Class GameBase

    Protected _ScreenSize As New Size(800, 600)
    Public ReadOnly Property ScreenSize As Size
        Get
            Return _ScreenSize
        End Get
    End Property

    Public Sub New()
    End Sub

    Public MustOverride Sub Init()

    Public MustOverride Sub Update(Tick As Integer, MouseInfo As MouseInfo, KeyBoardInfo As KeyBoardInfo)

    Public MustOverride Sub Draw(G As Graphics)

End Class
