using System.Collections.Generic;
using System.Text.Json;
using Godot;

public class PlayerManager
{
    public int numPlayers { get; private set; }
    public List<PlayerData> playerData;

    public PlayerManager(int _numPlayers, List<PlayerData> _playerData)
    {
        numPlayers = _numPlayers;
        playerData = _playerData;
    }

    public void load(ref Utf8JsonReader reader)
    {
        while(reader.Read())
        {
            switch(reader.TokenType)
            {
                case JsonTokenType.PropertyName:
                    string property = reader.GetString();
                    if(property == "count")
                    {
                        reader.Read();
                        numPlayers = reader.GetInt32();

                        for(int i = 0; i < numPlayers; i++)
                        {
                            PlayerData _data = new PlayerData(0, false, 0);
                            _data.load(ref reader);
                            playerData.Add(_data);
                        }
                    }
                break;

                case JsonTokenType.EndObject: return;
                default: break;
            }
        }
    }

    public void save(Utf8JsonWriter writer)
    {
        writer.WriteStartObject("player_data");

        writer.WriteNumber("count", numPlayers);
        for(int i = 0; i < numPlayers; i++)
            playerData[i].save(writer, i);

        writer.WriteEndObject();
    }

    public override string ToString()
    {
        string value = "PlayerManager: {\n";
        for(int i = 0; i < numPlayers; i++)
            value += playerData[i].ToString() + "\n";

        value += "}";

        return value;
    }
}