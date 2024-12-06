using System.Text.Json;
using Godot;

public class PlayerData
{
    /*
        Represents a player's data
        Contains all necessary info on the player
    */

    public int id { get; private set; } //player slot (0 -> 3)
    public bool ai { get; private set; } //false if real player

    public int characterID { get; private set; } //character id (0 = jario)
    public int coins { get; private set; } //coin count
    public int stars { get; private set; } //star count

    public PlayerData(int _id, bool _ai, int _characterID)
    {
        id = _id;
        ai = _ai; //not jario party using ai :pensive:

        characterID = _characterID;
    }

    public void load(ref Utf8JsonReader reader)
    {
        while(reader.Read())
        {
            switch(reader.TokenType)
            {
                case JsonTokenType.PropertyName:
                    string property = reader.GetString();
                    reader.Read();
                    
                    switch(property)
                    {
                        case "id": id = reader.GetInt32(); break;
                        case "ai": ai = reader.GetBoolean(); break;
                        case "character_id": characterID = reader.GetInt32(); break;
                        case "coins": coins = reader.GetInt32(); break;
                        case "stars": stars = reader.GetInt32(); break;
                    }
                break;

                case JsonTokenType.EndObject: return;
                default: break;
            }
        }
    }

    public void save(Utf8JsonWriter writer, int index)
    {
        //index passed in from player manager. just used for file saving, as the id is saved/read from disk
        writer.WriteStartObject("player_" + index);

        writer.WriteNumber("id", id);
        writer.WriteBoolean("ai", ai);
        writer.WriteNumber("character_id", characterID);
        writer.WriteNumber("coins", coins);
        writer.WriteNumber("stars", stars);

        writer.WriteEndObject();
    }

    public override string ToString()
    {
        return "PlayerData { id = " + id + ", characterID = " + characterID + ", coins = " + coins + ", stars = " + stars + " }"; 
    }
}