#version 330 core
in vec2 Texcoords;
out vec4 colour;
uniform sampler2D uFontTexture;
uniform vec4 uTextColour;
void main()
{
	vec4 sampled = vec4(1.0,1.0,1.0,texture(uFontTexture,Texcoords));
	colour = vec4(uTextColour.xyz,1.0) * sampled;
}
