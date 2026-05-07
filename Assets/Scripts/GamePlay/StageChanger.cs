using System.Collections;
using UnityEngine;

public class StageChanger : MonoBehaviour
{
    public int currentStage = 0;
    private float waitTime = 3f;
    private GameManager gameManager;
    private void OnEnable()
    {
        waitTime = 3f;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            WaitChangeStage();
        }
    }

    public IEnumerator WaitChangeStage()
    {

        yield return new WaitForSeconds(waitTime);
    }
}
