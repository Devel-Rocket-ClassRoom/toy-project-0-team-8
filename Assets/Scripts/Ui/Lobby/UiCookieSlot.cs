using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiCookieSlot : MonoBehaviour
{
    public int slotIndex = -1;
    public Image imageIcon;
    public TextMeshProUGUI textName;

    public Button button;

    public SaveCookie SaveCookieData { get; private set; }

    public void SetEmpty()
    {
        imageIcon.sprite = null;
        textName.text = string.Empty;
        SaveCookieData = null;
    }

    public void SetCookie(SaveCookie data)
    {
        SaveCookieData = data;
        imageIcon.sprite = data.CookieData.SpriteIcon;
        textName.text = data.CookieData.StringName;
    }



}
