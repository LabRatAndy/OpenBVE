#version 400 core

in dvec2 TexCoords;
out vec4 Colour;

uniform vec4 faceColour;

void main()
{
	Colour = faceColour;
}