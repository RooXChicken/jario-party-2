extends State

@export var transition: CanvasGroup;

func exit(_new_state: String, _data: Dictionary = {}) -> void:
	transition.queue_free();
