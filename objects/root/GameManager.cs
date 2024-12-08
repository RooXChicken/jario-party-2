using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Godot;

public partial class GameManager : Node2D
{
	/*
		Used for managing the state of the game
		Mainly saving/loading
	*/

	public static PlayerManager playerManager { get; private set; }
	public static GameData gameData { get; private set; }
	public static SettingsManager settingsManager { get; private set; }

	private Node2D optionsMenu;

	public GameManager()
	{
		//initialize data
		settingsManager = new SettingsManager(this);
		playerManager = new PlayerManager(0, new List<PlayerData>());
		gameData = new GameData(0, 0);
	}

	public override void _EnterTree()
	{
		settingsManager.fullscreenToggle(settingsManager.fullscreen, false);

		optionsMenu = (Node2D)((PackedScene)ResourceLoader.Load("res://objects/generic/gui/options_menu.tscn")).Instantiate();
		AddChild(optionsMenu);
	}

	public override void _Process(double delta)
	{
		if(Input.IsActionJustPressed("options"))
			optionsMenu.GetNode<CanvasLayer>("Layer").Visible = !optionsMenu.GetNode<CanvasLayer>("Layer").Visible;
	}

	public static void initializeGame(int _numPlayers, List<PlayerData> _playerData, GameData _gameData)
	{
		//load the game from the specified paramaters (to be used after game setup with frog)
		playerManager = new PlayerManager(_numPlayers, _playerData);
		gameData = _gameData;
	}

	public static void loadGame(string path)
	{
		//check if file exists
		if(!File.Exists(path))
		{
			GD.PrintErr("Failed to load game with path: " + path + " because it *does not exist*!");
			return;
		}

		//load the file
		byte[] fs = File.ReadAllBytes(path);
		Utf8JsonReader reader = new Utf8JsonReader(fs);

		//read from json
		while(reader.Read()) //returns false when there is no data left
		{
			switch(reader.TokenType)
			{
				case JsonTokenType.PropertyName:
					switch(reader.GetString())
					{
						//passed in as a reference because c# was passing the reader as a clone (for some reason)
						case "game_data": gameData.load(ref reader); break;
						case "player_data": playerManager.load(ref reader); break;
					}
				break;

				default: break;
			}
		}
	}

	public static void saveGame(string path)
	{
		//open the file (writer)
		using MemoryStream fs = new MemoryStream();
		using var writer = new Utf8JsonWriter(fs, new JsonWriterOptions { Indented = true });

		//begin root object
		writer.WriteStartObject();

		//save data
		gameData.save(writer);
		playerManager.save(writer);

		//end root object
		writer.WriteEndObject();

		//flush and close file
		writer.Flush();
		File.WriteAllText(path, writer.ToString());

		writer.Dispose();
		fs.Close();
	}
}
