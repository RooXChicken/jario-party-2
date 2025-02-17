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
		if(!HasNode("Text"))
			return;

		foreach(Node _node in GetChildren())
			if(_node is AnimatedSprite2D)
				_node.QueueFree();

		label = GetNode<Label>("Text");
		string _label = "";

		List<PromptData> _data = new List<PromptData>();
		for(int i = 0; i < _text.Length; i++)
		{
			char _c = _text[i];
			if(!_c.Equals('%'))
			{
				_label += _c;
				continue;
			}

			if(i+1 >= _text.Length)
				continue;

			PromptData _prompt;
			_prompt.position = i;
			_prompt.character = _text[i+1];

			_data.Add(_prompt);
			_label += "    ";
			i++;
		}

		int _count = 0;
		foreach(PromptData _prompt in _data)
		{
			float _x = 0;
			for(int i = 0; i < _prompt.position; i++)
			{
				Rect2 _rect = label.GetCharacterBounds(i);
				_x += _rect.Size.X;
			}

			AnimatedSprite2D _button = new AnimatedSprite2D();
			_button.SpriteFrames = GD.Load<SpriteFrames>("res://assets/sprites/gui/prompts/sprite_frames/button_" + _prompt.character + ".tres");
			_button.Position = new Vector2(_x + 16 + (int)(_count*(4*5.125)), 18);
			_button.Play();
			// _button.Scale = new Vector2(0.5f, 0.5f);
			AddChild(_button);

			_count++;
		}

		label.Text = _label;
		label.Size = new Vector2(label.Text.Length*35, label.Size.Y);
	}
}
