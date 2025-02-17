using Godot;
using System;
using System.IO;

public partial class MainMenu : Node2D
{

	// I am not entirely sure if "menufull" is any different from "menu4" but they were listed separately so - D

	private readonly int STAGE_COUNT = 4;

	private byte menuStage = 0;
	private AudioStreamSynchronized streamSync;

	private double[] volume;
	private AnimationPlayer animator;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//:3
		animator = GetNode<AnimationPlayer>("AnimationPlayer");

		// FUUUCK!!! I HATE IT!!!
		/*tween = GetTree().CreateTween();
		tween.TweenProperty(streamSync, "stream_" + menuStage + "/volume", 0, 1.0f);*/
		streamSync = (AudioStreamSynchronized)GetNode<AudioStreamPlayer>("MenuMusic").Stream;
		
		volume = new double[STAGE_COUNT];
		for(int i = 1; i < STAGE_COUNT; i++)
			volume[i] = 1.0;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(Input.IsActionJustPressed("select"))
			if(menuStage < STAGE_COUNT - 1)
			{
				menuStage++;
				animator.Play("menu_stage_" + menuStage);
			}
		if(Input.IsActionJustPressed("back"))
			if(menuStage > 0)
			{
				animator.PlayBackwards("menu_stage_" + menuStage);
				menuStage--;
			}

		// loops through all music tracks
		for(byte i = 0; i < STAGE_COUNT; i++)
		{
			// uses a math formula to determine the X in a formula to determine the final volume
			volume[i] = Math.Clamp(volume[i] + ((i > menuStage) ? 1 : -1) * delta, 0, 0.5);

			// set the volume to the result of the funciton ((2)^<STEP> - 1) * -80.0f
			// 0 is 0, 0.5 is -80 (silent)
			streamSync.SetSyncStreamVolume(i, (float)(Math.Pow(4, volume[i]) - 1)*-80.0f);
		}
	}

	public void removeTransition()
	{
		GetNode("TransitionGroup").QueueFree();
	}
}
