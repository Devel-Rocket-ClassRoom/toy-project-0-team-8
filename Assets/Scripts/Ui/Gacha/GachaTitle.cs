using UnityEngine;
using UnityEngine.UI;   
using TMPro;
using System;
using System.Collections;
public class GachaTitle : GenericWindow
{
    public long currentCrystal;
    private long textcurrentCrystal;
    public TextMeshProUGUI crystaltext;
    public GameObject addcrystalPanal;
    public GameObject enoughCrystalPanal;
    public GameObject TreasureOneButton;
    public GameObject TreasureTenButton;
    public GameObject CookiesOneButton;
    public GameObject CookiesTenButton;
    public GameObject treasureCenter;
    public GameObject cookiesCenter;
    public TextMeshProUGUI enoughCrystalText;
    public long maxCrystal = 99999999999;
    private bool notenoughCheck;
    private int GachaCount;

    private void Awake()
    {
        textcurrentCrystal = currentCrystal;
        enoughCrystalPanal.SetActive(false);
        crystaltext.text = textcurrentCrystal.ToString();
        cookiesCenter.SetActive(false);
        treasureCenter.SetActive(true);
    }
    public void OnClickAddCrystal(int crystal)
    {
        currentCrystal += crystal;
        textcurrentCrystal = Math.Clamp(currentCrystal, 0, maxCrystal);
        crystaltext.text = $"{textcurrentCrystal.ToString()}";
    }
    public void OnClickAddPanal()
    {
        addcrystalPanal.SetActive(true);
    }
    public void OnClickExitAddPanal()
    {
        addcrystalPanal.SetActive(false);
    }
    public override void Open()
    {
        base.Open();
        enoughCrystalPanal.SetActive(false);
    }
    public override void Close()
    {
        base.Close();
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
        windowManager.Open(1,GachaCount);
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
        
        windowManager.Open(1,GachaCount);
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
        treasureCenter.SetActive(true);
        CookiesOneButton.SetActive(false);
        CookiesTenButton.SetActive(false);
        TreasureOneButton.SetActive(true);
        TreasureTenButton.SetActive(true);
      
    }
    public void OnclickCookiesBbobgiPanal()
    {
        treasureCenter.SetActive(false);
        cookiesCenter.SetActive(true);
        TreasureOneButton.SetActive(false);
        TreasureTenButton.SetActive(false);
        CookiesOneButton.SetActive(true);
        CookiesTenButton.SetActive(true);
        
    }
}
