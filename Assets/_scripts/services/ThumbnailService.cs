using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ThumbnailService
{
    static Dictionary<string, Sprite> thumbnails = new Dictionary<string, Sprite>();
    static Vector2 CENTER = new Vector2(0.5f, 0.5f);

    // Update is called once per frame
    public static Sprite ThumbnailSpriteFor(string mediaUrl)
    {
        if (thumbnails.ContainsKey(mediaUrl))
        {
            return thumbnails[mediaUrl];
        }
        else
        {
            return null;
        }
    }

    public static IEnumerator DownloadMediaImage(string mediaUrl)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(mediaUrl);
        request.downloadHandler = new DownloadHandlerTexture(true);

        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
        }
        else
        {
            thumbnails[mediaUrl] = ConvertToThumbnail(((DownloadHandlerTexture)request.downloadHandler).texture);
            yield return null;
        }
    }

    public static Sprite ConvertToThumbnail(Texture2D texture)
    {
        float ratio = 1;
        int newWidth;
        int newHeight;
        Rect finalDimensions;

        if (texture.height > 200 && texture.height <= texture.width)
        {
            ratio = (200.0f / texture.height);

        }
        else if (texture.width > 200)
        {
            ratio = 200.0f / texture.width;
        }

        if (ratio < 1)
        {
            newWidth = (int)(texture.width * ratio);
            newHeight = (int)(texture.height * ratio);
            // The wiki docs for TextureScale recommend copying the texture before scaling, but we
            // don't use this texture for anything else so it's fine to reuse.
            TextureScale.Bilinear(texture, newWidth, newHeight);
        }

        if (texture.width < texture.height)
        {
            int trimHeight = (texture.height - texture.width) / 2;
            finalDimensions = new Rect(0.0f, trimHeight, texture.width, texture.width);
        }
        else
        {
            int trimWidth = (texture.width - texture.height) / 2;
            finalDimensions = new Rect(trimWidth, 0.0f, texture.height, texture.height);
        }
        return Sprite.Create(texture, finalDimensions, CENTER);
    }
}
