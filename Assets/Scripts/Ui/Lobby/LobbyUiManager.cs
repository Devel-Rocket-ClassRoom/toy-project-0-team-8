using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUiManager : MonoBehaviour
{
    public TextMeshProUGUI Level;
    public TextMeshProUGUI Coin;
    public TextMeshProUGUI Cristal;
    public TextMeshProUGUI currentExp;
    public float maxExp;
    public Image LobbyCookie;
    public Slider exp;
    public GachaManager data;

    private void OnEnable()
    {
        SaveLoadManager.Load();
        Level.text = $"{SaveLoadManager.Data.playerLevel}";
        maxExp = SaveLoadManager.Data.playerLevel * 100f;
        currentExp.text = $"{SaveLoadManager.Data.currentExp}/{maxExp}";
        exp.value = (float)SaveLoadManager.Data.currentExp / maxExp;

        Coin.text = $"{SaveLoadManager.Data.Coin:n0}";
        Cristal.text = $"{SaveLoadManager.Data.Cristal:n0}";

        for(int i = 0; i<data.cookieList.Count; i++)
        {
            if(data.cookieList[i].cookieId == (SaveLoadManager.Data.lobbyCookieId))
            {
                LobbyCookie.sprite = data.cookieList[i].Icon;

            }

        }

    }

}
