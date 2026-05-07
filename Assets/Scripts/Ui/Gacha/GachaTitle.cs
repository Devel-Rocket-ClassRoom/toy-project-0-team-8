using UnityEngine;
using UnityEngine.UI;   
using TMPro;
using System;
using System.Collections;
public class GachaTitle : GenericWindow
{
    public int currentCrystal;
    private int textcurrentCrystal;
    public TextMeshProUGUI crystaltext;
    public GameObject addcrystalPanal;
    public GameObject addcrystalPanalCookies;
    public GameObject enoughCrystalPanal;
    public GameObject TreasureOneButton;
    public GameObject TreasureTenButton;
    public GameObject CookiesOneButton;
    public GameObject CookiesTenButton;
    public GameObject treasureCenter;
    public GameObject cookiesCenter;
    public TextMeshProUGUI enoughCrystalText;
    private bool bbobgititleCheck = false;
    private bool gachatitleCheck = true;
    public int maxCrystal = 999999;
    private bool notenoughCheck;
    private int GachaCount;

/*    private void OnEnable()
    {
        
        currentCrystal = SaveLoadManager.Data.Cristal;
        textcurrentCrystal = currentCrystal;
        enoughCrystalPanal.SetActive(false);
        gachatitleCheck = true;
        bbobgititleCheck = false;
        crystaltext.text = textcurrentCrystal.ToString();
        cookiesCenter.SetActive(false);
        treasureCenter.SetActive(true);
    }*/
    public void OnClickAddCrystal(int crystal)
    {
        currentCrystal += crystal;
        textcurrentCrystal = Math.Clamp(currentCrystal, 0, maxCrystal);
        SaveLoadManager.Data.Cristal = currentCrystal;
        SaveLoadManager.Save();
        Debug.Log($"세이브 크리스탈 :{SaveLoadManager.Data.Cristal}");
        crystaltext.text = $"{textcurrentCrystal.ToString()}";
    }
    public void OnClickAddPanal()
    {
        if(gachatitleCheck)
        {
            addcrystalPanal.SetActive(true);
        }
        else if(bbobgititleCheck)
        {
            addcrystalPanalCookies.SetActive(true);
        }
    }
    public void OnClickExitAddPanal()
    {
        if(gachatitleCheck)
        {
            addcrystalPanal.SetActive(false);
        }
        else if(bbobgititleCheck)
        {
            addcrystalPanalCookies.SetActive(false);
        }
    }
    public override void Open()
    {
        base.Open();
        SaveLoadManager.Load();
        currentCrystal = SaveLoadManager.Data.Cristal;
        textcurrentCrystal = currentCrystal;
        crystaltext.text = textcurrentCrystal.ToString();

    }

    public override void Open(int count, int currentBbobgi)
    {
        base.Open(count, currentBbobgi);
        SaveLoadManager.Load();
        currentCrystal = SaveLoadManager.Data.Cristal;
        textcurrentCrystal = currentCrystal;
        crystaltext.text = textcurrentCrystal.ToString();
   
        enoughCrystalPanal.SetActive(false);
        if (currentBbobgi == 0)
        {
            OnClickTreasureBbobgiPanal();
        }
        else if (currentBbobgi == 1)
        {
            OnclickCookiesBbobgiPanal();
        }


           
    }
    
    public override void Close()
    {
        base.Close();
        SaveLoadManager.Data.Cristal = currentCrystal;
        Debug.Log($"현재 크리스탈 :{currentCrystal}");
        SaveLoadManager.Save();
        enoughCrystalPanal.SetActive(false);
    }

    public void OnOneClickGachaButton()
    {
        if (currentCrystal < 200)
        {
            Debug.Log("크리스탈 부족");
            StartCoroutine(NotEnoughCrystal());
            return;
        }
        currentCrystal -= 200;
        GachaCount = 1;
        textcurrentCrystal = Math.Clamp(currentCrystal, 0, maxCrystal);
        crystaltext.text = $"{textcurrentCrystal.ToString()}";
        windowManager.Open(1,GachaCount,0);
    }
    public void OnClickGachaButton()
    {
        if (currentCrystal < 2000)
        {
            Debug.Log("크리스탈 부족");
            if (notenoughCheck) return;

            StartCoroutine(NotEnoughCrystal());
            return;
        }

        currentCrystal -= 2000;
        GachaCount = 10;
        textcurrentCrystal = Math.Clamp(currentCrystal, 0, maxCrystal);
        crystaltext.text = $"{textcurrentCrystal.ToString()}";

        windowManager.Open(1, GachaCount,0);
    }
    public void OnOneClickGachaCookiesButton()
    {
        if (currentCrystal < 200)
        {
            Debug.Log("크리스탈 부족");
            StartCoroutine(NotEnoughCrystal());
            return;
        }
        currentCrystal -= 200;
        GachaCount = 1;
        textcurrentCrystal = Math.Clamp(currentCrystal, 0, maxCrystal);
        crystaltext.text = $"{textcurrentCrystal.ToString()}";
        windowManager.Open(1, GachaCount,1);
    }
    public void OnClickGachaCookieButton()
    {
        if (currentCrystal < 2000)
        {
            Debug.Log("크리스탈 부족");
            if (notenoughCheck) return;

            StartCoroutine(NotEnoughCrystal());
            return;
        }

        currentCrystal -= 2000;
        GachaCount = 10;
        textcurrentCrystal = Math.Clamp(currentCrystal, 0, maxCrystal);
        crystaltext.text = $"{textcurrentCrystal.ToString()}";

        windowManager.Open(1, GachaCount,1);
    }
    public IEnumerator NotEnoughCrystal()
    {
        enoughCrystalPanal.SetActive(false);
        enoughCrystalPanal.SetActive(true);
        notenoughCheck = true;
        float t = 0;
        while(t<1)
        {
            t += Time.deltaTime;
            enoughCrystalPanal.transform.localScale = Vector3.Lerp(Vector3.one, new Vector3(0.1f, 0.1f, 0.1f),t);
            if(Time.frameCount%5==0)
            {
                enoughCrystalText.color = UnityEngine.Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.7f, 1f);
            }
            yield return null;
        }
        notenoughCheck = false;
        enoughCrystalPanal.SetActive(false);
    }
    public void OnClickTreasureBbobgiPanal()
    {
        cookiesCenter.SetActive(false);
        addcrystalPanal.SetActive(false);
        addcrystalPanalCookies.SetActive(false);
        treasureCenter.SetActive(true);
        CookiesOneButton.SetActive(false);
        CookiesTenButton.SetActive(false);
        TreasureOneButton.SetActive(true);
        TreasureTenButton.SetActive(true);
        gachatitleCheck = true;
        bbobgititleCheck = false;
      
    }
    public void OnclickCookiesBbobgiPanal()
    {
        treasureCenter.SetActive(false);
        cookiesCenter.SetActive(true);
        addcrystalPanal.SetActive(false);
        addcrystalPanalCookies.SetActive(false);
        TreasureOneButton.SetActive(false);
        TreasureTenButton.SetActive(false);
        CookiesOneButton.SetActive(true);
        CookiesTenButton.SetActive(true);
        gachatitleCheck = false;
        bbobgititleCheck = true;
    }
}
