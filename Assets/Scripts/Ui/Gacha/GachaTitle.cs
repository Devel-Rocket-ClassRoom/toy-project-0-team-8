using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
public class GachaTitle : GenericWindow
{
    public int currentCrystal;
    public TextMeshProUGUI crystaltext;

    public void OnClickAddCrystal(int crystal)
    {
        currentCrystal += crystal;
        crystaltext.text = $"{currentCrystal.ToString()}";
    }
    public override void Open()
    {
        base.Open();
    }
    public override void Close()
    {
        base.Close();
    }

    public void OnClickBobbgiButton()
    {
        if (currentCrystal < 200)
        {
            Debug.Log("크리스탈 부족");
            return;
        }
        currentCrystal -= 200;
        windowManager.Open(1);
    }
}
