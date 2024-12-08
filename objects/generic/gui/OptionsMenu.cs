using Godot;
using System;

public partial class OptionsMenu : TabContainer
{
	private HSlider masterVolumeSlider;
	private HSlider musicVolumeSlider;
	private HSlider soundVolumeSlider;

	public override void _Ready()
	{
		//load values from settings
		GetNode<OptionButton>("Video/ResolutionDropdown").Selected = GameManager.settingsManager.resolution;
		GetNode<CheckButton>("Video/FullscreenButton").ButtonPressed = GameManager.settingsManager.fullscreen;

		masterVolumeSlider = GetNode<HSlider>("Audio/MasterVolumeSlider");
		musicVolumeSlider = GetNode<HSlider>("Audio/MusicVolumeSlider");
		soundVolumeSlider = GetNode<HSlider>("Audio/SoundVolumeSlider");

		masterVolumeSlider.Value = GameManager.settingsManager.masterVolume;
		musicVolumeSlider.Value = GameManager.settingsManager.musicVolume;
		soundVolumeSlider.Value = GameManager.settingsManager.soundVolume;
	}

	//called from the resolution dropdown in the video submenu
	public void resolutionChange(int index)
	{
		GameManager.settingsManager.resolutionChange(index);
	}

	//called from the fullscreen button in the video submenu
	public void fullscreenToggle(bool state)
	{
		GameManager.settingsManager.fullscreenToggle(state);
	}

	//called from the master audio slider
	public void masterVolumeChanged(bool changed)
	{
		//if it didn't change, why bother
		if(!changed) return;
		GameManager.settingsManager.masterVolumeChanged((int)masterVolumeSlider.Value);
	}

	//called from the music audio slider
	public void musicVolumeChanged(bool changed)
	{
		//if it didn't change, why bother
		if(!changed) return;
		GameManager.settingsManager.musicVolumeChanged((int)musicVolumeSlider.Value);
	}

	//called from the sound audio slider
	public void soundVolumeChanged(bool changed)
	{
		//if it didn't change, why bother
		if(!changed) return;
		GameManager.settingsManager.soundVolumeChanged((int)soundVolumeSlider.Value);
	}
}
