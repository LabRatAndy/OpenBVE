using OpenTK;
using OpenBveApi.Objects;
namespace LibRender
{
    /// <summary>
    /// Renderer class members pretaining to the shader implementation
    /// </summary>
    public static partial class Renderer
    {
        /// <summary>
        /// array containing the shaders as obtained by shaderenums
        /// </summary>
        internal static Shader[] shaderList;
        /// <summary>
        /// Records that the shader in that array has been loaded
        /// </summary>
        internal static bool[] ShadersLoaded;
        /// <summary>
        /// The VAO for drawing rendering the scene
        /// </summary>
        internal static VertexArrayObject VAO;
        /// <summary>
        /// The model matrix passed to the shader
        /// </summary>
        internal static Matrix4d modelTransform;
        /// <summary>
        /// The view matrix passed to the shader
        /// </summary>
        internal static Matrix4d ViewTransform;
        /// <summary>
        /// The prospective matrix passed to the shader
        /// </summary>
        internal static Matrix4d ProjectionTransform;
        /// <summary>
        /// The fied of view in radians for the scene. 
        /// </summary>
        internal static double fieldOfView;

        /// <summary>
        /// has renderer sucessfully initialised ?
        /// </summary>
        internal static bool initialised = false;

        /// <summary>
        /// Array of objects to draw and count
        /// </summary>
        internal static ShaderMesh[] Objs;
        internal static int objcount;
     
    }

}