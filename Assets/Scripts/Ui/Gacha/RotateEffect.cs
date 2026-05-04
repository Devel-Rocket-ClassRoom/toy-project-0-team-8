using UnityEngine;

public class RotateEffect : MonoBehaviour
{
    private float rotateSpeed = 50f;
    void Update()
    {
        transform.Rotate(Vector3.back * rotateSpeed*Time.deltaTime);
    }

}
