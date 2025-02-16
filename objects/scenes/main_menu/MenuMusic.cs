using Godot;
using System;

public partial class MenuMusic : AudioStreamPlayer
{

	private byte menuStage = 0;
	private AudioStreamSynchronized streamSync;

	private Tween tween;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//:3
		// what the fuck

		streamSync = (AudioStreamSynchronized)Stream;
		tween = CreateTween();
		GD.Print(Stream.GetType());
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	
		if(Input.IsActionJustPressed("select"))
			if(menuStage < 3)
			{
				tween.TweenProperty(GetParent(), "stream_" + menuStage + "/volume", 0, 1.0f);
				//streamSync.SetSyncStreamVolume(menuStage, 0);
				menuStage++;
			}
		if(Input.IsActionJustPressed("back"))
			if(menuStage > 0)
			{
				streamSync.SetSyncStreamVolume(menuStage, -80);
				menuStage--;
			}
	}
}
