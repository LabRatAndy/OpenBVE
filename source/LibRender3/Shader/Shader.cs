using System;
using OpenTK;
using OpenTk.Graphics;
using OpenTk.Graphics.OpenGL;

namespace LibRender3.Shader
{


    internal class Shader : IDisposable
    {
        private readonly int handle;
        private int vertexShaderhandle;
        private int fragmentShaderhandle;
        private int geometryShaderHandle;
        private int tesscontrolShaderHandle;
        private int tessevalShaderHandle;

        internal Shader(string vertexShaderFilename, string fragmentShaderFilename)
        {
			
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
    }
}
