using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Godot;

public class GameManager
{
    public PlayerManager playerManager { get; private set; }
    public GameData gameData { get; private set; }

    public GameManager()
    {
        playerManager = new PlayerManager(0, new List<PlayerData>());
        gameData = new GameData(0, 0);
    }

    public void initializeGame(int _numPlayers, List<PlayerData> _playerData, GameData _gameData)
    {
        playerManager = new PlayerManager(_numPlayers, _playerData);
        gameData = _gameData;
    }

    public void loadGame(string path)
    {
        byte[] fs = File.ReadAllBytes(path);
        Utf8JsonReader reader = new Utf8JsonReader(fs);

        while(reader.Read())
        {
            switch(reader.TokenType)
            {
                case JsonTokenType.PropertyName:
                    switch(reader.GetString())
                    {
                        case "game": gameData.load(ref reader); break;
                        case "player_data": playerManager.load(ref reader); break;
                    }
                break;

                default: break;
            }
        }
    }

    public void saveGame(string path)
    {
        using FileStream fs = File.OpenWrite(path);
        using var writer = new Utf8JsonWriter(fs, new JsonWriterOptions { Indented = true });

        writer.WriteStartObject();

        gameData.save(writer);
        playerManager.save(writer);

        writer.WriteEndObject();

        writer.Flush();
        fs.Flush();

        writer.Dispose();
        fs.Close();
    }
}