using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveScript 
{
    //private static string path = Application.persistentDataPath + "/swipeData1.dat";

    public static void SaveData(SwipeScript swipeData) {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/savedData.dat";

        FileStream stream = new FileStream(path, FileMode.Create);

        SavedData data = new SavedData(swipeData);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static SavedData LoadData() {
        string path = Application.persistentDataPath + "/savedData.dat";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SavedData data = formatter.Deserialize(stream) as SavedData;
            stream.Close();

            return data;
        }
        else {
            Debug.Log("File doesn't exists");
            return null;
        }
    }
}
