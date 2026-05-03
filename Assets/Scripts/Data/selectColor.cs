using UnityEngine;

public class selectColor : MonoBehaviour
{
    public Material[] materials;

    private Renderer target;
    private void Awake()
    {
        target = GetComponent<Renderer>();
    }

    private void Start()
    {
        if (target != null && materials != null && materials.Length > 0)
        {
            target.material = materials[Random.Range(0, materials.Length)];
        }
    }
}
