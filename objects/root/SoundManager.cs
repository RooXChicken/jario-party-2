using System.Collections.Generic;
using Godot;

class SoundEffect
{
    /*
		Stores sound effect information
		Used exclusively in the SoundManager class

        Load sound effects with SoundManager.loadSound()
	*/

    public string name { get; private set; }
    public AudioStream stream { get; private set; }

    public float volume { get; private set; }

    public SoundEffect(string _name, string _path, float _volume)
    {
        name = _name;
        stream = GD.Load<AudioStream>(_path);

        volume = _volume;
    }

    //frees the audio stream (and memory)
    public void destroy() { stream.Free(); }
}

public partial class SoundManager : Node
{
    /*
		Generic Sound Player
		Used for playing any generic sound effect (not music)

		Sound effects are cached to prevent unnecessary loading
        Because of this, call loadSound() as much as needed! There is no downside

        There is an artificial limit of 64 sounds.
        This can be changed as is needed (64 should be plenty though)

        Sound unloading is possible, but considering the small size of sound effects,
        it's generally okay to not unload them (saves reloading too!)
	*/

    private static readonly short MAX_SOUNDS = 64;
    private static Dictionary<string, SoundEffect> loadedStreams;
    private static AudioStreamPlayer2D[] soundSlots;

    public SoundManager()
    {
        //initialize sound streams
        loadedStreams = new Dictionary<string, SoundEffect>();
        soundSlots = new AudioStreamPlayer2D[MAX_SOUNDS];

        //create all sound slots
        for(int i = 0; i < MAX_SOUNDS; i++)
        {
            soundSlots[i] = new AudioStreamPlayer2D();
            soundSlots[i].Bus = "Sound";
            AddChild(soundSlots[i]);
        }
    }

    public static void loadSound(string _name, string _path, float _volume)
    {
        if(loadedStreams.ContainsKey(_name))
            return;

        //add sound to dictionary
        loadedStreams.Add(_name, new SoundEffect(_name, _path, _volume));
    }

    public static void loadSound(string _name)
    {
        if(!loadedStreams.ContainsKey(_name))
            return;

        //unload and unregister sound
        loadedStreams[_name].destroy();
        loadedStreams.Remove(_name);
    }

    public static void playSound(string name)
    {
        if(!loadedStreams.ContainsKey(name))
        {
            GD.PrintErr("Failed to play sound " + name + "! It is not loaded!");
            return;
        }

        //set stream and play sound
        int soundSlot = getOpenSlot();
        soundSlots[soundSlot].Stream = loadedStreams[name].stream;
        soundSlots[soundSlot].VolumeDb = loadedStreams[name].volume;
        soundSlots[soundSlot].Play();
    }

    private static int getOpenSlot()
    {
        //loop over the array of sounds to find one that is playing
        for(int i = 0; i < MAX_SOUNDS; i++)
            if(!soundSlots[i].Playing) return i;

        //override sound 0 if all slots are full (and log it to console)
        GD.Print("All sound slots are full! Override sound slot 0");
        return 0;
    }
}