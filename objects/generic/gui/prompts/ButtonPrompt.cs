using Godot;
using System;
using System.Collections.Generic;

struct PromptData
{
	public int position;
	public char character;
}

[Tool]
public partial class ButtonPrompt : Node2D
{
	private string _text = "";
	private Label label;

	[Export]
	public string text { get { return _text; } set { _text = value; updateText(); } }

	public override void _Ready()
	{
		//set button according to the object meta
		updateText();
		// GetNode<AnimatedSprite2D>("ButtonSprite").SpriteFrames = GD.Load<SpriteFrames>("res://assets/sprites/gui/prompts/sprite_frames/button_" + button + ".tres");
	}

	private void updateText()
	{
		// prevent godot from losing it's mind as a tool
		if(!HasNode("Text"))
			return;

		// also tool logic. frees all old sprites
		foreach(Node _node in GetChildren())
			if(_node is AnimatedSprite2D)
				_node.QueueFree();

		label = GetNode<Label>("Text");
		string _label = "";

		List<PromptData> _data = new List<PromptData>();

		// loop through all characters in the text
		for(int i = 0; i < _text.Length; i++)
		{
			char _c = _text[i];
			if(!_c.Equals('%')) // if it is not a % then add it to the label's text
			{
				_label += _c;
				continue;
			}

			if(i+1 >= _text.Length) // ensures no errors if there is a dangling %
				continue;

			// create prompt data
			PromptData _prompt;
			_prompt.position = i;
			_prompt.character = _text[i+1];

			_data.Add(_prompt);
			_label += "    "; // add whitespace to leave room for the button
			i++;
		}

		// used to store the amount of buttons created
		int _count = 0;
		foreach(PromptData _prompt in _data)
		{
			float _x = 0;
			for(int i = 0; i < _prompt.position; i++)
			{
				Rect2 _rect = label.GetCharacterBounds(i); // adds up all of the character bounds to determine the location of where to place the sprite
				_x += _rect.Size.X;
			}

			// create our button
			AnimatedSprite2D _button = new AnimatedSprite2D();
			_button.SpriteFrames = GD.Load<SpriteFrames>("res://assets/sprites/gui/prompts/sprite_frames/button_" + _prompt.character + ".tres");
			_button.Position = new Vector2(_x + 16 + (int)(_count*(4*5.125)), 18); //place it at the calculated position
			// the 5.125 is the size of the space character. we multiply this by 4 (space amount) and multiply by the amount of previous buttons.
			// deals with the fact that the whitespace isn't accounted for when there are is than one button

			_button.Play();
			AddChild(_button);

			_count++;
		}

		label.Text = _label;
		label.Size = new Vector2(label.Text.Length*35, label.Size.Y);
	}
}
