Imports System.Windows

Public Class Controls

    Private WithEvents ColorPickerDialog As New System.Windows.Forms.ColorDialog With {
        .FullOpen = True,
        .AllowFullOpen = True,
        .AnyColor = True,
        .Color = Color.CornflowerBlue
    }

    Public Event ModeSelect(mode As String)

    Public Event ColorSelect(color As IColorManager)

    Public Shared ReadOnly Property Behaviours As New Dictionary(Of String, IBehaviour)
    Public Shared ReadOnly Property ColorManagers As New Dictionary(Of String, IColorManager)

    Private MySimpleColMan As SimpleColorManager

    Public Property CMs As IColorManager()
        Get
            Dim keys As New List(Of String)
            For Each k As String In ColorManagers.Keys
                If k.Length <= 1 Then
                    keys.Add("0" & k)
                Else
                    keys.Add(k)
                End If
            Next
            keys.Sort()
            Dim cmL As New List(Of IColorManager)
            For Each k As String In keys
                If k.Length = 2 AndAlso k.StartsWith("0") Then
                    cmL.Add(ColorManagers(k.Substring(1)))
                Else
                    cmL.Add(ColorManagers(k))
                End If
            Next
            Return cmL.ToArray
        End Get
        Set(value As IColorManager())
        End Set
    End Property

    Public Property BHs As IBehaviour()
        Get
            Dim keys As New List(Of String)
            For Each k As String In Behaviours.Keys
                If k.Length <= 1 Then
                    keys.Add("0" & k)
                Else
                    keys.Add(k)
                End If
            Next
            keys.Sort()
            Dim bhL As New List(Of IBehaviour)
            For Each k As String In keys
                If k.Length = 2 AndAlso k.StartsWith("0") Then
                    bhL.Add(Behaviours(k.Substring(1)))
                Else
                    bhL.Add(Behaviours(k))
                End If
            Next
            Return bhL.ToArray
        End Get
        Set(value As IBehaviour())
        End Set
    End Property

    Public Sub New()
        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
        Dim AllBehaviours As New List(Of IBehaviour)
        Dim AllColorManagers As New List(Of IColorManager)

        For Each ItmType As Type In Reflection.Assembly.GetExecutingAssembly.GetTypes
            If ItmType.GetInterfaces.Any(Function(I) I.Name = "IBehaviour") Then
                Dim IBObj As IBehaviour = Activator.CreateInstance(ItmType)
                Behaviours.Add(IBObj.Key, IBObj)
            End If
            If ItmType.GetInterfaces.Any(Function(I) I.Name = "IColorManager") Then
                Dim ICmObj As IColorManager = Activator.CreateInstance(ItmType)
                If TypeOf ICmObj Is SimpleColorManager Then
                    MySimpleColMan = ICmObj
                    MySimpleColMan.Mode = IColorManager.Modes.Replace
                    MySimpleColMan.RefreshMode = True
                End If
                ColorManagers.Add(ICmObj.Key, ICmObj)
            End If
        Next

        BehaveIC.GetBindingExpression(DataContextProperty).UpdateTarget()
        ColorIC.GetBindingExpression(DataContextProperty).UpdateTarget()
    End Sub

    Private Sub ColorTab_Click(sender As Object, e As RoutedEventArgs) Handles ColorTab.Click
        BehaveTab.Tag = Nothing
        ColorTab.Tag = "True"
        BehaveSV.Visibility = Visibility.Collapsed
        ColorSV.Visibility = Visibility.Visible
    End Sub

    Private Sub BehaveTab_Click(sender As Object, e As RoutedEventArgs) Handles BehaveTab.Click
        BehaveTab.Tag = "True"
        ColorTab.Tag = Nothing
        BehaveSV.Visibility = Visibility.Visible
        ColorSV.Visibility = Visibility.Collapsed
    End Sub

    Private Sub BehaveButton_Click(sender As Object, e As RoutedEventArgs)
        If TypeOf sender Is System.Windows.Controls.Button Then
            Dim btn As System.Windows.Controls.Button = sender
            If TypeOf btn.DataContext Is IBehaviour Then
                Dim behv As IBehaviour = btn.DataContext
                RaiseEvent ModeSelect(behv.Key)
            End If
        End If
    End Sub

    Private Sub ColorButton_Click(sender As Object, e As RoutedEventArgs)
        If TypeOf sender Is System.Windows.Controls.Button Then
            Dim btn As System.Windows.Controls.Button = sender
            If TypeOf btn.DataContext Is IColorManager Then
                Dim colman As IColorManager = btn.DataContext
                RaiseEvent ColorSelect(colman)
                If colman Is MySimpleColMan Then MySimpleColMan.Refresh = True
            End If
        End If
    End Sub

    Private Sub ColorButton_MouseUp(sender As Object, e As Input.MouseButtonEventArgs)
        If e.ChangedButton = Input.MouseButton.Right Then
            If TypeOf sender Is System.Windows.Controls.Button Then
                Dim btn As System.Windows.Controls.Button = sender
                If TypeOf btn.DataContext Is SimpleColorManager Then
                    ColorConMen.IsOpen = True
                End If
            End If
        End If
    End Sub

    Private Sub RndColSlider_ValueChanged(sender As Object, e As RoutedPropertyChangedEventArgs(Of Double)) Handles RndColSlider.ValueChanged
        SliderValueRun.Text = Math.Round(RndColSlider.Value)
        MySimpleColMan.RandomizeValue = Math.Round(RndColSlider.Value)
        MySimpleColMan.Refresh = True
    End Sub

    Private Sub ChangeColorMI_Click(sender As Object, e As RoutedEventArgs) Handles ChangeColorMI.Click
        ColorPickerDialog.Color = MySimpleColMan.Color
        If ColorPickerDialog.ShowDialog = DialogResult.OK Then
            MySimpleColMan.Color = ColorPickerDialog.Color
            MySimpleColMan.Refresh = True
        End If
    End Sub

    Private Sub ModeMI_Click(sender As Object, e As RoutedEventArgs) Handles ModeMI_Replace.Click,
                                                                             ModeMI_And.Click,
                                                                             ModeMI_Or.Click,
                                                                             ModeMI_Xor.Click,
                                                                             ModeMI_Multiply.Click,
                                                                             ModeMI_Additive.Click,
                                                                             ModeMI_Subtractive.Click
        For Each MI As System.Windows.Controls.MenuItem In {ModeMI_Replace, ModeMI_And, ModeMI_Xor, ModeMI_Or, ModeMI_Multiply, ModeMI_Additive, ModeMI_Subtractive}
            MI.IsChecked = MI Is sender
            If MI Is sender Then
                ModeRun.Text = MI.Header
                Dim mode As IColorManager.Modes = [Enum].Parse(GetType(IColorManager.Modes), MI.Header)
                MySimpleColMan.Mode = mode
            End If
        Next
    End Sub

End Class
