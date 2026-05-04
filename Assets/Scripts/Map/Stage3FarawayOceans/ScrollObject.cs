using UnityEngine;

public class ScrollObject : MonoBehaviour
{
    public float _screenWidth = 32.4f;
    public float _width = 32.4f;

    [SerializeField]
    private float _speed = 10f;

    private float _xLoc => _width;

    // Update is called once per frame
    private void Update()
    {
        if (transform.position.x <= -(_screenWidth * 0.5 + _width * 0.5))
        {
            transform.position = new Vector3(_xLoc, 0);
        }

        transform.position += Vector3.left * _speed * Time.deltaTime;
    }
}
