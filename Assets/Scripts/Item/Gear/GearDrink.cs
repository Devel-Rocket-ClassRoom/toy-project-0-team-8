using System.Collections;
using UnityEngine;

public class GearDrink : MonoBehaviour
{
    private float coolTime = 10f;
    private GameObject go;
    private GameManager gm;
    private float t;
    private void OnEnable()
    {
        go = GameObject.FindWithTag(Tags.GameManager);
        if (go != null)
        {
            gm = go.GetComponent<GameManager>();
        }
        t = 0;
    }
    private void Update()
    {
        t += Time.deltaTime;
        if (t > coolTime)
        {
            gm.ActivateDash(2f);
            t = 0;
        }

    }

}
