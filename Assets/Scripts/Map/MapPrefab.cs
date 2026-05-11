using UnityEngine;

public class MapPrefab : MonoBehaviour
{
    private BlindPanel[] _childPanels;

    private void Awake()
    {
        _childPanels = GetComponentsInChildren<BlindPanel>();
    }

    public void DisappearBlindPanels()
    {
        foreach (var childPanel in _childPanels)
        {
            childPanel.gameObject.SetActive(false);
        }
    }

    public void AppearBlindPanels()
    {
        foreach (var childPanel in _childPanels)
        {
            childPanel.gameObject.SetActive(true);
        }
    }
}
