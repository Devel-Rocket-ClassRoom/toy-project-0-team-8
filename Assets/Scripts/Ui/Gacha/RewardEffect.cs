using UnityEngine;
using UnityEngine.AdaptivePerformance;

public class RewardEffect : MonoBehaviour
{
    private float rotatespeed = 5f;
    private float scaleSpeed = 3f;
    private float time = 0f;
    private float scaletime = 0f;
    private float scaleMin = 0.1f;
    private float scaleMax = 3f;

    private void OnEnable()
    {
        time = 0f;
        scaletime = 0f;
        transform.localScale = new Vector3(scaleMin, scaleMin, scaleMin);
    }

    private void Update()
    {
        scaletime += Time.deltaTime / scaleSpeed;
        time += Time.deltaTime * rotatespeed;
        transform.localScale = Vector3.Lerp(new Vector3(scaleMin, scaleMin, scaleMin),new Vector3(scaleMax, scaleMax, scaleMax),scaletime);
        transform.Rotate(Vector3.up * time);
    }
}
