using UnityEngine;

public class JellyChanger : MonoBehaviour
{

    public Sprite GomJelly;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Jelly>() != null)
        {
            var go = collision.gameObject;
            ChangeJelly(go);
        }
    }

    private void ChangeJelly(GameObject jelly)
    {
        Debug.Log("변환");
        jelly.GetComponent<SpriteRenderer>().sprite = GomJelly;
        jelly.transform.localScale = Vector3.one;
    }
}
