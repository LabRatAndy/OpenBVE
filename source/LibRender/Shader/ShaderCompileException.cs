using System;

namespace LibRender
{
    public class ShaderCompileException : Exception
    {
        private string shaderFilename;
        public ShaderCompileException(string errormessage, string shaderfilename): base(errormessage)
        {
            this.shaderFilename = shaderfilename;
        }
        public string ShaderFileaname
        {
            get { return shaderFilename; }
        }
    }
}