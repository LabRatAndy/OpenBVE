using System;
using OpenBveApi.Colors;
using OpenBveApi.Textures;
using OpenBveApi.Objects;

namespace LibRender
{
    public struct MaterialStruct
    {
        Color32 ambientColour;
        Color32 diffuseColour;
        Color24 specularColour;
        float shininess;
        Color24 emmisiveColour;
        Color24 textureTransparentColour;
        Texture dayTexture;
        Texture nightTexture;


        public MaterialStruct(MeshMaterial material)
        {
            ambientColour = material.Color;
            diffuseColour = material.Color;
            // not currently supported by OpenBVE so set to white at 50% shininess
            specularColour = Color24.White;
            shininess = 0.5f;
            emmisiveColour = material.EmissiveColor;
            textureTransparentColour = material.TransparentColor;
            dayTexture = material.DaytimeTexture;
            nightTexture = material.NighttimeTexture;
        }

        public MaterialStruct(MeshMaterial material, Color24 specular, float shininess)
        {
            ambientColour = material.Color;
            diffuseColour = material.Color;
            specularColour = specular;
            this.shininess = shininess;
            emmisiveColour = material.EmissiveColor;
            textureTransparentColour = material.TransparentColor;
            dayTexture = material.DaytimeTexture;
            nightTexture = material.NighttimeTexture;
        }
    }

}