using UnityEngine;

public class LobbyBoxEffect : MonoBehaviour
{
    public GameObject line;
    private int lineCount = 8;
    private float minlan = 5f;
    private float maxlan = 10f;

    private void Start()
    {
        for (int i = 0; i < lineCount; i++)
        {
            float currentZ = i * (360 / lineCount);
            Quaternion currentQuat = Quaternion.Euler(0,0,currentZ);
            GameObject currentLine = Instantiate(line, transform.position, currentQuat,transform);
            float randomScaleX = Random.Range(minlan,maxlan);
            float randomScaleY = Random.Range(0.5f, 2f);
            currentLine.transform.localScale = new Vector3(randomScaleX, randomScaleY, 1f);
        }
    }
}
