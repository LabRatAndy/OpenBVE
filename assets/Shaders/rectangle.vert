#version 330 core
layout (location = 0) in vec4 vertex;
out vec2 TexCoords;
uniform mat4 uRectangleViewMatrix;
uniform mat4 uRectangleProjectionMatrix;
void main()
{
	gl_Position = uRectangleProjectionMatrix* uRectangleViewMatrix * vec4(vertex.xy,0.0,1.0);
	TexCoords = vec2(vertex.zw);
}
