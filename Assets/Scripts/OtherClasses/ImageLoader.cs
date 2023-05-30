using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ImageLoader
{
    public static Sprite LoadImage(string base64String)
    {
        byte[] bytes = System.Convert.FromBase64String(base64String);

        Texture2D texture = new Texture2D(1, 1);
        texture.LoadImage(bytes);
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        return sprite;
    }
}
