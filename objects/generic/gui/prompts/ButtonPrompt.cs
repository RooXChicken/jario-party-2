using Godot;
using System;

public partial class ButtonPrompt : Node2D
{
	[Export]
	public string button { get; set; } = "a";

	public override void _Ready()
	{
		//set button according to the object meta
		GetNode<AnimatedSprite2D>("ButtonSprite").SpriteFrames = GD.Load<SpriteFrames>("res://assets/sprites/gui/prompts/sprite_frames/button_" + button + ".tres");
	}
}
