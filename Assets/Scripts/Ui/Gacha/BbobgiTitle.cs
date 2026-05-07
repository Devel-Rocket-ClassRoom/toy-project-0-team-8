using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    private List<GachaGear> rewardGearList;
    private List<GachaCookie> rewardCookieList;
    public GachaManager gachaManager;
    private bool isClick = false;
    private bool isOpen = false;
    private float effectscalemin = 1f;
    private float effectscaleMax = 10f;
    private int count;
    private bool rewardCheck = false;
    private bool firstCheck = false;
    private bool exitable = false;
    private bool escCheck = false;
    private bool escdoubleCheck = false;
    private int howTitle;

    private Coroutine cor;
    private void Awake()
    {
        rewardGearList = new List<GachaGear>();
        rewardCookieList = new List<GachaCookie>();

        isClick = false;
        isOpen = false;
        rewardCheck = false;
        firstCheck = false;
        exitable = false;
        escCheck = false;
        escdoubleCheck = false;
        time = 0;
        count= 0;
        animator.speed = 1.0f;
        Treasureview.SetActive(false);
        TreasureviewOne.SetActive(false);
        effects.SetActive(false);
    }
    private void OnEnable()
    {
        howTitle = -1;
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
        escCheck = false;
        escdoubleCheck = false;
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
        escCheck = false;
        escdoubleCheck = false;
        time = 0;
        animator.speed = 1.0f;
        this.count = count;

    }
    public override void Open(int count, int currentBbobgi)
    {
        base.Open(count, currentBbobgi);
        howTitle = currentBbobgi;
        Treasureview.SetActive(false);
        TreasureviewOne.SetActive(false);
        effects.SetActive(false);
        exitbutton.SetActive(false);
        isClick = false;
        isOpen = false;
        rewardCheck = false;
        firstCheck = false;
        exitable = false;
        escCheck = false;
        escdoubleCheck = false;
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
        howTitle = -1;
        
    }

    private void Update()
    {
        if (escCheck)
        {
            return;
        }
        // 상자 클릭 시 효과 재생
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (escdoubleCheck)
            {

                StopCoroutine(cor);
                cor = null;
                rewardCheck = false;
                escdoubleCheck = false;
                escCheck = true;
                SkipReward();
                return;

            }
            escdoubleCheck = true;
            effects.transform.localScale = new Vector3(effectscaleMax, effectscaleMax, effectscaleMax);
            rewardCheck = true;
        }
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
            if (howTitle == 0)
            {
                Reward();
                if (count == 10)
                {
                    // 10회 뽑기 시 연출 정지
                    effects.GetComponent<Animator>().speed = 0;
                }

                effectParticle.gameObject.SetActive(false);
                cor = StartCoroutine(UpdateSlot());
                rewardCheck = false;
            }
            else if (howTitle == 1)
            {
                CookieReward();
                if(count == 10)
                {
                    effects.GetComponent <Animator>().speed = 0;
                }
                effectParticle.gameObject.SetActive(false);
                cor = StartCoroutine(UpdateSlotCookie());
                rewardCheck = false;
            }
 
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
        // 쿠키 분리하던지 여기서 자체 분기 하던지
        rewardGearList.Clear();

        Treasureview.SetActive(false);
        TreasureviewOne.SetActive(false);

        if(count == 1)
        {
           TreasureviewOne.SetActive(true);
           TreasureviewOne.transform.localScale = new Vector3(3f, 3f, 3f);
            rewardGearList.Add(gachaManager.GachaItem());
        }
        else if (count <= 10)
        {
            Treasureview.SetActive(true);
            for (int i = 0; i < count; i++)
            {
                rewardGearList.Add(gachaManager.GachaItem());
            }
        }

        // 리워드 리스트 json으로 저장
        SaveGearReward();
    }
    
    public void CookieReward()
    {
        // 쿠키 분리하던지 여기서 자체 분기 하던지
        rewardCookieList.Clear();

        Treasureview.SetActive(false);
        TreasureviewOne.SetActive(false);

        if(count == 1)
        {
           TreasureviewOne.SetActive(true);
           TreasureviewOne.transform.localScale = new Vector3(3f, 3f, 3f);
            rewardCookieList.Add(gachaManager.GachaCookies());
        }
        else if (count <= 10)
        {
            Treasureview.SetActive(true);
            for (int i = 0; i < count; i++)
            {
                rewardCookieList.Add(gachaManager.GachaCookies());
            }
        }

        // 리워드 리스트 json으로 저장
        SaveCookieReward();
    }
    
    public void SaveGearReward()
    {
        foreach (GachaGear gear in rewardGearList)
        {
            string gearId = gear.itemId;

            if (SaveLoadManager.Data.GearList.ContainsKey(gearId))
            {
                SaveLoadManager.Data.GearList[gearId]++;
            }
        }

        SaveLoadManager.Save();
    }
    public void SaveCookieReward()
    {

        foreach (GachaCookie cookie in rewardCookieList)
        {
            string cookieId = cookie.cookieId;

            if (SaveLoadManager.Data.CookieList.ContainsKey(cookieId))
            {
                SaveLoadManager.Data.CookieList[cookieId]++;
            }
        }

        SaveLoadManager.Save();
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
        for (int i = 0; i < rewardGearList.Count; i++)
        {
            GameObject go = Instantiate(rewardPrefab,contentArea);
            go.SetActive(true); // 나중에 바꿀예정 테스트용
            go.transform.GetChild(0).GetComponent<Image>().sprite = rewardGearList[i].Icon;
            yield return new WaitForSeconds(0.5f);
        }
        cor = null; ;
        exitable = true;
        exitbutton.SetActive(true);
    }

    public IEnumerator UpdateSlotCookie()
    {
        Debug.Log("쿠키뽑기 업데이트");
        foreach (Transform child in contentArea)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < rewardCookieList.Count; i++)
        {
            GameObject go = Instantiate(rewardPrefab, contentArea);
            go.SetActive(true); // 나중에 바꿀예정 테스트용
            go.transform.GetChild(0).GetComponent<Image>().sprite = rewardCookieList[i].Icon;
            yield return new WaitForSeconds(0.5f);
        }
        cor = null; ;
        exitable = true;
        exitbutton.SetActive(true);
    }
    public void SkipReward()
    {
        if (howTitle == 0)
        {
            int currentcount = contentArea.childCount;
            for(int i = currentcount; i<rewardGearList.Count;i++)
            {
                GameObject go = Instantiate(rewardPrefab, contentArea);
                go.SetActive(true);
                go.transform.GetChild(0).GetComponent<Image>().sprite = rewardGearList[i].Icon;
            }
        }
        else if (howTitle == 1)
        {
            int currentcount = contentArea.childCount;
            for(int i = currentcount; i<rewardCookieList.Count;i++)
            {
                GameObject go = Instantiate(rewardPrefab, contentArea);
                go.SetActive(true);
                go.transform.GetChild(0).GetComponent<Image>().sprite = rewardCookieList[i].Icon;
            }
          
        }
        exitable = true;
        exitbutton.SetActive(true);
    }
}
