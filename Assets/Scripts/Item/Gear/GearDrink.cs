using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GearDrink : GearBase
{
    private float coolTime = 10f;
    private GameObject go;
    private GameManager gm;
    private float t;

    public override float GetProgressBarAmount()
    {
        return t / coolTime;
    }

    public override UnityEvent OnGearActivated { get; } = new();

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
            OnGearActivated?.Invoke();
        }
    }
}
