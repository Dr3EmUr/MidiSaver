using Melanchall.DryWetMidi;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Multimedia;
using System.Timers;
using Melanchall.DryWetMidi.Interaction;

public class RecordingManager
{
    
    // config options
    const string baseOutputPath = "output/out";
    // the seconds that have to pass before generating another recording file
    const int secondsToFileSwitching = 20;

    // Dependencies
    private InputDevice device;
    // Constructor
    public RecordingManager(InputDevice inputDevice)
    {
        this.device = inputDevice;
    }

    // Public methods
    public void StartRecording()
    {
        Recording recording = new Recording(TempoMap.Default,device);

        device.StartEventsListening();
        recording.Start();
    }

    public void StopRecording()
    {

    }

    // Private methods

    public void GetCurrentRecordingPath()
    {
        DateTime currentDateTime = DateTime.Now;
        string currentDate = currentDateTime.ToShortDateString();
        string currentTime = currentDateTime.ToShortTimeString();

        string dirPath = baseOutputPath + currentDate;
        Directory.CreateDirectory(dirPath);

        string counter = "0";
        if (File.Exists(dirPath + currentTime))
        {

        }
        
    }
}