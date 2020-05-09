#version 330 core
layout (location = 0) in vec2 vertex;
layout (location = 1) in vec2 texCords;
out VS_out
{
	vec2 TexCoords;
} vs_out;
uniform mat4 rectangleProjectionMatrix;
void main()
{
	gl_Position = rectangleProjectionMatrix *vec4(vertex,0.0,0.0);
	vs_out.TexCoords = texCords;
}
