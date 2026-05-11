using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private float moveSpeed;
    private float time = 0f;
    private float maxY = 3f;
    private float firstY;
    private bool highCheck = false;

    private void OnEnable()
    {
        firstY = transform.position.y;
        highCheck = false;
        time = Random.value;
        moveSpeed = Random.Range(0f, 5f);
    }

    private void Update()
    {
        time += Time.deltaTime / moveSpeed * moveSpeed;
        if (!highCheck)
        {
            float y = Mathf.Lerp(firstY, maxY, time);
            transform.position = new Vector3(transform.position.x, y, 0);
        }
        else if (highCheck)
        {
            float y = Mathf.Lerp(maxY, firstY, time);
            transform.position = new Vector3(transform.position.x, y, 0);
        }
        if (transform.position.y >= maxY)
        {
            highCheck = true;
            time = 0f;
        }
        else if (transform.position.y <= firstY)
        {
            highCheck = false;
            time = 0f;
        }
    }
}
