using System.Collections;
using TMPro;
using Unity.Android.Gradle;
using UnityEngine;
public class BoxEffect : MonoBehaviour
{
    public GameObject[] linePrefab;
    public int lineCount = 20;
    public float minLength = 10f;
    public float maxLength = 100f;

    private void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(RateLine());
    }

    public IEnumerator RateLine()
    {
        for (int i = 0; i < lineCount; i++)
        {
            GameObject currentLine = linePrefab[Random.Range(0, linePrefab.Length)];
            float randomZ = Random.Range(0f, 360f);
            Quaternion randomRotation = Quaternion.Euler(0, 0, randomZ);
            GameObject line = Instantiate(currentLine,transform.position,randomRotation);
            line.transform.SetParent(this.transform);
            float randomScaleX = Random.Range(minLength, maxLength);
            line.transform.localScale = new Vector3(randomScaleX, 0.5f, 1f);
            float randomTime = Random.Range(0.1f,0.3f);
            yield return new WaitForSeconds(randomTime);
        }
    }

}
