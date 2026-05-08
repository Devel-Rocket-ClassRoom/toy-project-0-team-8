using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeScene : MonoBehaviour
{
    public Image ReadyCharacter;
    public UiGearSlot[] equipGear;

    private void OnEnable()
    {
        // 씬이 바뀔 때마다 데이터 로드
        SaveLoadManager.Load();   
    }
    public void OnGachaScene()
    {

        SceneManager.LoadScene("GachaScene");
    }
    public void OnLobbyScene()
    {
        SceneManager.LoadScene("Lobby");
    }
    public void OnReadyScene()
    {

        SceneManager.LoadScene("ReadyScene");
    }
    public void OnPlayScene()
    {
        // 누를 때 데이터 테이블에 쿠키 정보 저장
        SaveLoadManager.Save();
        SceneManager.LoadScene("PlayScene");
    }
    public void OnResultScene()
    {
        SaveLoadManager.Save();
        SceneManager.LoadScene("GameResultScene");
    }
}
