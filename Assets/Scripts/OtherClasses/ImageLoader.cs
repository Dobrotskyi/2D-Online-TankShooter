using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public static class ImageLoader
{
    public static Sprite MakeSprite(string base64String, Vector2 center)
    {
        byte[] bytes = System.Convert.FromBase64String(base64String);

        Texture2D texture = new Texture2D(1, 1);
        texture.LoadImage(bytes);
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width - 0.5f, texture.height - 2), center);

        return sprite;
    }
}
