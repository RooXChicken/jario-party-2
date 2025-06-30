shader_type canvas_item;

instance uniform vec3 color;
instance uniform float speed;

void vertex() {
	UV.x += TIME * speed;
	UV.y += TIME * speed;
}

void fragment() {
	COLOR *= vec4(color, 1.0);
}