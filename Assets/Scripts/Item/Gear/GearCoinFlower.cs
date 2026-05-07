using UnityEngine;

public class GearCoinFlower : MonoBehaviour
{
    public GameObject coinFlowerPrefab;
    public GameObject stageroot;
    private float cooltime = 0.3f;
    private float t = 0;
    private float weight = 80f;

    private void OnEnable()
    {
        t = 0;
        stageroot = GameObject.FindWithTag(Tags.StageRoot);
    }
    private void Update()
    {
        t += Time.deltaTime;
        if (t > cooltime)
        {
            t = 0;
            float randomW = Random.Range(0, 100);
            if (randomW < weight)
            {
                Instantiate(coinFlowerPrefab,new Vector3(transform.position.x+6f,transform.position.y,0f),Quaternion.identity,stageroot.transform);
            }
        }
    }
}
