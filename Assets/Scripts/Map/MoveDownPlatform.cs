using UnityEngine;

public class MoveDownPlatform : MonoBehaviour
{
    private float moveSpeed;
    private float time = 0f;
    private float maxY = 0f;
    private float firstY;
    private bool downCheck = true;

    private void OnEnable()
    {
        firstY = transform.position.y;
        downCheck = true;
        time = Random.value;
        moveSpeed = Random.Range(0f, 5f);
    }

    private void Update()
    {
        time += Time.deltaTime / moveSpeed * moveSpeed;
        if (downCheck)
        {
            float y = Mathf.Lerp(firstY, maxY, time);
            transform.position = new Vector3(transform.position.x, y, 0);
        }
        else if (!downCheck)
        {
            float y = Mathf.Lerp(maxY, firstY, time);
            transform.position = new Vector3(transform.position.x, y, 0);
        }
        if (transform.position.y <= -maxY)
        {
            downCheck = false;
            time = 0f;
        }
        else if (transform.position.y >= firstY)
        {
            downCheck = true;
            time = 0f;
        }
    }
}
