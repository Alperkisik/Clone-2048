using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveSystem : MonoBehaviour
{
    public static void SaveGame(GameData gameData)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/LevelData.save";

        FileStream stream = new FileStream(path, FileMode.Create);

        GameDatas data = new GameDatas(gameData);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static GameDatas LoadGameDatas()
    {
        string path = Application.persistentDataPath + "/LevelData.save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GameDatas data = formatter.Deserialize(stream) as GameDatas;

            return data;
        }
        else
        {
            return null;
        }
    }
}
