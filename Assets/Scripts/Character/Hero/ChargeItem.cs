using UnityEngine;

public class ChargeItem : MonoBehaviour
{
    private int stack;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            stack = collision.gameObject.GetComponent<HeroCookie>().ChargeStack;
            if (stack < 5)
                collision.gameObject.GetComponent<HeroCookie>().ChargeStack += 1;
        }
    }
}
