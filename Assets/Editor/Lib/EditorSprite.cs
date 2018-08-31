
using UnityEngine;
using UnityEditor;

namespace EditorTools
{
    public class EditorSprite : ObjectBase, IEditorSprite
    {
        public EditorSprite(string path) : base(path) { }

        TextureImporter _textureImport;
        TextureImporter textureImport
        {
            get
            {
                if (_textureImport == null)
                {
                    _textureImport = importer as TextureImporter;
                }
                return _textureImport;
            }
        }

        Texture2D _Tex;
        public Texture2D Tex
        {
            get
            {
                if (_Tex == null)
                {
                    _Tex = Load() as Texture2D;
                }
                return _Tex;
            }
        }

        public void SetReadable(bool readable)
        {
            textureImport.isReadable = readable;
        }

        public void SetTextureType(TextureImporterType type)
        {
            textureImport.textureType = type;
        }

        public void SetMaxTextureSize(int size)
        {
            textureImport.maxTextureSize = size;
        }

        public void SetMipmapEnabled(bool b)
        {
            textureImport.mipmapEnabled = b;
        }

        public void SetAphaIsTransparency(bool b)
        {
            textureImport.alphaIsTransparency = true;
        }

        public void SetNormalMap(bool normal)
        {
            textureImport.normalmap = normal;
        }

        public void SetPlatformTextureSettings(string platform, int maxSize, TextureImporterFormat format)
        {
            textureImport.SetPlatformTextureSettings(platform, maxSize, format);
        }
        public void SetPlatformTextureSettings(string platform, int maxSize, TextureImporterFormat format, bool allowAphaSplit)
        {
            textureImport.SetPlatformTextureSettings(platform, maxSize, format, allowAphaSplit);
        }
        public void SetPlatformTextureSettings(string platform, int maxSize, TextureImporterFormat format, int compress, bool allowAphaSplit)
        {
            textureImport.SetPlatformTextureSettings(platform, maxSize, format, compress, allowAphaSplit);
        }

        /// <summary>
        /// 判断是否有alpha通道
        /// </summary>
        /// <param name="_tex"></param>
        /// <returns></returns>
        public bool HasAlphaChannel()
        {
            return textureImport.DoesSourceTextureHaveAlpha();
        }


        public bool IsSquare()
        {
            if (Tex.height % 4 == 0 &&
                Tex.width % 4 == 0)
            {
                return true;
            }
            return false;
        }
    }

}