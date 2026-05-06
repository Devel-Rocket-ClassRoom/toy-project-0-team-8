using UnityEngine;

public class CoinChangeJelly : ItemBase
{
    private GameObject TransformArea;

    private void Start()
    {
        // 비활성화 된것도 찾아서 갖고오기
        TransformArea = GameObject.FindAnyObjectByType<CoinChanger>(FindObjectsInactive.Include).gameObject;
    }

    protected override float ItemDuration => 0f;

    protected override void ApplyItemEffect(CookieController other)
    {
        TransformArea.SetActive(true);
    }

}
