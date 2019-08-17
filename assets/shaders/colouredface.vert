#version 400 core
layout (location = 0) in devc3 coordinates;
layout (location = 1) in dvec3 normal;
layout (location = 2) in dvec2 texcoords;
 out dvec2 TexCoords;

 uniform dmat4 model;
 uniform dmat4 view;
 uniform dmat4 projection;

 void main()
 {
	gl_Position = projection * view * model * dvec4(coordinates,1.0d);
	TexCoords = texcoords;
 }