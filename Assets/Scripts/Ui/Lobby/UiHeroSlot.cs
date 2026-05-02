using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiHeroSlot : MonoBehaviour
{
    public int slotIndex = -1;
    public Image imageIcon;
    public TextMeshProUGUI textName;

    public Button button;

    public SaveCharacter SaveCharacterData { get; private set; }

    public void SetEmpty()
    {
        imageIcon.sprite = null;
        textName.text = string.Empty;
        SaveCharacterData = null;
    }

    public void SetCharacter(SaveCharacter data)
    {
        SaveCharacterData = data;
        imageIcon.sprite = data.CharacterData.SpriteIcon;
        textName.text = data.CharacterData.StringName;
    }
}
