using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace LibRender3.Shader
{


    internal class Shader : IDisposable
    {
        private readonly int handle;
        private int vertexShaderhandle = -1;
        private int fragmentShaderhandle = -1;
        private int geometryShaderHandle = -1;
        private int tesscontrolShaderHandle = -1;
        private int tessevalShaderHandle = -1;

        internal Shader(string vertexShaderFilename, string fragmentShaderFilename)
        {
            if (string.IsNullOrEmpty(vertexShaderFilename)) throw new ArgumentNullException("vertexShaderFilename is null or empty string");
            if (string.IsNullOrEmpty(fragmentShaderFilename)) throw new ArgumentNullException("fragmentShader file name is null or empty string");
            handle = GL.CreateProgram();
            LoadShader(vertexShaderFilename, ShaderType.VertexShader);
            LoadShader(fragmentShaderFilename, ShaderType.FragmentShader);		
        }
		internal Shader(string vertexShaderFilename,string fragmentShaderFilename, string geomShaderfilename, string tessctrlShaderfilename, string tessevalShaderFilename)
        {

        }

		internal void Activate()
        {

        }
		internal void Deactivate()
        {

        }
		internal int GetUniformHandle(string uniformName)
        {

        }
		internal int GetAttributeHandle(string attributeName)
        {
			
        }

		private void LoadShader(string Shaderfilename, ShaderType shaderType)
        {

        }
		public void Dispose()
		{
		
		}
		~Shader()
		{
		
		}
    }
}
