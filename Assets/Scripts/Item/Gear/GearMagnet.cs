using UnityEngine;
using UnityEngine.Events;

public class GearMagnet : GearBase
{
    private float radius = 2f;
    private float magnetspeed = 15f;

    public override float GetProgressBarAmount()
    {
        return 1f;
    }

    // 자석은 별도로 "활성화" 타이밍이 없어서 그냥 킵
    public override UnityEvent OnGearActivated { get; } = new();

    private void Update()
    {
        Collider2D[] col = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (Collider2D target in col)
        {
            ItemBase itemBase = target.GetComponent<ItemBase>();
            if (itemBase != null)
            {
                target.transform.position = Vector3.MoveTowards(target.transform.position, transform.position, magnetspeed * Time.deltaTime);
            }
        }
    }
}
