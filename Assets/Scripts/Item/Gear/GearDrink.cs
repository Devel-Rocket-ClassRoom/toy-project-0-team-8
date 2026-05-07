using System.Collections;
using UnityEngine;

public class GearDrink : MonoBehaviour
{
    private float radius = 2f;
    private float magnetspeed = 5f;



    private void Update()
    {
        Collider2D[] col = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach(Collider2D target in col)
        {
            ItemBase itemBase = target.GetComponent<ItemBase>();
            if(itemBase!=null)
            {
                target.transform.position = Vector3.MoveTowards(target.transform.position,transform.position,magnetspeed*Time.deltaTime);
            }
        }
    }


}
