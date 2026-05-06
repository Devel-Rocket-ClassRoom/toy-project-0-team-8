using UnityEngine;

public class CoinChanger : MonoBehaviour
{
    public GameObject coinPrefab; // 생성할 코인 프리팹
    public float coinSpacing = 0.5f; // 코인 간의 간격 (코인 크기에 맞게 조절)

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            ConvertToCoins(collision);
        }
    }
    public void ConvertToCoins(Collider2D obstarcle)
    {

        Bounds bounds = obstarcle.bounds;

        // 콜라이더의 좌측 하단(최솟값)과 우측 상단(최댓값) 좌표
        float minX = bounds.min.x;
        float minY = bounds.min.y;
        float maxX = bounds.max.x;
        float maxY = bounds.max.y;

        float halfSpacing = coinSpacing / 2f;

        // 가로축(X) 반복문
        for (float x = minX + halfSpacing; x <= maxX; x += coinSpacing)
        {
            // 세로축(Y) 반복문
            for (float y = minY + halfSpacing; y <= maxY; y += coinSpacing)
            {
                Vector3 spawnPosition = new Vector3(x, y, 0);
                Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
            }
        }

        // 애니메이션 넣기
        Destroy(obstarcle.gameObject);
    }
}
