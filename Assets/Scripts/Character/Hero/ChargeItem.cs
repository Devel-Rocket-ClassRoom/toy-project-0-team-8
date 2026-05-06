using UnityEngine;

public class ChargeItem : ItemBase
{
    private int stack;

    protected override float ItemDuration => 0.5f;

    protected override void ApplyItemEffect(CookieController other)
    {
        stack = other.GetComponent<HeroCookie>().ChargeStack;
        if (stack < 5)
            other.GetComponent<HeroCookie>().ChargeStack += 1;
    }

    protected override void RemoveItemEffect(CookieController other)
    {
        
    }
}
