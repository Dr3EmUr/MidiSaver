using Melanchall.DryWetMidi;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Multimedia;
using System.Timers;
using Melanchall.DryWetMidi.Interaction;

var connectedDevices = InputDevice.GetAll().ToList();

List<RecordingManager> managers = new List<RecordingManager>();
foreach(var device in connectedDevices)
{
    Console.WriteLine(device.Name + " Was found!");
    managers.Add(new RecordingManager(device));
}

Console.WriteLine("Listening for MIDI Events... press ENTER to save and exit the program");
Console.ReadLine();

foreach(var manager in managers)
{
    manager.StopRecording(true);
}

Console.WriteLine("Saved!");