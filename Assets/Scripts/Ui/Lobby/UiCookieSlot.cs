using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiCookieSlot : MonoBehaviour
{
    public GachaCookie CookieData;

    public int slotIndex = -1;
    public Image imageIcon;
    public TextMeshProUGUI textName;
    public TextMeshProUGUI textLevel;
    public int level;

    public Button button;
    public Image selectMark;

    private void Start()
    {
        SetCookie();
        
    }
    public void SetEmpty()
    {
        imageIcon.sprite = null;
        textName.text = string.Empty;
    }

    public void SetCookie()
    {
        imageIcon.sprite = CookieData.Icon;
        textName.text = CookieData.cookieName;
        textLevel.text = $"Lv.{level}";
    }

}
