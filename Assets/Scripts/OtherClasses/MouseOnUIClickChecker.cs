using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class MouseOnUIClickChecker
{
    public static bool IsMouseOverUI()
    {
        PointerEventData pointerEventData = new(EventSystem.current);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> raycastResults = new();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);

        for (int i = 0; i < raycastResults.Count; i++)
        {
            if (raycastResults[i].gameObject.GetComponent<MouseUIClickThrough>() != null)
            {
                raycastResults.RemoveAt(i);
                i--;
            }
        }

        return raycastResults.Count > 0;
    }
}
