using UnityEngine;
using TMPro;
using UnityEditor;
using System.Collections;
public class BbobgiTitle : GenericWindow
{

    public Animator animator;
    public float time;
    public GameObject[] effects;
    public GameObject Treasureview;
    private bool isClick = false;
    private bool isOpen = false;
    private int count;

    private void Awake()
    {
        isClick = false;
        isOpen = false;
        time = 0;
        count= 0;
        animator.speed = 1.0f;
    }
    public override void Open()
    {
        base.Open();
        isClick = false;
        isOpen = false;
        time = 0;
        animator.speed = 1.0f;
    }
    public override void Close()
    {
        base.Close();
        isClick= false;
        isOpen= false;
    }

    private void Update()
    {
        if (isClick)
        {

            time += Time.deltaTime;
            if (time > 3)
            {
                animator.SetBool("isClick", isClick);
                isClick = false;
                isOpen = true;
                time = 0;
            }
        }
        if (isOpen)
        {
            time += Time.deltaTime;
            if (time > 1)
            {
                time= 0;
                isOpen = false;
            }
        }
    }

    public void OnClickChest()
    {
        isClick = true;
        Debug.Log("상자클릭");
        animator.speed = 2f;
        foreach (var obj in effects)
        {
            obj.SetActive(true);
        }
    }
}
