using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public static class Storage
{
    private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
    {
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        TypeNameHandling = TypeNameHandling.All
    };
    
    public static void Save<T>(string filePath, T records)
    {
        var j = JsonConvert.SerializeObject(records, Settings);
        File.WriteAllText(filePath, j);
    }

    public static T Load<T>(string filePath) where T : new()
    {
        if (!File.Exists(filePath))
            return new T();
        
        var str = File.ReadAllText(filePath);
        return JsonConvert.DeserializeObject<T>(str, Settings);
    }

    public static T LoadConfig<T>(string fileName)
    {
        var str = Resources.Load<TextAsset>(fileName).text;
        return JsonConvert.DeserializeObject<T>(str, Settings);
    }
}