using Godot;
using System;

public partial class main_menu : Node2D
{

	// I am not entirely sure if "menufull" is any different from "menu4" but they were listed separately so

	private AudioStreamInteractive stream;

	private byte menuStage = 1;
	private byte currentStage = 1;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		stream = GetNode<AudioStreamInteractive>("MenuMusic");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(Input.IsActionJustPressed("select"))
			menuStage++;
		if(Input.IsActionJustPressed("back"))
			menuStage--;
		if(currentStage != menuStage)
		{
			// advance stage in music here but idk what i'm doing
		}
		
	}
}
