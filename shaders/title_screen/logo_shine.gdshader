shader_type canvas_item;

void fragment() {
	COLOR = texture(TEXTURE, UV); // sample color from texture

	float uvX = UV.x + (UV.y*0.05); // modify uv coordinate based off UV.y to make it angular

	float slide = 0.0;
	if(int(TIME) % 4 == 0) {
		slide = (TIME - floor(TIME))*1.1;
	}

	if(uvX < slide && uvX > slide - 0.05)
		COLOR.rgb *= 1.2; // make it
}