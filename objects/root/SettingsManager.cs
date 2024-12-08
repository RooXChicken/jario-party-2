using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Godot;

public partial class SettingsManager : Node2D
{
	/*
		Used for managing game settings
		Resolution, volume, controls, etc
	*/

	//used to get a node that exists in the scene tree
	private Node2D gameManager;

	public int masterVolume = 0;
	public int musicVolume = 0;
	public int soundVolume = 0;

	public int resolution = 1;
	public bool fullscreen = false;

	public SettingsManager(Node2D _gameManager)
	{
		gameManager = _gameManager;

		//load settings
		loadSettings("options.cfg");
		masterVolumeChanged(masterVolume);
		musicVolumeChanged(musicVolume);
		soundVolumeChanged(soundVolume);
	}

	//called from the resolution dropdown in the video submenu
	public void resolutionChange(int index)
	{
		resolution = index;
		switch(resolution)
		{
			case 0: gameManager.GetWindow().Size = new Vector2I(640, 360); break;
			case 1: gameManager.GetWindow().Size = new Vector2I(1280, 720); break;
			case 2: gameManager.GetWindow().Size = new Vector2I(1920, 1080); break;
			case 3: gameManager.GetWindow().Size = new Vector2I(2560, 1440); break;
			case 4: gameManager.GetWindow().Size = new Vector2I(3840, 2160); break;
		}

		saveSettings("options.cfg");
	}

	//called from the fullscreen button in the video submenu
	public void fullscreenToggle(bool state)
	{
		fullscreen = state;
		
		//sets the window mode
		DisplayServer.WindowSetMode(fullscreen ? DisplayServer.WindowMode.Fullscreen : DisplayServer.WindowMode.Windowed);

		//call resolution change to set the resolution to what it was
		resolutionChange(resolution);

		saveSettings("options.cfg");
	}

	//called from the master audio slider
	public void masterVolumeChanged(int volume)
	{
		masterVolume = volume;
		AudioServer.SetBusVolumeDb(0, masterVolume);

		saveSettings("options.cfg");
	}

	//called from the music audio slider
	public void musicVolumeChanged(int volume)
	{
		musicVolume = volume;
		AudioServer.SetBusVolumeDb(1, musicVolume);

		saveSettings("options.cfg");
	}

	//called from the sound audio slider
	public void soundVolumeChanged(int volume)
	{
		soundVolume = volume;
		AudioServer.SetBusVolumeDb(2, soundVolume);
		
		saveSettings("options.cfg");
	}

	public void loadSettings(string path)
	{
		//create defaults if it doesn't exist
		if(!File.Exists(path))
			saveSettings(path);

		//load the file
		byte[] fs = File.ReadAllBytes(path);
		Utf8JsonReader reader = new Utf8JsonReader(fs);

		//read from json
		while(reader.Read()) //returns false when there is no data left
		{
			switch(reader.TokenType)
			{
				case JsonTokenType.PropertyName:
					string item = reader.GetString();
					reader.Read();

					switch(item)
					{
						case "master_volume": masterVolume = reader.GetInt16(); break;
						case "music_volume": musicVolume = reader.GetInt16(); break;
						case "sound_volume": soundVolume = reader.GetInt16(); break;

						case "resolution": resolution = reader.GetInt16(); break;
						case "fullscreen": fullscreen = reader.GetBoolean(); break;
					}
				break;

				default: break;
			}
		}
	}

	public void saveSettings(string path)
	{
		//open the file (writer)
		using FileStream fs = File.OpenWrite(path);
		using var writer = new Utf8JsonWriter(fs, new JsonWriterOptions { Indented = true });

		//begin root object
		writer.WriteStartObject();

		writer.WriteNumber("master_volume", masterVolume);
		writer.WriteNumber("music_volume", musicVolume);
		writer.WriteNumber("sound_volume", soundVolume);

		writer.WriteNumber("resolution", resolution);
		writer.WriteBoolean("fullscreen", fullscreen);

		//end root object
		writer.WriteEndObject();

		//flush and close file
		writer.Flush();
		fs.Flush();

		writer.Dispose();
		fs.Close();
	}
}
