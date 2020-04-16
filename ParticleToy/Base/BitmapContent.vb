Imports System.IO
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Media.Imaging

Public Class BitmapContent
    Inherits ContentControl

    Public Shared ReadOnly ImageProperty As DependencyProperty = DependencyProperty.Register("Image", GetType(Bitmap), GetType(BitmapContent), New PropertyMetadata(Nothing, AddressOf ImageChanged))
    Public Property Image As Bitmap
        Get
            Return Nothing
        End Get
        Set(value As Bitmap)
            If value Is Nothing Then
                imgBox.Source = Nothing
            Else
                Dim MS As New MemoryStream()
                value.Save(MS, Drawing.Imaging.ImageFormat.Png)
                Dim Image As New BitmapImage
                Image.BeginInit()
                MS.Seek(0, SeekOrigin.Begin)
                Image.StreamSource = MS
                Image.EndInit()
                imgBox.Source = Image
            End If
        End Set
    End Property
    Private Shared Sub ImageChanged(dependencyObject As DependencyObject, e As DependencyPropertyChangedEventArgs)
        If TypeOf dependencyObject Is BitmapContent Then
            Dim bc As BitmapContent = dependencyObject
            bc.Image = e.NewValue
        End If
    End Sub

    Public Shared ReadOnly IsRightClickIconProperty As DependencyProperty = DependencyProperty.Register("IsRightClickIcon", GetType(String), GetType(BitmapContent), New PropertyMetadata(Nothing, AddressOf IsRightClickIconChanged))
    Public Property IsRightClickIcon As String
        Get
            Return False
        End Get
        Set(value As String)
            If value = "Simple Color" Then
                Image = My.Resources.right_click
                Me.Visibility = Visibility.Visible
            End If
        End Set
    End Property
    Private Shared Sub IsRightClickIconChanged(dependencyObject As DependencyObject, e As DependencyPropertyChangedEventArgs)
        If TypeOf dependencyObject Is BitmapContent Then
            Dim bc As BitmapContent = dependencyObject
            bc.IsRightClickIcon = e.NewValue
        End If
    End Sub

    Private imgBox As New Image

    Public Sub New()
        Me.Background = Media.Brushes.Red
        Me.Content = imgBox
        Me.Effect = New Media.Effects.DropShadowEffect() With {
            .BlurRadius = 0,
            .Direction = -45,
            .Opacity = 112 / 255,
            .ShadowDepth = 3,
            .Color = Media.Color.FromArgb(255, 0, 148, 255)
        }
    End Sub

End Class
