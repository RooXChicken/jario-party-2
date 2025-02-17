shader_type canvas_item;

uniform vec3 color;
uniform float speed;

void vertex() {
	UV.x += TIME * speed;
	UV.y += TIME * speed;
}

void fragment() {
	COLOR *= vec4(color.r/255.0, color.g/255.0, color.b/255.0, 1.0);
}