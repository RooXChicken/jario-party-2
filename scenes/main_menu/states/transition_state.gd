extends State

@export var transition: CanvasGroup;

func exit(new_state: String, data: Dictionary = {}) -> void:
	transition.queue_free();
