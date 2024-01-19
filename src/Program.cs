using Melanchall.DryWetMidi;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Multimedia;
using System.Timers;
using Melanchall.DryWetMidi.Interaction;

InputDevice device = InputDevice.GetByName("CASIO USB-MIDI");
RecordingManager manager = new RecordingManager(device);

manager.StartRecording();

Console.WriteLine("Listening for MIDI Events... press ENTER to save and exit the program");
Console.ReadLine();

manager.StopRecording(true);

Console.WriteLine("Saved!");