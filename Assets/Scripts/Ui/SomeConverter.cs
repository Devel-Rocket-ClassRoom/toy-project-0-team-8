using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using UnityEngine.UIElements;


public class SomeConverter : JsonConverter<SomeClass>
{
    public override SomeClass ReadJson(JsonReader reader, Type objectType, SomeClass existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        SomeClass s = new SomeClass();
        JObject jobj = JObject.Load(reader);

        s.pos = VectorConvert(reader);
        s.rot = QuternionConvert(reader);
        s.scale = VectorConvert(reader);
        s.color = ColorConverter(reader);

        return s;
    }
    public Vector3 VectorConvert(JsonReader reader)
    { 
        Vector3 v = Vector3.zero;
        JObject jObj = JObject.Load(reader);  // JSON을 JObject로 파싱
        v.x = (float)jObj["X"];               // 키로 값 접근
        v.y = (float)jObj["Y"];
        v.z = (float)jObj["Z"];
        return v;
    }
    public Quaternion QuternionConvert(JsonReader reader)
    {
        Quaternion q = Quaternion.identity;
        JObject jObj = JObject.Load(reader);  // JSON을 JObject로 파싱
        q.x = (float)jObj["X"];               // 키로 값 접근
        q.y = (float)jObj["Y"];
        q.z = (float)jObj["Z"];
        q.w = (float)jObj["W"];
        return q;
    }
    public Color ColorConverter(JsonReader reader)
    {
        Color c = Color.clear;
        JObject jObj = JObject.Load(reader); 
        c.r = (float)jObj["R"];
        c.g = (float)jObj["g"];
        c.b = (float)jObj["b"];
        c.a = (float)jObj["a"];
        return c;
    }

    public override void WriteJson(JsonWriter writer, SomeClass value, JsonSerializer serializer)
    {
    }
}
