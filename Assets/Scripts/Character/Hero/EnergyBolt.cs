using UnityEngine;

public class EnergyBolt : MonoBehaviour
{
    [SerializeField]
    private float _speed = 15f;

    [SerializeField]
    private float _lifeTime = 3f;

    private void Start()
    {
        Destroy(gameObject, _lifeTime);
    }

    private void Update()
    {
        transform.position += Vector3.right * _speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Tags.Obstacle))
        {
            var obstacle = other.GetComponent<Obstacle>();
            if (obstacle != null)
                obstacle.Break();
            // 관통: 자신은 파괴하지 않음
        }
    }
}
