using Godot;
using System;
using System.IO;

public partial class MainMenu : Node2D
{

	// I am not entirely sure if "menufull" is any different from "menu4" but they were listed separately so - D

	private AudioStreamPlayer player;
	private byte menuStage = 0;
	private string currentStage = "1";
	private AudioStreamSynchronized stream;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//:3
		stream = (AudioStreamSynchronized)GetNode<AudioStreamPlayer>("MenuMusic").Stream;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
		if(Input.IsActionJustPressed("select"))
			if(menuStage < 3)
			{
				stream.SetSyncStreamVolume(menuStage, 0);
				menuStage++;
			}
		if(Input.IsActionJustPressed("back"))
			if(menuStage > 0)
			{
				stream.SetSyncStreamVolume(menuStage, -80);
				menuStage--;
				
			}
				
		
	}

	public void removeTransition()
	{
		GetNode("TransitionGroup").QueueFree();
	}
}
