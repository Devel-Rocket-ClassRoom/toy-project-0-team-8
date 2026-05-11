using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUiManager : MonoBehaviour
{
    [Header("=== 메뉴 버튼 ===")]
    [SerializeField]
    private Button _menuButton;

    [Header("=== 상단 메뉴 버튼 누르면 등장할 패널 ===")]
    [SerializeField]
    private MenuPanel _menuPanel;

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

        for (int i = 0; i < data.cookieList.Count; i++)
        {
            if (data.cookieList[i].cookieId == (SaveLoadManager.Data.lobbyCookieId))
            {
                LobbyCookie.sprite = data.cookieList[i].Icon;
            }
        }

        // 처음 등장 시 메뉴 패널은 비활성화 상태로
        _menuPanel.gameObject.SetActive(false);
        // 종료 버튼 누르면 게임 종료되도록
        _menuPanel.OnQuitButtonPressed.AddListener(QuitGame);
        // 계속하기 누르면 다시 돌아오게
        _menuPanel.OnResumeButtonPressed.AddListener(MenuPanelToggle);
        // 메뉴 버튼 누르면 메뉴 패널 토글되도록
        _menuButton.onClick.AddListener(MenuPanelToggle);
    }

    private void QuitGame()
    {
        Application.Quit();
    }

    private void MenuPanelToggle()
    {
        _menuPanel.gameObject.SetActive(!_menuPanel.gameObject.activeSelf);
    }
}
