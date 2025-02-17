using Godot;
using System;

public partial class TitleScreen : Node2D
{
	private AnimationPlayer anim;
	private AudioStreamPlayer2D titleThemePlayer;

	public override void _Ready()
	{
		//get animation player
		anim = GetNode<AnimationPlayer>("AnimationPlayer");

		//get stream player
		titleThemePlayer = GetNode<AudioStreamPlayer2D>("TitleTheme");

		//preload the nex scene in the background
		ResourceLoader.LoadThreadedRequest("res://objects/scenes/main_menu/main_menu.tscn");
	}

	public override void _Process(double delta)
	{
		if(Input.IsActionJustPressed("select") && anim.CurrentAnimationPosition > 2)
		{
			Node scene = ((PackedScene)ResourceLoader.LoadThreadedGet("res://objects/scenes/main_menu/main_menu.tscn")).Instantiate();
			scene.GetNode<Sprite2D>("TransitionGroup/PreviousScreenSprite").Texture = ImageTexture.CreateFromImage(GetViewport().GetTexture().GetImage());

			GetParent().AddChild(scene);
			QueueFree();
		}

		//skip to title
		if(Input.IsActionJustPressed("select") && anim.CurrentAnimationPosition < 2)
		{
			anim.Seek(2);

			// if(!titleThemePlayer.Playing)
			// 	titleThemePlayer.Play();
		}
	}

	public void playLoopingShine()
	{
		anim.Play("logo_shine_loop");
	}

	// public void LoadTitle()
	// {
	// 	//add the preloaded scene
	// 	GetParent().AddChild(((PackedScene)ResourceLoader.LoadThreadedGet("res://objects/title_screen/title_screen.tscn")).Instantiate());

	// 	//remove the splashscreen scene
	// 	QueueFree();
	// }
}
