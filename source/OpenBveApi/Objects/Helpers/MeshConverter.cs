using OpenBveApi.Math;
using OpenBveApi.LibRender;

namespace OpenBveApi.Objects
{
    /// <summary>
    /// Helper class used to convert  a Mesh struct into a shader mesh struct made up of a VBO and EBOs.
    /// </summary>
    public static class MeshConverter
    {
        /// <summary>
        /// Helper struct used to gather the vertex data prior to conversion to a double array and vbo.
        /// </summary>
        private struct MeshConverterVertex
        {
           public Vector3 coordinates;
           public Vector3 normals;
           public Vector2 texcoords;
        }
        /// <summary>
        /// Converts a Mesh struct into a ShaderMesh Struct
        /// </summary>
        /// <param name="mesh">The Mesh to convert</param>
        /// <returns>The ShaderMesh version of the supplied Mesh struct</returns>
        public static ShaderMesh ConvertMesh(Mesh mesh, bool dynamic)
        {
            MeshConverterVertex[] vertices = new MeshConverterVertex[mesh.Vertices.Length];
            int[][] faces = null;
            ushort[] materials = null;
            double[] vertexData = null;
            MeshMaterial[] materiallist = null;
            ElementBufferObject[] ebos = null;
			int[] faceType = null;

            GetVertexData(mesh, ref vertices);
            GetFaceData(mesh, ref faces,ref faceType);
            GetNormalsData(mesh, ref vertices);
            GetFacesMaterials(mesh, ref materials);
            GetVertexDataAsDoubleArray(vertices, out vertexData);
            vertices = null;
            VertexBufferObject vbo = new VertexBufferObject(vertexData);
			vbo.Bind();
			if(dynamic == true) vbo.BufferData(OpenTK.Graphics.OpenGL.BufferUsageHint.DynamicDraw);
			else vbo.BufferData(OpenTK.Graphics.OpenGL.BufferUsageHint.StaticDraw);
            GetFacesasEBOArray(faces, out ebos, dynamic);
			vbo.UnBind();
            faces = null;
			//BufferData(ref vbo, ref ebos, dynamic);
            GetMeshMaterialsArray(mesh, materials, out materiallist);
            SetEBOMaterialType(materiallist, ref ebos);
            return new ShaderMesh(vbo, ebos, materiallist);
        }
        /// <summary>
        /// Extracts the vertex coordinates and tex coordinates  from the vertices associated with the mesh struct and places into array of vertex helper structs 
        /// </summary>
        /// <param name="mesh">The mesh being converted</param>
        /// <param name="Vertices">The array of Vertex structs that contains the data for the vertices associated with the mesh </param>
        private static void GetVertexData(Mesh mesh, ref MeshConverterVertex[] Vertices)
        {
            for (int n = 0; n < mesh.Vertices.Length; n++)
            {
                Vertices[n].coordinates = mesh.Vertices[n].Coordinates;
                Vertices[n].texcoords = mesh.Vertices[n].TextureCoordinates;
            }
        }
        /// <summary>
        /// Extracts the Face details for each face in the mesh's faces array and places into a seperate int array row, 
        /// 1 for each face; in a multidimensional int array. ie int[face][vertiices that make up face]
        /// </summary>
        /// <param name="mesh">Teh mesh being converted</param>
        /// <param name="faces">The multidimensional int array containing the facedata associated with the mesh</param>
        private static void GetFaceData(Mesh mesh, ref int[][] faces, ref int[] faceType)
        {
			faceType = new int[mesh.Faces.Length];
            faces = new int[mesh.Faces.Length][];
            for (int face = 0; face < mesh.Faces.Length; face++)
            {
				switch(mesh.Faces[face].Flags&MeshFace.FaceTypeMask)
				{
					case MeshFace.FaceTypePolygon:
						faceType[face] = MeshFace.FaceTypePolygon;
						break;
					case MeshFace.FaceTypeTriangles:
						faceType[face] = MeshFace.FaceTypeTriangles;
						break;
					case MeshFace.FaceTypeTriangleStrip:
						faceType[face] = MeshFace.FaceTypeTriangleStrip;
						break;
					case MeshFace.FaceTypeQuads:
						faceType[face] = MeshFace.FaceTypeQuads;
						break;
					case MeshFace.FaceTypeQuadStrip:
						faceType[face] = MeshFace.FaceTypeQuadStrip;
						break;
				}
				faces[face] = new int[mesh.Faces[face].Vertices.Length];
                for (int vertex = 0; vertex < mesh.Faces[face].Vertices.Length; vertex++)
                {
                    faces[face][vertex] = mesh.Faces[face].Vertices[vertex].Index;
                }
            }
        }
        /// <summary>
        /// Extracts the normals data from the mesh being converted and adds it to the normal in the vertex struct array
        /// </summary>
        /// <param name="mesh">The mesh to convert</param>
        /// <param name="Verticies">The array of vertex structs that the normals should be added to</param>
        private static void GetNormalsData(Mesh mesh, ref MeshConverterVertex[] Verticies)
        {
            for (int face = 0; face < mesh.Faces.Length; face++)
            {
                for (int vertex = 0; vertex < mesh.Faces[face].Vertices.Length; vertex++)
                {
                    int index = mesh.Faces[face].Vertices[vertex].Index;
                    Verticies[index].normals = mesh.Faces[face].Vertices[vertex].Normal;
                }
            }
        }
        /// <summary>
        /// Extracts the material index for each face and stores as a ushort array in the order the faces are retrieved by GetFaceDate
        /// </summary>
        /// <param name="mesh">The mesh being converted</param>
        /// <param name="materials">The ushort array storeing the index to the material used for each face in the order they are retreived by GetFaceData</param>
        private static void GetFacesMaterials(Mesh mesh, ref ushort[] materials)
        {
			materials = new ushort[mesh.Faces.Length];
            for (int face = 0; face < mesh.Faces.Length; face++)
            {
                materials[face] = mesh.Faces[face].Material;
            }
        }
        /// <summary>
        /// Converts the Vertex struct array to a double array that is used to create a VBO
        /// </summary>
        /// <param name="vertices">Array of Vertex helper structs that is to be converted to a double array</param>
        /// <param name="VertexData">The double array representing the vertex data contained by the mesh. Used to create the VBO</param>
        private static void GetVertexDataAsDoubleArray(MeshConverterVertex[] vertices, out double[]VertexData)
        {
            VertexData = new double[vertices.Length * 8];
            for (int n = 0; n < vertices.Length; n++)
            {
                int vertexoffset = n * 8;
                VertexData[vertexoffset] = vertices[n].coordinates.X;
                VertexData[vertexoffset + 1] = vertices[n].coordinates.Y;
                VertexData[vertexoffset + 2] = vertices[n].coordinates.Z;
                VertexData[vertexoffset + 3] = vertices[n].normals.X;
                VertexData[vertexoffset + 4] = vertices[n].normals.Y;
                VertexData[vertexoffset + 5] = vertices[n].normals.Z;
                VertexData[vertexoffset + 6] = vertices[n].texcoords.X;
                VertexData[vertexoffset + 7] = vertices[n].texcoords.Y;
            }
        }
        /// <summary>
        /// Converts the faces data array into an array of EBOs one for each face 
        /// </summary>
        /// <param name="faces">int[face][vertiices that make up face] array, containing the face data to make the EBO array</param>
        /// <param name="ebos">Array of EBOs that represent the faces one for each face</param>
        private static void GetFacesasEBOArray(int[][] faces, out ElementBufferObject[] ebos, bool dynamic)
        {
            int facecount = faces.GetLength(0);
            ebos = new ElementBufferObject[facecount];
            for (int n = 0; n < facecount; n++)
            {
                ebos[n] = new ElementBufferObject(faces[n]);
				ebos[n].Bind();
				if(dynamic == true) ebos[n].BufferData(OpenTK.Graphics.OpenGL.BufferUsageHint.DynamicDraw);
				else ebos[n].BufferData(OpenTK.Graphics.OpenGL.BufferUsageHint.StaticDraw);
				ebos[n].Unbind();
            }
        }
        /// <summary>
        /// Extracts an array of MeshMaterail structs from the mesh conataining the materials used on the faces of the mesh in the 
        /// order of which the faces are in the faces data array and therefore the EBO array and the resultant ShaderMesh
        /// </summary>
        /// <param name="mesh">The mesh being converted </param>
        /// <param name="materialindex">a ushort array that contains the material index for each face in the order that the faces are in, in the EBO array </param>
        /// <param name="materials">Array returning the mesh materials used in the order that the faces are in</param>
        private static void GetMeshMaterialsArray(Mesh mesh, ushort[] materialindex, out MeshMaterial[] materials)
        {
            materials = new MeshMaterial[materialindex.Length];
            for (int n = 0; n < materialindex.Length; n++)
            {
                materials[n] = mesh.Materials[materialindex[n]];
            }
        }
        /// <summary>
        /// Sets the material type for each of the ebos either type coloured, textured or transparent textured
        /// </summary>
        /// <param name="materials">materials array</param>
        /// <param name="ebos"> EBO array passed by reference</param>
        private static void SetEBOMaterialType(MeshMaterial[] materials, ref ElementBufferObject[] ebos)
        {
            for (int n = 0; n < ebos.Length; n++)
            {
                if(materials[n].DaytimeTexture != null || materials[n].NighttimeTexture != null)
                {
                    // textured
                    if((materials[n].Flags & MeshMaterial.TransparentColorMask)== 1)
                    {
                        // transparnecy used
                        ebos[n].Type = (int)EBOType.Transparent;
                    }
                    else
                    {
                        // just plain textured
                        ebos[n].Type = (int)EBOType.Textured;
                    }
                }
                else
                {
                    // plain coloured 
                    ebos[n].Type = (int)EBOType.Coloured;
                }
            }
        }
		/// <summary>
        /// Calls the bufferdata methods for the VBO and EBOS attached to the shadermesh
        /// </summary>
        /// <param name="vbo">Vertex buffer object that needs the data buffering</param>
        /// <param name="ebos">Element buffer object array that needs the data buffering</param>
        /// <param name="dynamic">bool whether the object is marked as dynamic or not</param>
		private static void BufferData(ref VertexBufferObject vbo, ref ElementBufferObject[] ebos, bool dynamic)
		{
			OpenTK.Graphics.OpenGL.BufferUsageHint hint;
			if(dynamic==true)
			{
				hint = OpenTK.Graphics.OpenGL.BufferUsageHint.DynamicDraw;
			}	
			else
			{
				hint = OpenTK.Graphics.OpenGL.BufferUsageHint.StaticDraw;
			}
			vbo.BufferData(hint);
			foreach(ElementBufferObject ebo in ebos)
			{
				ebo.BufferData(hint);
				ebo.Unbind();
			}
			vbo.UnBind();

		}
    }

}
