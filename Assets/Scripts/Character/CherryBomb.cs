using System.Collections;
using UnityEngine;

public class CherryBomb : MonoBehaviour
{
    public GameObject effectPrefab;
    public GameObject jellyPrefab;
    public float jellyRadius = 1.5f;
    public float jellyAddRadius = 0.35f;
    private bool isExploded = false;

    private Coroutine bombCor;
    private void OnEnable()
    {
        isExploded = false;
        if (bombCor != null)
            StopCoroutine(bombCor);
        bombCor = StartCoroutine(Bomb());
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Hurdle")) //장애물 이름 뭘로할지 몰라 임시
        {
            Instantiate(effectPrefab, transform.position, Quaternion.identity);
            isExploded = true;
            SpawnJelly();
            Destroy(gameObject);
        }
    }
    public void SpawnJelly()
    {
        int jellycount = 12;
        int count = 0;
        while(count<3) // 젤리를 원모양으로 3번 생성 원크기가 늘어남에 따라 반지름 증가
        {
            for (int i = 0; i < jellycount; i++)
            {
                float angle = i * Mathf.PI * 2 / jellycount;
                float x = Mathf.Cos(angle) * jellyRadius;
                float y = Mathf.Sin(angle) * jellyRadius;
                Vector3 spawnPos = transform.position + new Vector3(x, y, 0);
                GameObject jelly = Instantiate(jellyPrefab, spawnPos, Quaternion.identity);
            }
            count++;
            jellycount += 4;
            jellyRadius += jellyAddRadius;
        }  

    }
    public IEnumerator Bomb() // 시간 지나면 터지도록 
    {
        yield return new WaitForSeconds(3f);
        if(!isExploded)
        {
            isExploded=true;
            if(effectPrefab != null)
            Instantiate(effectPrefab, transform.position, Quaternion.identity);
            SpawnJelly();
            Destroy(gameObject);
        }
    }

}
