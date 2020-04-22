Public Interface IColorManager
    Enum Modes
        None = False
        Replace
        [And]
        [Or]
        [Xor]
        Multiply
        Additive
        Subtractive
    End Enum

    ReadOnly Property Key As String

    ReadOnly Property Icon As Bitmap

    ReadOnly Property Name As String

    ReadOnly Property DefaultMode As Modes

    Property Mode As Modes

    'ReadOnly Property IsSelected As Boolean

    'ReadOnly Property IsUnlocked As Boolean

    Function TurnOn(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Microsoft.VisualBasic.Devices.Keyboard) As Color?

    Function Manage(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Microsoft.VisualBasic.Devices.Keyboard) As Color?

End Interface
