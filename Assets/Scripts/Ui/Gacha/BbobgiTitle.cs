using UnityEngine;
using TMPro;
using UnityEditor;
public class BbobgiTitle : GenericWindow
{

    public Animator animator;
    public float time;

    private bool isClick = false;

    private void Awake()
    {
        isClick = false;
        time = 0;
        animator.speed = 1.0f;
    }
    public override void Open()
    {
        base.Open();
    }
    public override void Close()
    {
        base.Close();
    }

    private void Update()
    {
        if(isClick)
        {
            
            time += Time.deltaTime;
            if (time > 3)
            {
                animator.SetBool("isClick", isClick);
                isClick = false;
                time = 0;
            }
        }
    }
    public void OnClickChest()
    {
        isClick = true;
        Debug.Log("상자클릭");
        animator.speed = 2f;
    }
}
