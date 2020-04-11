Public Interface IColorManager
    Enum Modes
        None = False
        Replace
        [AND]
        [OR]
        Multiply
        Additive
        Subtractive
    End Enum

    ReadOnly Property Key As String

    ReadOnly Property Icon As Bitmap

    ReadOnly Property Name As String

    ReadOnly Property DefaultMode As Modes

    Property Mode As Modes

    Function TurnOn(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Microsoft.VisualBasic.Devices.Keyboard) As Color?

    Function Manage(Particle As Particle, Game As Game, Tick As Integer, MouseInfo As MouseInfo, Keyboard As Microsoft.VisualBasic.Devices.Keyboard) As Color?

End Interface
