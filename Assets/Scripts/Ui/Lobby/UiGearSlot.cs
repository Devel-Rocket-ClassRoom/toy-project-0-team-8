using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiGearSlot : MonoBehaviour
{
    public GachaGear GearData;

    public int slotIndex = -1;
    public Image imageIcon;
    public TextMeshProUGUI textName;
    public TextMeshProUGUI textLevel;
    public int level = 0;
    public Image selectMark;

    public Button button;

    private void Start()
    {
        SetGear();
    }


    public void SetEmpty()
    {
        imageIcon.sprite = null;
        textLevel.text = string.Empty;
    }

    public void SetGear()
    {
        imageIcon.sprite = GearData.Icon;
        textLevel.text = $"Lv.{level}";

        // 레벨이 0이면 회색으로 표시하는 기능 추가
    }
}
