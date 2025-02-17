shader_type canvas_item;

uniform vec3 color;
uniform float speed;

void vertex() {
	UV.x += TIME * speed;
	UV.y += TIME * speed;
}

void fragment() {
	COLOR *= vec4(color, 1.0);
}