using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Multimedia;
using Melanchall.DryWetMidi.Interaction;
using Timer = System.Timers.Timer;
using System.Timers;
using Melanchall.DryWetMidi.Tools;

public class RecordingManager
{
    
    // config options


    // the base output directory
    const string baseOutputPath = "output/";
    // the seconds that have to pass before generating another recording file
    const int millisecondsToFileSwitching = 5000; // for debug purpuses it's five, but it should be at least 10 IMO


    // Dependencies
    private InputDevice device;


    // Global variables
    readonly Timer timer;
    RecordingState state;
    Recording currentRecording;
    string currentPath;
    MidiEvent? additionalFirstMidiEvent;
    


    // Constructor
    public RecordingManager(InputDevice inputDevice)
    {
        this.device = inputDevice;

        timer = new Timer(millisecondsToFileSwitching);
        timer.Elapsed += onTimerElapsed;

        currentRecording = new Recording(TempoMap.Default,device);
        state = RecordingState.Stopped;

        device.EventReceived += onMidiInput;
        device.StartEventsListening();

        // default currentPath cause compiler is angry at me
        currentPath = getUpdatedRecordingPath();
    }


    // Public methods
    public void StartRecording()
    {   
        Console.WriteLine("Started recording");

        currentPath = getUpdatedRecordingPath();

        currentRecording.Stop();
        currentRecording = new Recording(TempoMap.Default,device);
        currentRecording.Start();

        timer.Start();
        state = RecordingState.Recording;
    }

    public void StopRecording(bool outputFile = false)
    {
        Console.WriteLine("Stopped recording");
        timer.Stop();
        
        currentRecording.Stop();
        state = RecordingState.Stopped;

        if (outputFile == true && !File.Exists(currentPath) && currentRecording.GetEvents().Count != 0)
        {
            MidiFile file;
            if (additionalFirstMidiEvent != null)
            {
                var firstTimedEvent = new TimedEvent(additionalFirstMidiEvent);
                firstTimedEvent.Time = 0;

                List<TimedEvent> events = [firstTimedEvent, .. currentRecording.GetEvents()];
                file = events.ToFile();
            }
            else
            {
                file = currentRecording.ToFile();
            }

            // for some reason, I have to set the time to 100 bpm first and then at 120 bpm, else it doesn't get recorded into the file.
            // The reason why I have to set it to 120 bpm is because that's the speed the MIDI is being recorded, apparently.

            var manager = new TempoMapManager();
            manager.SetTempo(0, new Tempo(600000));
            manager.SetTempo(0, new Tempo(500000));

            file.ReplaceTempoMap(manager.TempoMap);
            file.Write(currentPath);

            Console.WriteLine("Outputted file");
        }
            
    }

    // Private helper methods

    private void onMidiInput(object? sender, MidiEventReceivedEventArgs args)
    {
        Console.WriteLine("Midi input!");
        if (state == RecordingState.Recording)
        {
            timer.Stop();
            timer.Start();
        }
        else
        {
            additionalFirstMidiEvent = args.Event;
            StartRecording();
        }
    }
    private void onTimerElapsed(object? sender, ElapsedEventArgs args)
    {
        Console.WriteLine("Timer elapsed");
        StopRecording(true);
    }

    private string getUpdatedRecordingPath()
    {
        DateTime currentDateTime = DateTime.Now;
        string currentDateString = currentDateTime.Day + "_" + currentDateTime.Month + "_" + currentDateTime.Year;
        string currentHourString = currentDateTime.Hour + "-" + currentDateTime.Minute + "-" + currentDateTime.Second + "-" + currentDateTime.Microsecond;

        string dirPath = baseOutputPath + currentDateString;

        if(!Directory.Exists(dirPath)) 
            Directory.CreateDirectory(dirPath);

        string fileName = device.Name + "_" + currentHourString;
        return dirPath + "/" +  fileName + ".mid";
    }
}