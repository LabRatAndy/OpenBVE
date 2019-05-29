using System;
using OpenBveApi;
using OpenTK.Graphics.OpenGL;

namespace LibRender
{
    /// <summary>
    /// Class repressenting the function of an openTK/OpenGL shader
    /// </summary>
    internal class Shader : IDisposable
    {
        /// <summary>
        /// openGL shader program handle 
        /// </summary>
        private int handle;
        /// <summary>
        /// opengl handle for compiled vertex shader
        /// </summary>
        private int vertexshaderhandle;
        /// <summary>
        /// opengl handle for compiled fragment shader
        /// </summary>
        private int fragmentshaderhandle;

        /// <summary>
        /// constructor compiles assembles and links the shader program from the provided vertex and fragment shader source
        /// </summary>
        /// <param name="vertexshaderfile">File path for vertex shader source code</param>
        /// <param name="fragmentshaderfile">File path for fragment shader source code</param>
        internal Shader(string vertexshaderfile, string fragmentshaderfile)
        {
            handle = GL.CreateProgram();
            LoadShader(vertexshaderfile, ShaderType.VertexShader);
            LoadShader(fragmentshaderfile, ShaderType.FragmentShader);
            GL.AttachShader(handle, vertexshaderhandle);
            GL.AttachShader(handle, fragmentshaderhandle);
            GL.LinkProgram(handle);
            string error = null;
            error = GL.GetProgramInfoLog(handle);
            if(!string.IsNullOrEmpty(error))
            {
                GL.DetachShader(handle, vertexshaderhandle);
                GL.DetachShader(handle, fragmentshaderhandle);
                GL.DeleteShader(vertexshaderhandle);
                GL.DeleteShader(fragmentshaderhandle);
                throw new ///todo;
            }
            GL.DetachShader(handle, vertexshaderhandle);
            GL.DetachShader(handle, fragmentshaderhandle);
            GL.DeleteShader(vertexshaderhandle);
            GL.DeleteShader(fragmentshaderhandle);
        }

        /// <summary>
        /// Selects the shader program for use
        /// </summary>
        internal void Use()
        {
            GL.UseProgram(handle);
        }

        /// <summary>
        /// Gets the opengl handle of an attribute within the shader
        /// </summary>
        /// <param name="attributename"> name of the attribute as it appears in the shader</param>
        /// <returns>handle of the shader</returns>
        internal int GetAttributeHandle(string attributename)
        {
            return GL.GetAttribLocation(handle, attributename);
        }

        /// <summary>
        /// Gets the opengl handle of a uniform within the shader
        /// </summary>
        /// <param name="uniformname">Name of the uniform as it appears in the shader</param>
        /// <returns>handle of the shader</returns>
        internal int GetUniformHandle(string uniformname)
        {
            return GL.GetUniformLocation(handle, uniformname);
        }

        /// <summary>
        /// Cleans up deleteing the openGL shader program
        /// </summary>
        public void Dispose()
        {
            GL.DeleteProgram(handle);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Finaliser
        /// </summary>
        ~Shader()
        {
            GL.DeleteProgram(handle);
        }

        /// <summary>
        /// Loads and compiles the shader source
        /// </summary>
        /// <param name="filename">Filename of the shader sourcecode</param>
        /// <param name="shadertype">Shader type either Vertex or fragment</param>
        private void LoadShader(string filename, ShaderType shadertype)
        {
            string shadersource = System.IO.File.ReadAllText(filename, System.Text.Encoding.UTF8);
            if(shadertype == ShaderType.VertexShader)
            {
                vertexshaderhandle = GL.CreateShader(shadertype);
                GL.ShaderSource(vertexshaderhandle, shadersource);
                GL.CompileShader(vertexshaderhandle);
                string error = null;
                error = GL.GetShaderInfoLog(vertexshaderhandle);
                if(!string.IsNullOrEmpty(error))
                {
                    throw new /// todo; 
                }
            }
            if(shadertype == ShaderType.FragmentShader)
            {
                fragmentshaderhandle = GL.CreateShader(shadertype);
                GL.ShaderSource(fragmentshaderhandle, shadersource);
                GL.CompileShader(fragmentshaderhandle);
                string error = null;
                error = GL.GetShaderInfoLog(fragmentshaderhandle);
                if(!string.IsNullOrEmpty(error))
                {
                    throw new /// todo;
                }
            }
        }
    }
}