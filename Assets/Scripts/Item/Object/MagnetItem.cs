using System.Collections;
using UnityEngine;

public class MagnetItem : MonoBehaviour
{
    public float duration = 3f;
    public float distance = 5f;

    public IEnumerator SetActiveItem()
    {
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime / duration;
            GameObject gameObject = GetComponent<GameObject>();
            if (gameObject.CompareTag("Jelly"))
            {
                gameObject.transform.position = Vector3.Lerp(new Vector3(distance, distance, distance), transform.position, t);
            }
            yield return null;
        }
    }
}
