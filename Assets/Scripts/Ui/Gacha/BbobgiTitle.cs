using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.UIElements;
using SaveDataVC = SaveDataV1;

public class BbobgiTitle : GenericWindow
{

    public Animator animator;
    public float time;
    private float effcetTime = 0;
    private float effectScaleSpeed = 5f;
    public GameObject effects;
    public GameObject Treasureview;
    public GameObject TreasureviewOne;
    public GameObject rewardPrefab;
    public GameObject exitbutton;
    public ParticleSystem effectParticle;
    public Transform contentArea;
    private List<Gacha> rewardList;
    public GachaManager gachaManager;
    private bool isClick = false;
    private bool isOpen = false;
    private float effectscalemin = 1f;
    private float effectscaleMax = 10f;
    private int count;
    private bool rewardCheck = false;
    private bool firstCheck = false;
    private bool exitable = false;
    private void Awake()
    {
        rewardList = new List<Gacha>();

        isClick = false;
        isOpen = false;
        rewardCheck = false;
        firstCheck = false;
        exitable = false;
        time = 0;
        count= 0;
        animator.speed = 1.0f;
        Treasureview.SetActive(false);
        TreasureviewOne.SetActive(false);
        effects.SetActive(false);
    }
    public override void Open()
    {
        base.Open();
        Treasureview.SetActive(false);
        TreasureviewOne.SetActive(false);
        effects.SetActive(false);
        exitbutton.SetActive(false);
        isClick = false;
        isOpen = false;
        rewardCheck = false;
        firstCheck = false;
        exitable = false;
        time = 0;
        animator.speed = 1.0f;
    }
    public override void Open(int count)
    {
        base.Open(count);
        Treasureview.SetActive(false);
        TreasureviewOne.SetActive(false);
        effects.SetActive(false);
        exitbutton.SetActive(false);
        isClick = false;
        isOpen = false;
        rewardCheck = false;
        firstCheck = false;
        exitable = false;
        time = 0;
        animator.speed = 1.0f;
        this.count = count;

    }
    public override void Close()
    {
        base.Close();
        isClick= false;
        isOpen= false;
        rewardCheck= false;
        firstCheck = false;
        exitable = false;
        Treasureview.SetActive(false);
        TreasureviewOne.SetActive(false);
        effects.SetActive(false);
        exitbutton.SetActive(false);
        
    }

    private void Update()
    {
        // 상자 클릭 시 효과 재생
        if (isClick)
        {
            effcetTime += Time.deltaTime/effectScaleSpeed;
            effects.transform.localScale = Vector3.Lerp(new Vector3(effectscalemin, effectscalemin, effectscalemin), new Vector3(effectscaleMax, effectscaleMax, effectscaleMax), effcetTime);
            time += Time.deltaTime;
            if (time > 3)
            {
                animator.SetBool("isClick", isClick);
                isClick = false;
                isOpen = true;
                time = 0;
            }
        }

        // 상자가 열리는 연출
        if (isOpen)
        {
            time += Time.deltaTime;
            if (time > 1)
            {
                time= 0;
                rewardCheck = true;
                isOpen = false;
            }
        }

        // 보상 획득
        if(rewardCheck)
        {
            Reward();
            if(count==10)
            {
                // 10회 뽑기 시 연출 정지
                effects.GetComponent<Animator>().speed = 0;
            }

            effectParticle.gameObject.SetActive(false);
            StartCoroutine(UpdateSlot());
            rewardCheck = false;
        }

    }

    public void OnClickChest()
    {
        if(firstCheck)
        {
            return;
        }

        firstCheck = true;
        isClick = true;
        Debug.Log("상자클릭");
        animator.speed = 2f;
        effcetTime = 0;
        effects.SetActive(true);
        effects.GetComponent<Animator>().speed = 1;

        effectParticle.gameObject.SetActive(true);

    }
    public IEnumerator Rewards()
    {
        yield return null;
    }
    public void Reward()
    {
        rewardList.Clear();

        Treasureview.SetActive(false);
        TreasureviewOne.SetActive(false);

        if(count == 1)
        {
           TreasureviewOne.SetActive(true);
           TreasureviewOne.transform.localScale = new Vector3(3f, 3f, 3f);
           rewardList.Add(gachaManager.GachaItem());
        }
        else if (count <= 10)
        {
            Treasureview.SetActive(true);
            for (int i = 0; i < count; i++)
            {
                rewardList.Add(gachaManager.GachaItem());
            }
        }

        // 리워드 리스트 json으로 저장
        SaveReward();
    }
    public void SaveReward()
    {
        List<SaveGear> list = new List<SaveGear>();

        for (int i = 0; i < rewardList.Count; i++)
        {
            SaveGear newGear = new SaveGear();
            newGear.GearData = DataTableManager.GearTable.Get(rewardList[i].itemId);

            list.Add(newGear);

        }

        SaveLoadManager.Data = new SaveDataVC();
        SaveLoadManager.Data.GearList = list;

        SaveLoadManager.Save(1);
    }
    public void OnClickExit()
    {
        if(exitable)
        {
            windowManager.Open(0);
        }
        else
        {
            return;
        }
    }
    public IEnumerator UpdateSlot()
    {
        foreach (Transform child in contentArea)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < rewardList.Count; i++)
        {
            GameObject go = Instantiate(rewardPrefab,contentArea);
            go.SetActive(true); // 나중에 바꿀예정 테스트용
            go.GetComponent<Image>().sprite = rewardList[i].Icon;
            yield return new WaitForSeconds(0.5f);
        }
        exitable = true;
        exitbutton.SetActive(true);
    }
}
