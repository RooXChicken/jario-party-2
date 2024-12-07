shader_type canvas_item;

uniform float slide : hint_range(0, 1.1);

void fragment()
{
	COLOR = texture(TEXTURE, UV); //sample color from texture

	float uvX = UV.x + (UV.y*0.05); //modify uv coordinate based off UV.y to make it angular
	if(uvX < slide && uvX > slide - 0.05)
		COLOR.rgb *= 1.2; //make it
}