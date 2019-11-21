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
			//link shader
			int status = 0;
			GL.AttachShader(handle,vertexShaderhandle);
			GL.AttachShader(handle,fragmentShaderhandle);
			GL.LinkProgram(handle);
			GL.GetProgram(handle, GetProgramParameterName.LinkStatus,out status);
			if(status != 0) throw new ApplicationException(GL.GetProgramInfoLog(handle));
			//clean up
			GL.DetachShader(handle,vertexShaderhandle);
			GL.DetachShader(handle,fragmentShaderhandle);
			GL.DeleteShader(vertexShaderhandle);
			GL.DeleteShader(fragmentShaderhandle);
        }
		internal Shader(string vertexShaderFilename,string fragmentShaderFilename, string geomShaderfilename, string tessctrlShaderfilename, string tessevalShaderFilename)
        {
			//load shaders as needed null strings skipped
			if(!string.IsNullOrEmpty(vertexShaderFilename)) LoadShader(vertexShaderFilename,ShaderType.VertexShader);
			if(!string.IsNullOrEmpty(fragmentShaderFilename)) LoadShader(fragmentShaderFilename,ShaderType.FragmentShader);
			if(!string.IsNullOrEmpty(geomShaderfilename)) LoadShader(geomShaderfilename,ShaderType.GeometryShader);
			if(!string.IsNullOrEmpty(tessctrlShaderfilename)) LoadShader(tessctrlShaderfilename,ShaderType.TessControlShader);
			if(!string.IsNullOrEmpty(tessevalShaderFilename)) LoadShader(tessevalShaderFilename,ShaderType.TessEvaluationShader);
			//link shader
			int status = 0;
			if(vertexShaderhandle != -1) GL.AttachShader(handle,vertexShaderhandle);
			if(fragmentShaderhandle != -1) GL.AttachShader(handle,fragmentShaderhandle);
			if(geometryShaderHandle != -1) GL.AttachShader(handle,geometryShaderHandle);
			if(tesscontrolShaderHandle != -1) GL.AttachShader(handle,tesscontrolShaderHandle);
			if(tessevalShaderHandle != -1) GL.AttachShader(handle, tessevalShaderHandle);
			GL.LinkProgram(handle);
			GL.GetProgram(handle, GetProgramParameterName.LinkStatus, out status);
			if(status !=0) throw new ApplicationException(GL.GetProgramInfoLog(handle));
			//clean up
			if(vertexShaderhandle != -1) 
			{
				GL.DetachShader(handle,vertexShaderhandle);
				GL.DeleteShader(vertexShaderhandle);
			}
			if(fragmentShaderhandle != -1) 
			{
				GL.DetachShader(handle,fragmentShaderhandle);
				GL.DeleteShader(fragmentShaderhandle);
			}
			if(geometryShaderHandle != -1)
			{
				GL.DetachShader(handle,geometryShaderHandle);
				GL.DeleteShader(geometryShaderHandle);
			}
			if(tesscontrolShaderHandle != -1)
			{
				GL.DetachShader(handle,tesscontrolShaderHandle);
				GL.DeleteShader(tesscontrolShaderHandle);
			}
			if(tessevalShaderHandle != -1)
			{ 
				GL.DetachShader(handle, tessevalShaderHandle);
				GL.DeleteShader(tessevalShaderHandle);
			}
        }

		internal void Activate()
        {
			GL.UseProgram(handle);
        }
		internal void Deactivate()
        {
			GL.UseProgram(0);
        }
		internal int GetUniformHandle(string uniformName)
        {
			return GL.GetUniformLocation(handle,uniformName);
        }
		internal int GetAttributeHandle(string attributeName)
        {
			return GL.GetAttribLocation(handle, attributeName);
        }

		private void LoadShader(string Shaderfilename, ShaderType shaderType)
        {
			string shadersource = System.IO.File.ReadAllText(Shaderfilename, System.Text.Encoding.UTF8);
			switch (shaderType)
			{
				case ShaderType.VertexShader:
					vertexShaderhandle = GL.CreateShader(shaderType);
					GL.ShaderSource(vertexShaderhandle,shadersource);
					GL.CompileShader(vertexShaderhandle);
					CheckShader(vertexShaderhandle);
					break;
				case ShaderType.FragmentShader:
					fragmentShaderhandle = GL.CreateShader(shaderType);
					GL.ShaderSource(fragmentShaderhandle, shadersource);
					GL.CompileShader(fragmentShaderhandle);
					CheckShader(fragmentShaderhandle);
					break;
				case ShaderType.GeometryShader:
					geometryShaderHandle = GL.CreateShader(shaderType);
					GL.ShaderSource(geometryShaderHandle,shadersource);
					GL.CompileShader(geometryShaderHandle);
					CheckShader(geometryShaderHandle);
					break;
				case ShaderType.TessControlShader:
					tesscontrolShaderHandle = GL.CreateShader(shaderType);
					GL.ShaderSource(tesscontrolShaderHandle,shadersource);
					GL.CompileShader(tesscontrolShaderHandle);
					CheckShader(tesscontrolShaderHandle);
					break;
				case ShaderType.TessEvaluationShader:
					tessevalShaderHandle = GL.CreateShader(shaderType);
					GL.ShaderSource(tessevalShaderHandle,shadersource);
					GL.CompileShader(tessevalShaderHandle);
					CheckShader(tessevalShaderHandle);
					break;
				default:
					throw new ArgumentException("ShaderType not supported by load Shader");
			}

        }
		private void CheckShader(int Shaderhandle)
		{
			int status  = 0;
			GL.GetShader(Shaderhandle, ShaderParameter.CompileStatus, out status);
			if(status != 0)
			{
				throw new ApplicationException(GL.GetShaderInfoLog(Shaderhandle));
			}
		}
		public void Dispose()
		{
			GL.DeleteProgram(handle);
			GC.SuppressFinalize(this);
		}
		~Shader()
		{
			GL.DeleteProgram(handle);
		}
    }
}
