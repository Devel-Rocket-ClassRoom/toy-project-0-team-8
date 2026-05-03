using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiGearSlot : MonoBehaviour
{
    public int slotIndex = -1;
    public Image imageIcon;
    public TextMeshProUGUI textName;

    public Button button;

    public SaveGear SaveGearData { get; private set; }

    public void SetEmpty()
    {
        imageIcon.sprite = null;
        textName.text = string.Empty;
        SaveGearData = null;
    }

    public void SetGear(SaveGear data)
    {
        SaveGearData = data;
        imageIcon.sprite = data.GearData.SpriteIcon;
        textName.text = data.GearData.StringName;
    }
}
