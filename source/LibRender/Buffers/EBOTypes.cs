namespace LibRender
{
    /// <summary>
    /// The face type of the EBO. ie which shader to use
    /// </summary>
    internal enum EBOFaceType
    {
        /// <summary>Plain old RGBA coloured face </summary>
        ColouredFace = 1,
        /// <summary>Textured face without a transparency </summary>
        TexturedFace = 2,
        /// <summary>Textured face with a transparancy </summary>
        Transparent = 3,
    }

}