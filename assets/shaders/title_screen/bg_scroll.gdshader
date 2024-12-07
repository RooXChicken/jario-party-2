shader_type canvas_item;

uniform float speed;

void vertex() {
	UV.x += TIME * speed;
	UV.y += TIME * (speed / float(2));
}
