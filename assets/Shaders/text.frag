#version 330 core
in vec2 Texcoords;
out vec4 colour;
uniform sampler2D uFontTexture;
uniform vec4 uTextColour;
void main()
{
	vec4 col = vec4(1.0,1.0,1.0,texture2D(uFontTexture,Texcoords).r);
	colour = vec4(uTextColour.rgb,1.0) * col;
}
