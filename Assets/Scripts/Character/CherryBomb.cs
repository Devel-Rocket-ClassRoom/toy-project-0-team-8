using System.Collections;
using UnityEngine;

public class CherryBomb : MonoBehaviour
{
    // 장애물 파괴 시에 사용할 hash값 등록
    private static readonly int _break = Animator.StringToHash("Break");
    public GameObject effectPrefab;
    public GameObject jellyPrefab;
    public GameObject stageroot;
    public float jellyRadius = 1.5f;
    public float jellyAddRadius = 0.35f;
    private float splashRadius = 2f;
    private bool isExploded = false;
    public AudioSource bombAudio;
    private Coroutine bombCor;
    private void OnEnable()
    {
        isExploded = false;
        if (bombCor != null)
            StopCoroutine(bombCor);
        bombAudio = GetComponent<AudioSource>();
        stageroot = GameObject.FindWithTag(Tags.StageRoot);
        bombCor = StartCoroutine(Bomb());
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle")) // 시간이 안지나도 장애물 닿을시 파괴 
        {
            Effect();
            isExploded = true;
            SpawnJelly();
            AudioSource.PlayClipAtPoint(bombAudio.clip, transform.position);
            Collider2D[] obstacles = Physics2D.OverlapCircleAll(transform.position,splashRadius);  // 범위 내에 장애물들이 있는지 확인
            foreach (var obstacle in obstacles)
            {
                if(obstacle.CompareTag("Obstacle"))
                {
                    obstacle.GetComponent<Animator>().SetTrigger(_break); // 장애물이 있으면 부숨
                }

            }
            Destroy(gameObject);
        }
    }
    public void SpawnJelly()
    {
        int jellycount = 12;
        int count = 0;
        while(count<3) // 젤리를 원모양으로 3번 생성 젤리가 늘어남에 따라 반지름 증가
        {
            for (int i = 0; i < jellycount; i++)
            {
                float angle = i * Mathf.PI * 2 / jellycount;
                float x = Mathf.Cos(angle) * jellyRadius;
                float y = Mathf.Sin(angle) * jellyRadius;
                Vector3 spawnPos = transform.position + new Vector3(x, y, 0);
                GameObject jelly = Instantiate(jellyPrefab, spawnPos, Quaternion.identity,stageroot.transform);
            }
            count++;
            jellycount += 4;
            jellyRadius += jellyAddRadius;
        }  

    }
    public IEnumerator Bomb() // 시간 지나면 터지도록 
    {
        yield return new WaitForSeconds(1.5f);
        if(!isExploded)
        {
            isExploded =true;
            if (effectPrefab != null)
                Effect();
            SpawnJelly();
            AudioSource.PlayClipAtPoint(bombAudio.clip, transform.position);
            Collider2D[] obstacles = Physics2D.OverlapCircleAll(transform.position, splashRadius);
            foreach (var obstacle in obstacles)
            {
                if (obstacle.CompareTag("Obstacle"))
                {
                    Destroy(obstacle.gameObject);
                }

            }
            Destroy(gameObject);
        }
    }
    public void Effect()
    {
        GameObject eft = Instantiate(effectPrefab, transform.position, Quaternion.identity);
        eft.transform.parent = stageroot.transform;
        Destroy(eft,3f);
    }

}
