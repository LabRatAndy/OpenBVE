#version 330 core
in VS_out
{
	vec2 TexCoords;
} fs_in;
uniform sampler2D rectTexture;
uniform vec4 rectColour;
out vec4 colour;
void main()
{
	vec4 textColour = texture(rectTexture,fs_in.TexCoords);
	colour = rectColour * textColour;
}

