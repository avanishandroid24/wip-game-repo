using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.IO;
public static class FileHandler
{
    public static void SaveToJson<T>(List<T> toSave, string filename)
	{        
        string content = JsonHelper.ToJson<T>(toSave.ToArray(), true);
        WriteFile(GetPath(filename), content);
        Debug.Log("Saved to " + GetPath(filename));
	}

    public static void SaveToJson(string toSave, string filename)
	{
        WriteFile(GetPath(filename), toSave);
	}

    public static string GetPath(string filename)
	{
        return Application.persistentDataPath + "/" + filename;
	}

    public static List<T> ReadFromJson<T>(string filename)
	{
        string content = ReadFile(GetPath(filename));
        if(string.IsNullOrEmpty(content) || content == "{}")
		{
            return new List<T>();
		}

        List<T> res = JsonHelper.FromJson<T>(content).ToList();
        return res;
	}

    public static void WriteFile(string path, string content)
	{
        FileStream filestream = new FileStream(path, FileMode.Create);

        using(StreamWriter writer = new StreamWriter(filestream))
		{
            writer.Write(content);
		}
	}

    public static string ReadFile(string path)
	{
        if(File.Exists(path))
		{
            using(StreamReader reader = new StreamReader(path))
			{
                string content = reader.ReadToEnd();
                return content;
			}
		}

        return "";
	}
}

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}
