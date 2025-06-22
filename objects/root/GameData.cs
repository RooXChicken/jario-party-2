using System.IO;
using System.Text.Json;
using Godot;

public class GameData
{
	/*
		Represents the master map data.
		Stores map id and turn information
	*/

	public int id { get; private set; }

	public int turnTotal { get; private set; }
	public int currentTurn { get; private set; }

	public GameData(int _id, int _turnTotal)
	{
		id = _id;
		turnTotal = _turnTotal;
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
						case "turn_total": turnTotal = reader.GetInt32(); break;
						case "current_turn": currentTurn = reader.GetInt32(); break;
					}
				break;

				case JsonTokenType.EndObject: return;
				default: break;
			}
		}
	}

	public void save(Utf8JsonWriter writer)
	{
		//start object game data
		writer.WriteStartObject("game_data");

		writer.WriteNumber("id", id);
		writer.WriteNumber("turn_total", turnTotal);
		writer.WriteNumber("current_turn", currentTurn);

		writer.WriteEndObject();
	}

	public override string ToString()
	{
		return "GameData { id = " + id + ", turnTotal = " + turnTotal + ", currentTurn = " + currentTurn + " }"; 
	}
}
