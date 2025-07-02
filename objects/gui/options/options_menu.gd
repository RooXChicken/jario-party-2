extends Node

var master_volume_slider: HSlider;
var music_volume_slider: HSlider;
var sound_volume_slider: HSlider;

func _ready() -> void:
	var fullscreen_button: CheckButton = get_node("Video/FullscreenButton");
	fullscreen_button.button_pressed = GameManager.settings.get_flag_or_default("fullscreen", false);
	
	var resolution_dropdown: OptionButton = get_node("Video/ResolutionDropdown");
	resolution_dropdown.selected = GameManager.settings.get_flag_or_default("resolution", 0);
	
	master_volume_slider = get_node("Audio/MasterVolumeSlider");
	music_volume_slider = get_node("Audio/MusicVolumeSlider");
	sound_volume_slider = get_node("Audio/SoundVolumeSlider");
	
	master_volume_slider.value = GameManager.settings.get_flag_or_default("master_volume", 0);
	music_volume_slider.value = GameManager.settings.get_flag_or_default("music_volume", 0);
	sound_volume_slider.value = GameManager.settings.get_flag_or_default("sound_volume", 0);

func master_volume_changed(changed: bool) -> void:
	if(changed):
		GameManager.settings.set_flag("master_volume",master_volume_slider.value);

func music_volume_changed(changed: bool) -> void:
	if(changed):
		GameManager.settings.set_flag("music_volume", music_volume_slider.value);

func sound_volume_changed(changed: bool) -> void:
	if(changed):
		GameManager.settings.set_flag("sound_volume", sound_volume_slider.value);

func resolution_change(value: int) -> void:
	GameManager.settings.set_flag("resolution", value);

func fullscreen_toggle(value: bool) -> void:
	GameManager.settings.set_flag("fullscreen", value);
