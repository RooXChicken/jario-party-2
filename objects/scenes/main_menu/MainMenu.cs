using Godot;
using System;
using System.IO;

public partial class MainMenu : Node2D
{

	// I am not entirely sure if "menufull" is any different from "menu4" but they were listed separately so - D

	private byte menuStage = 0;
	private AnimationPlayer animator;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//:3
		animator = GetNode<AnimationPlayer>("AnimationPlayer");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(Input.IsActionJustPressed("select"))
			{
				animator.Play("MenuStage2");
			}
		if(Input.IsActionJustPressed("back"))
			{
				animator.PlayBackwards("MenuStage2");
			}
	}

	public void removeTransition()
	{
		GetNode("TransitionGroup").QueueFree();
	}
}
