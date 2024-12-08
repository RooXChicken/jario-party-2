using Godot;
using System;

public partial class MainMenu : Node2D
{

	// I am not entirely sure if "menufull" is any different from "menu4" but they were listed separately so

	private AudioStreamPlayer player;
	private byte menuStage = 1;
	private string currentStage = "1";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		player = GetNode<AudioStreamPlayer>("MenuMusic");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
		if(Input.IsActionJustPressed("select"))
			if(menuStage <= 5)
				menuStage++;
		if(Input.IsActionJustPressed("back"))
			if(menuStage > 1)
				menuStage--;
		if(currentStage != menuStage.ToString())
		{
			currentStage = menuStage.ToString();
			player.Set("parameters/switch_to_clip", "Menu " + currentStage);
			
		}
		
	}

	public void removeTransition()
	{
		GetNode("TransitionGroup").QueueFree();
	}
}
