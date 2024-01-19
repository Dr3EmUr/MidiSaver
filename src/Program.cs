using Melanchall.DryWetMidi;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Multimedia;
using System.Timers;
using Melanchall.DryWetMidi.Interaction;

InputDevice device = InputDevice.GetByName("CASIO USB-MIDI");
RecordingManager manager = new RecordingManager(device);

// I commented this since the RecordingManager starts recording on MIDI input anyway, 
// so there's no point in starting it manually here
//
// manager.StartRecording();

Console.WriteLine("Listening for MIDI Events... press ENTER to save and exit the program");
Console.ReadLine();

manager.StopRecording(true);

Console.WriteLine("Saved!");