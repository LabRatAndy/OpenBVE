#version 330 core
in vec2 TexCoords;
out vec4 FinalColour;
uniform bool uRectangleHasTexture;
uniform bool uRectangleHasColour;
uniform vec4 uRectangleColour;
uniform sampler2D uRectangleTexture;
void main()
{
	vec4 rectColour = vec4(1.0,1.0,1.0,1.0);
	vec4 textureColour = vec4(1.0,1.0,1.0,1.0);
	if(uRectangleHasColour)
	{
		rectColour = uRectangleColour;
	}
	if(uRectangleHasTexture)
	{
		textureColour = texture(uRectangleTexture,TexCoords);
	}
	FinalColour = textureColour*rectColour;
}
