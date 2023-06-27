using UnityEngine;

public class LoadingPanel : MonoBehaviour
{
    [SerializeField] private GameObject _usesLoading;
    private IUseLoading _obj;
    [SerializeField] private GameObject _loadingPanel;

    private void OnEnable()
    {
        _obj = _usesLoading.GetComponent<IUseLoading>();
        if(_obj == null)
            throw new System.Exception($"GameObject {_usesLoading} has no IUseLoadingComponent");

        _obj.StartLoading += ShowLoadingPanel;
        _obj.EndLoading += HideLoadingPanel;
    }

    private void OnDisable()
    {
        _obj.StartLoading -= ShowLoadingPanel;
        _obj.EndLoading -= HideLoadingPanel;
    }

    private void ShowLoadingPanel()
    {
        if(_loadingPanel.activeSelf)
            return;
        _loadingPanel.SetActive(true);
    }

    private void HideLoadingPanel()
    {
        _loadingPanel.SetActive(false);
    }
}
