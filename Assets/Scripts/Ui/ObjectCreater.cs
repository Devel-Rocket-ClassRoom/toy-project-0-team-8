using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor.Overlays;
using UnityEngine;

public class ObjectCreater : MonoBehaviour
{
    public GameObject[] prefab;

    private List<GameObject> goList = new List<GameObject>();
    private JsonTest jsonTest;

    public void Start()
    {
        jsonTest = GetComponent<JsonTest>();
    }

    public void Create()
    {
        int ran = Random.Range(1, 11);

        for (int i = 0; i <= ran; i++)
        {
            float dist = Mathf.Abs(Camera.main.transform.position.z);
            Vector3 pos = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0.1f, 0.9f), Random.Range(0.1f, 0.9f), dist));
            pos.z = 0f;

            Quaternion rot = Quaternion.Euler(Random.Range(0,360), Random.Range(0, 360), Random.Range(0, 360));

            int index = Random.Range(0, prefab.Length);
            GameObject obj = Instantiate(prefab[index], pos, rot, transform);

            goList.Add(obj);
        }
    }

    public void Clear()
    {
        goList.Clear();
        // 0번은 자동으로 뺴고 해줌
        foreach (Transform child in transform)
        {
            
            Destroy(child.gameObject);
        }
    }


    public void Save()
    {
        jsonTest.SaveAll(goList, "AllSave");
    }
    public void Load()
    {
        Clear();

        SomeClass loadedData = jsonTest.LoadAll("AllSave");

        foreach (var data in loadedData.allObjects)
        {
            
            GameObject obj = Instantiate(prefab[data.index], data.pos, data.rot, transform);
            obj.transform.localScale = data.scale;

            Renderer rend = obj.GetComponent<Renderer>();
            if (rend != null) rend.material.color = data.color;

            goList.Add(obj); 
        }
    }
}
