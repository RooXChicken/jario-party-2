extends State

func enter(old_state: String, data: Dictionary = {}) -> void:
	print("entering " + name + data["test"]);

func exit(new_state: String, data: Dictionary = {}) -> void:
	print("exiting " + name + " to " + new_state);

#func update(delta: float) -> void:
	#print("hey ");
