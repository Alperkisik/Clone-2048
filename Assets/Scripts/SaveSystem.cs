using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveSystem : MonoBehaviour
{
    public static void SaveLevel(Level level)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/LevelData.save";
        //Debug.Log(path);

        FileStream stream = new FileStream(path, FileMode.Create);

        LevelData data = new LevelData(level);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static LevelData LoadLevel()
    {
        string path = Application.persistentDataPath + "/LevelData.save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            LevelData data = formatter.Deserialize(stream) as LevelData;
            stream.Close();

            return data;
        }
        else
        {
            return null;
        }
    }
}
