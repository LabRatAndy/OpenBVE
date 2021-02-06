#version 330 core
layout (location = 0) in vec4 vertex;
out vec2 Texcoords;
uniform mat4 uTextProjectionMatrix;
void main()
{
	gl_Position = uTextProjectionMatrix* vec4(vertex.xy,0.0,1.0);
	Texcoords = vertex.zw;
}
