using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class JsonSavingLoading
{
    public static bool SaveData<T>(T dataClass, string filePath, string fileName)
    {
        if (!Directory.Exists(Application.persistentDataPath + "/" + filePath))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/" + filePath);
        }

        string data = JsonUtility.ToJson(dataClass);
        File.WriteAllText(Application.persistentDataPath + "/" + filePath + "/" + fileName + ".txt", data);
        return true;
    }

    public static bool LoadData<T>(out T dataClass, string filePath, string fileName)
    {
        if (Directory.Exists(Application.persistentDataPath + "/" + filePath))
        {
            if (File.Exists(Application.persistentDataPath + "/" + filePath + "/" + fileName + ".txt"))
            {
                string data = File.ReadAllText(Application.persistentDataPath + "/" + filePath + "/" + fileName + ".txt");
                dataClass = JsonUtility.FromJson<T>(data);
                return true;
            }
        }

        dataClass = default;
        return false;
    }
}
