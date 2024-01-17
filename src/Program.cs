using Melanchall.DryWetMidi;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Multimedia;
using System.Timers;
using Melanchall.DryWetMidi.Interaction;

InputDevice device = InputDevice.GetByName("CASIO USB-MIDI");
Recording recording = new Recording(TempoMap.Default,device);

device.StartEventsListening();
recording.Start();

Console.WriteLine("Listening for MIDI Events... press ENTER to save and exit the program");
Console.ReadLine();

recording.Stop();

MidiFile outFile = recording.ToFile();
outFile.Write("output/out.mid",true);

Console.WriteLine("Saved!");