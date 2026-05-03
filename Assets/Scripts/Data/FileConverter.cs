using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using UnityEngine;


[Serializable]
public class Vector3Converter : JsonConverter<Vector3>
{
    // JSON -> Vector3 (역직렬화)
    public override Vector3 ReadJson(JsonReader reader, Type objectType,
        Vector3 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        Vector3 v = Vector3.zero;
        JObject jObj = JObject.Load(reader);  // JSON을 JObject로 파싱
        v.x = (float)jObj["X"];               // 키로 값 접근
        v.y = (float)jObj["Y"];
        v.z = (float)jObj["Z"];
        return v;

    }

    // Vector3 -> JSON (직렬화)
    public override void WriteJson(JsonWriter writer, Vector3 value,
        JsonSerializer serializer)
    {
        writer.WriteStartObject();       // {
        writer.WritePropertyName("X");   //   "X":
        writer.WriteValue(value.x);      //        0.0
        writer.WritePropertyName("Y");   //   "Y":
        writer.WriteValue(value.y);      //        0.0
        writer.WritePropertyName("Z");   //   "Z":
        writer.WriteValue(value.z);      //        0.0
        writer.WriteEndObject();         // }

    }
}
[Serializable]
public class QuaternionConverter : JsonConverter<Quaternion>
{
    // JSON -> Vector3 (역직렬화)
    public override Quaternion ReadJson(JsonReader reader, Type objectType,
        Quaternion existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        Quaternion v = Quaternion.identity;
        JObject jObj = JObject.Load(reader);  // JSON을 JObject로 파싱
        v.x = (float)jObj["X"];               // 키로 값 접근
        v.y = (float)jObj["Y"];
        v.z = (float)jObj["Z"];
        v.w = (float)jObj["W"];
        return v;

    }

    // Vector3 -> JSON (직렬화)
    public override void WriteJson(JsonWriter writer, Quaternion value,
        JsonSerializer serializer)
    {
        writer.WriteStartObject();       
        writer.WritePropertyName("X");   
        writer.WriteValue(value.x);      
        writer.WritePropertyName("Y");   
        writer.WriteValue(value.y);      
        writer.WritePropertyName("Z");   
        writer.WriteValue(value.z);          
        writer.WritePropertyName("W");   
        writer.WriteValue(value.w);     
        writer.WriteEndObject();    

    }
}
[Serializable]
public class ColorConverter : JsonConverter<Color>
{
    // JSON -> Vector3 (역직렬화)
    public override Color ReadJson(JsonReader reader, Type objectType,
        Color existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        Color v = Color.clear;
        JObject jObj = JObject.Load(reader);  // JSON을 JObject로 파싱
        v.r = (float)jObj["R"];               // 키로 값 접근
        v.g = (float)jObj["G"];
        v.b = (float)jObj["B"];
        v.a = (float)jObj["A"];
        return v;

    }

    // Vector3 -> JSON (직렬화)
    public override void WriteJson(JsonWriter writer, Color value,
        JsonSerializer serializer)
    {
        writer.WriteStartObject();       
        writer.WritePropertyName("R");   
        writer.WriteValue(value.r);      
        writer.WritePropertyName("G");   
        writer.WriteValue(value.g);      
        writer.WritePropertyName("B");   
        writer.WriteValue(value.b);      
        writer.WriteEndObject();         

    }
}

public class GearDataConverter : JsonConverter<GearData>
{
    public override GearData ReadJson(JsonReader reader, Type objectType, GearData existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        string id = reader.Value as string;
        return DataTableManager.GearTable.Get(id);
    }

    public override void WriteJson(JsonWriter writer, GearData value, JsonSerializer serializer)
    {
        writer.WriteValue(value.Id);
    }
}

public class CharacterDataConverter : JsonConverter<CookieData>
{
    public override CookieData ReadJson(JsonReader reader, Type objectType, CookieData existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        string id = reader.Value as string;
        return DataTableManager.CookieTable.Get(id);
    }

    public override void WriteJson(JsonWriter writer, CookieData value, JsonSerializer serializer)
    {
        writer.WriteValue(value.Id);
    }
}