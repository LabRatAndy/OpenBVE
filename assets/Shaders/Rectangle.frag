#version 330 core
in VS_out
{
	vec2 TexCoords;
} fs_in;
uniform sampler2D rectTexture;
uniform bool usesTexture;
uniform vec4 rectColour;
uniform bool usesColour;
out vec4 colour;
void main()
{
	vec4 textColour;
	if(usesTexture)
	{
		textColour = texture(rectTexture,fs_in.TexCoords);
		if(usesColour)
		{
			colour = rectColour * textColour;
		}
		else
		{
			colour = textColour;
		}
	}
	else
	{
		colour = rectColour;
	}
}

