using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeScene : MonoBehaviour
{
    public Image ReadyCharacter;
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
        SceneManager.LoadScene("PlayScene");
    }
}
