using Godot;
using System;

public partial class ButtonPrompt : Node2D
{
	public override void _Ready()
	{
		//checks if the object has the meta corresponding to the button
		string button = "a";
		if(HasMeta("button"))
			button = GetMeta("button").As<string>();

		//set button according to the object meta
		GetNode<AnimatedSprite2D>("ButtonSprite").SpriteFrames = GD.Load<SpriteFrames>("res://assets/sprites/gui/prompts/sprite_frames/button_" + button + ".tres");
	}
}
