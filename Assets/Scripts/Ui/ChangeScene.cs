using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void OnGachaScene()
    {

        SceneManager.LoadScene("GachaScene");
    }
    public void OnLobbyScene()
    {

        SceneManager.LoadScene("Lobby");
    }
    public void OnPlayScene()
    {

        SceneManager.LoadScene("ReadyScene");
    }
}
