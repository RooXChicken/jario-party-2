extends Node

var master_volume_slider: HSlider;
var music_volume_slider: HSlider;
var sound_volume_slider: HSlider;

func _ready() -> void:
	var fullscreen_button: CheckButton = get_node("Video/FullscreenButton");
	fullscreen_button.button_pressed = GameManager.settings.fullscreen;
	
	var resolution_dropdown: OptionButton = get_node("Video/ResolutionDropdown");
	resolution_dropdown.selected = GameManager.settings.resolution;
	
	master_volume_slider = get_node("Audio/MasterVolumeSlider");
	music_volume_slider = get_node("Audio/MusicVolumeSlider");
	sound_volume_slider = get_node("Audio/SoundVolumeSlider");
	
	master_volume_slider.value = GameManager.settings.master_volume;
	music_volume_slider.value = GameManager.settings.music_volume;
	sound_volume_slider.value = GameManager.settings.sound_volume;

func master_volume_changed(changed: bool) -> void:
	if(changed):
		GameManager.settings.master_volume = master_volume_slider.value;
		GameManager.settings.save_settings();

func music_volume_changed(changed: bool) -> void:
	if(changed):
		GameManager.settings.music_volume = music_volume_slider.value;
		GameManager.settings.save_settings();

func sound_volume_changed(changed: bool) -> void:
	if(changed):
		GameManager.settings.sound_volume = sound_volume_slider.value;
		GameManager.settings.save_settings();

func resolution_change(value: int) -> void:
	GameManager.settings.resolution = value;
	GameManager.settings.save_settings();

func fullscreen_toggle(value: bool) -> void:
	GameManager.settings.fullscreen = value;
	GameManager.settings.save_settings();
