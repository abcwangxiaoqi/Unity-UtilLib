
using EditorTools;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TextureHandle
{
    IEditorSprite texture;
    public TextureHandle(string path)
    {
        texture = new EditorSprite(path);
    }

    public void Handle()
    {
        if (texture == null || texture.Tex==null)
        {
            return;
        }
        texture.SetReadable(true);
        texture.SetTextureType(TextureImporterType.Default);
        texture.SetMaxTextureSize(GetTextureMaxSize(texture));
        texture.SetMipmapEnabled(false);
        texture.Save();

        if (texture.Name.EndsWith("_n"))
        {
            texture.SetNormalMap(true);
        }

        if (texture.IsSquare())//方图
        {
            if (texture.HasAlphaChannel())//有alpha通道
            {
                texture.SetAphaIsTransparency(true);
                texture.SetPlatformTextureSettings(RuntimePlatform.Android.ToString(), GetTextureMaxSize(texture), TextureImporterFormat.ETC2_RGBA8);
                texture.SetPlatformTextureSettings(RuntimePlatform.IPhonePlayer.ToString(), GetTextureMaxSize(texture), TextureImporterFormat.PVRTC_RGBA4);
            }
            else
            {
                texture.SetPlatformTextureSettings(RuntimePlatform.Android.ToString(), GetTextureMaxSize(texture), TextureImporterFormat.ETC_RGB4);
                texture.SetPlatformTextureSettings(RuntimePlatform.IPhonePlayer.ToString(), GetTextureMaxSize(texture), TextureImporterFormat.PVRTC_RGB4);
            }
        }
        else//非方图
        {
            if (texture.HasAlphaChannel())//有alpha通道
            {
                texture.SetAphaIsTransparency(true);
                texture.SetPlatformTextureSettings(RuntimePlatform.Android.ToString(), GetTextureMaxSize(texture), TextureImporterFormat.ARGB16);
                texture.SetPlatformTextureSettings(RuntimePlatform.IPhonePlayer.ToString(), GetTextureMaxSize(texture), TextureImporterFormat.ARGB16);
            }
            else
            {
                texture.SetPlatformTextureSettings(RuntimePlatform.Android.ToString(), GetTextureMaxSize(texture), TextureImporterFormat.RGB16);
                texture.SetPlatformTextureSettings(RuntimePlatform.IPhonePlayer.ToString(), GetTextureMaxSize(texture), TextureImporterFormat.RGB16);
            }
        }
        texture.SetReadable(false);
        texture.Save();
        texture.Import();
    }

    /// <summary>
    /// 根据图片宽高 返回图片最大尺寸
    /// </summary>
    /// <param name="size"></param>
    /// <returns></returns>
    static int GetTextureMaxSize(IEditorSprite texture)
    {
        Texture2D tex = texture.Tex;
        int width = tex.width;
        int hight = tex.height;
        int size = width >= hight ? width : hight;
        if (size >= 2048) return 2048;

        List<int> data = new List<int>() { 32, 64, 128, 256, 512, 1024, 2048 };

        if (data.Exists(delegate(int i) { return i == size; }))
        {
            return data.Find(delegate(int i) { return i == size; });
        }

        for (int i = 0; i < data.Count; i++)
        {
            if (size < data[i])
            {
                return data[i];
            }
        }
        return size;
    }
}
