using Unity.VisualScripting;
using UnityEngine;

public class MouseUIClickThrough : MonoBehaviour
{
    [SerializeField] private bool _useForChildren = true;

    private void Awake()
    {
        if (!_useForChildren) return;

        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).AddComponent<MouseUIClickThrough>();
    }
}
