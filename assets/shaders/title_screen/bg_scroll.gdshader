shader_type canvas_item;

uniform float speed;

void vertex()
{
	//modify the uv coordinates based off of TIME
	UV.x += TIME * speed;
	UV.y += TIME * (speed / float(2));
}
