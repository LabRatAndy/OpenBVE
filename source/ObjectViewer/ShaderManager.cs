using System;
using System.Collections.Generic;

namespace OpenBve
{
    /// <summary>
    /// Singleton class to manage loading and use of shaders
    /// </summary>
    internal sealed class ShaderManager : IDisposable
    {
        // lazy T initialisation of the singleton
        private static readonly Lazy<ShaderManager> lazy = new Lazy<ShaderManager>(() => new ShaderManager());
        //
        private List<Shader> shaderList = null;
        private Dictionary<string, int> shaderindexdictionary = null;
        /// <summary>
        /// Returns the instance of the shader manager use this to get the instance of the shadermanager
        /// </summary>
        internal static ShaderManager Instance
        {
            get { return lazy.Value; }
        }
        /// <summary>
        /// Constructor is private but can only be called by the class itself using a lazy T initalisation to get a singleton object
        /// </summary>
        private ShaderManager()
        {
            shaderindexdictionary = new Dictionary<string, int>();
            shaderList = new List<Shader>();
        }
        /// <summary>
        /// Adds a Shader to the manager
        /// </summary>
        /// <param name="shader">Shader object to add to the manager</param>
        /// <param name="shadername">The name of the shader, this can be used to access the shader</param>
        internal void AddShader(Shader shader, string shadername)
        {
            if (shader == null) throw new ArgumentNullException("Shader cannot be null");
            if (string.IsNullOrWhiteSpace(shadername)) throw new ArgumentException("Shadername cannot be null, empty string or only whitespace characters");
            shaderList.Add(shader);
            int index = shaderList.IndexOf(shader);
            shaderindexdictionary.Add(shadername, index);
        }
        /// <summary>
        /// Gets the shader
        /// </summary>
        /// <param name="shadername">the name of the shader to get as given at addshader</param>
        /// <returns>The shader requested</returns>
        internal Shader GetShader(string shadername)
        {
            if (string.IsNullOrWhiteSpace(shadername)) throw new ArgumentException("Shadername cannot be null, empty string or only whitespace characters");
            int index;
            if(shaderindexdictionary.TryGetValue(shadername, out index) == false)
            {
                throw new KeyNotFoundException("cannot find shadername in shaders list");
            }
            return shaderList[index];
        }
        /// <summary>
        /// Gets the shader 
        /// </summary>
        /// <param name="index">The index of the Shader to get</param>
        /// <returns>The shader requested</returns>
        internal Shader GetShader(int index)
        {
            return shaderList[index];
        }
        /// <summary>
        /// Returns the index of the shader from the shadername given in addshader
        /// </summary>
        /// <param name="shadername">The name of the shader given in addshader</param>
        /// <returns>The index of the shader</returns>
        internal int GetShaderIndex(string shadername)
        {
            if (string.IsNullOrWhiteSpace(shadername)) throw new ArgumentException("Shadername cannot be null, empty string or only whitespace characters");
            int index;
            if(shaderindexdictionary.TryGetValue(shadername, out index) == false)
            {
                throw new KeyNotFoundException("cannot find shadername in shaders list");
            }
            return index;
        }
        /// <summary>
        /// Dispose method cleans up the shaders
        /// </summary>
        public void Dispose()
        {
            foreach( Shader s in shaderList)
            {
                s.Dispose();
            }
            GC.SuppressFinalize(this);
        }
        ~ShaderManager()
        {
            foreach(Shader s in shaderList)
            {
                s.Dispose();
            }
        }
    }

}