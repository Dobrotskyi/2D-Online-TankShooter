using UnityEngine;

public static class ImageLoader
{
    public static Sprite MakeSprite(string base64String, Vector2 center)
    {
        byte[] bytes = System.Convert.FromBase64String(base64String);

        Texture2D texture = new Texture2D(1, 1);
        texture.LoadImage(bytes);
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), center);

        return sprite;
    }
}
