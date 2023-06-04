using UnityEngine;

public static class ImageConverter
{
    public static Vector2 CountSizeToKeepAspectRation(Vector2 size, Vector2 containerSize)
    {
        float maxParam = Mathf.Max(size.x, size.y);
        float minParam = Mathf.Min(size.x, size.y);
        float aspectRatio = minParam / maxParam;
        float diff;

        if (maxParam == size.x)
            diff = containerSize.x / maxParam;
        else
            diff = containerSize.y / maxParam;

        float firstParam = maxParam * diff;
        float secondParam = firstParam * aspectRatio;

        Vector2 newSize;
        if (maxParam == size.x)
            newSize = new Vector2(firstParam, secondParam);
        else
            newSize = new Vector2(secondParam, firstParam);

        return newSize;
    }
}
