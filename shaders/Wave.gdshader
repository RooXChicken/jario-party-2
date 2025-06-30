shader_type canvas_item;

uniform float vSpeed;

void vertex() {
	VERTEX.y *= vSpeed;
}