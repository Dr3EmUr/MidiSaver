using Melanchall.DryWetMidi;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Multimedia;
using System.Timers;

public class MidiListener
{
    MidiFile file = new MidiFile();
    InputDevice device;
    public MidiListener(InputDevice inputDevice)
    {
        this.device = inputDevice;
    }

    public void Start()
    {
        long deltaTime = 0;
        System.Timers.Timer timer = new System.Timers.Timer(1);
        timer.Elapsed += (object? sender, ElapsedEventArgs e) => 
        {
            deltaTime += 10;
        };

        device.EventReceived += HandleMidiInput;
        device.StartEventsListening();

        timer.Start();

        void HandleMidiInput(object? sender, MidiEventReceivedEventArgs e)
        {          
            e.Event.DeltaTime = deltaTime;
            deltaTime = 0;

            TrackChunk chunk = new TrackChunk(e.Event);
            file.Chunks.Add(chunk);

            Console.Write(e.Event);
        }
    }

    public void WriteResults(string outputPath)
    {
        File.Delete(outputPath);
        file.Write(outputPath,true,MidiFileFormat.SingleTrack);
    }
}