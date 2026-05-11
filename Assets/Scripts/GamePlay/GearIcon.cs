using System;
using UnityEngine;
using UnityEngine.UI;

public class GearIcon : MonoBehaviour
{
    private Image _gearImage;

    private void Awake()
    {
        _gearImage = GetComponent<Image>();
    }

    private void Update()
    {
        // 이미지가 없으면, 하얀 박스 안나오게 함
        if (_gearImage.sprite == null)
        {
            _gearImage.color = new Color(1, 1, 1, 0);
        }
        else
        {
            _gearImage.color = new Color(1, 1, 1, 1);
        }
    }
}
