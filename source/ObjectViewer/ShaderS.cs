using System;
using OpenTK.Graphics.OpenGL;

namespace OpenBve
{
    /// <summary>
    /// Class to represent a openTK Shader program
    /// </summary>
    internal class Shader : IDisposable
    {
        private int handle;
        private int vertexshader;
        private int fragmentshader;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="vertexshaderpath">file path and name to vertex shader source</param>
        /// <param name="fragmentshaderpath">file path and name to fragmetn shader source</param>
        internal Shader(string vertexshaderpath, string fragmentshaderpath)
        {
            handle = GL.CreateProgram();
            LoadShader(vertexshaderpath, ShaderType.VertexShader);
            LoadShader(fragmentshaderpath, ShaderType.FragmentShader);
            GL.AttachShader(handle, vertexshader);
            GL.AttachShader(handle, fragmentshader);
            GL.LinkProgram(handle);
#if DEBUG
            string error;
            error = GL.GetProgramInfoLog(handle);
            Console.WriteLine(error);
#endif
            GL.DetachShader(handle, vertexshader);
            GL.DetachShader(handle, fragmentshader);
            GL.DeleteShader(vertexshader);
            GL.DeleteShader(fragmentshader);
        }
        /// <summary>
        /// Activate the shader program for use
        /// </summary>
         internal void Use()
        {
            GL.UseProgram(handle);
        }
        /// <summary>
        /// gets the handle of an attribute within the shader
        /// </summary>
        /// <param name="attributename">name of attribute to get the handle of</param>
        /// <returns> the int that is the handle to the attribute</returns>
        internal int GetAttributeHandle(string attributename)
        {
            return GL.GetAttribLocation(handle, attributename);
        }
        /// <summary>
        /// Gets the openTK handle of the uniform with in the shader
        /// </summary>
        /// <param name="uniformname">Name of the uniform to get the handle of</param>
        /// <returns>The int that is the openTK handle of the uniform </returns>
        internal int GetUniformHandle(string uniformname)
        {
            return GL.GetUniformLocation(handle, uniformname);
        }
        /// <summary>
        /// cleans up, relasing the underlying openTK/OpenGL shader program
        /// </summary>
        public void Dispose()
        {
            GL.DeleteProgram(handle);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// finalizer make sure underlying openTK shader has been release
        /// </summary>
        ~Shader()
        {
            GL.DeleteProgram(handle);
        }
        /// <summary>
        /// loads the Shader source and compiles the shader 
        /// </summary>
        /// <param name="filename">filename of the shader source</param>
        /// <param name="shadertype">type of shader vertexshader or fragmentshader</param>
        private void LoadShader(string filename,ShaderType shadertype)
        {
            string shadersource = System.IO.File.ReadAllText(filename, System.Text.Encoding.UTF8);
            if(shadertype == ShaderType.VertexShader)
            {
                vertexshader = GL.CreateShader(shadertype);
                GL.ShaderSource(vertexshader, shadersource);
                GL.CompileShader(vertexshader);
#if DEBUG
                string error = null;
                error = GL.GetShaderInfoLog(vertexshader);
                Console.WriteLine(error);
#endif
            }
            if(shadertype == ShaderType.FragmentShader)
            {
                fragmentshader = GL.CreateShader(shadertype);
                GL.ShaderSource(fragmentshader, shadersource);
                GL.CompileShader(fragmentshader);
#if DEBUG
                string error = null;
                error = GL.GetShaderInfoLog(fragmentshader);
                Console.WriteLine(error);
#endif
            }
        }
    }
}