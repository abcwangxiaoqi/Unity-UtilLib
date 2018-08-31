
using UnityEditor;
using UnityEngine;
namespace EditorTools
{
    public interface IEditorSprite : IObjectBase
    {
        Texture2D Tex { get; }
        void SetReadable(bool readable);
        void SetTextureType(TextureImporterType type);
        void SetMaxTextureSize(int size);
        void SetMipmapEnabled(bool b);
        void SetNormalMap(bool normal);
        void SetAphaIsTransparency(bool b);
        void SetPlatformTextureSettings(string platform, int maxSize, TextureImporterFormat format);
        void SetPlatformTextureSettings(string platform, int maxSize, TextureImporterFormat format, bool allowAphaSplit);
        void SetPlatformTextureSettings(string platform, int maxSize, TextureImporterFormat format, int compress, bool allowAphaSplit);
        bool HasAlphaChannel();
        bool IsSquare();
    }
}