using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
public class GachaTitle : GenericWindow
{
    public long currentCrystal;
    private long textcurrentCrystal;
    public TextMeshProUGUI crystaltext;
    public GameObject addcrystalPanal;
    public long maxCrystal = 99999999999;
    private int GachaCount;

    private void Awake()
    {
        textcurrentCrystal = currentCrystal;
        crystaltext.text = textcurrentCrystal.ToString();
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
    }
    public override void Close()
    {
        base.Close();
    }

    public void OnOneClickGachaButton()
    {
        if (currentCrystal < 200)
        {
            Debug.Log("크리스탈 부족");
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
            return;
        }
        currentCrystal -= 2000;
        GachaCount = 10;
        textcurrentCrystal = Math.Clamp(currentCrystal, 0, maxCrystal);
        crystaltext.text = $"{textcurrentCrystal.ToString()}";
        windowManager.Open(1,GachaCount);
    }
}
