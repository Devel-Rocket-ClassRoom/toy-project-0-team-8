using UnityEngine;
using UnityEngine.EventSystems;
public class GenericWindow : MonoBehaviour
{
    public GameObject firstSelected;

    protected WindowManager windowManager;
    public void Init(WindowManager wmanager)
    {
        windowManager = wmanager;
    }
    public virtual void Open()
    {
        gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(firstSelected);
    }
    public virtual void Open(int count)
    {
        Open();
    }
    public virtual void Close()
    {
        gameObject.SetActive(false);
    }
}