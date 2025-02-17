shader_type canvas_item;

uniform float speed;
uniform bool verticalMovement;

void vertex()
{
	//modify the uv coordinates based off of TIME
	UV.x += TIME * speed;
	if(verticalMovement)
		UV.y += TIME * (speed / float(2));
}
