#version 400 core
layout (location = 0) in dvec3 coordinates;
layout (location = 1) in dvec3 normal;
layout (location = 2) in dvec2 texcoords;
 out dvec2 TexCoords;

 uniform dmat4 model;
 uniform dmat4 view;
 uniform dmat4 projection;

 void main()
 {
	gl_Position = vec4( projection * view * model * dvec4(coordinates,1.0));
	TexCoords = texcoords;
 }
