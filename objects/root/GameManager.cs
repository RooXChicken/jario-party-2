using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Godot;

public class GameManager
{
    /*
        Used for managing the state of the game
        Mainly saving/loading
    */

    public PlayerManager playerManager { get; private set; }
    public GameData gameData { get; private set; }

    public GameManager()
    {
        //initialize data
        playerManager = new PlayerManager(0, new List<PlayerData>());
        gameData = new GameData(0, 0);
    }

    public void initializeGame(int _numPlayers, List<PlayerData> _playerData, GameData _gameData)
    {
        //load the game from the specified paramaters (to be used after game setup with frog)
        playerManager = new PlayerManager(_numPlayers, _playerData);
        gameData = _gameData;
    }

    public void loadGame(string path)
    {
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

    public void saveGame(string path)
    {
        //open the file (writer)
        using FileStream fs = File.OpenWrite(path);
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
        fs.Flush();

        writer.Dispose();
        fs.Close();
    }
}