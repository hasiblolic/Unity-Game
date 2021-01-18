using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SavePlayer(Player player) {
        BinaryFormatter formatter = new BinaryFormatter();

        // creating a new file using unity's persitantdatapath
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");

        PlayerData data = new PlayerData(player);

        // serializing data from player into new file
        formatter.Serialize(file, data);

        file.Close();
    }
    
    public static PlayerData LoadPlayer() {
        // check if a save file already exists
        if(File.Exists(Application.persistentDataPath + "/gamesave.save")) {
            // creating a new stream and opening the save file
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(Application.persistentDataPath + "/gamesave.save", FileMode.Open);

            // deserializing the save file and returning the save as PlayerData
            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();
            return data;
        } else {
            Debug.LogError("Save file not found!");
            return null;
        }
    }
}
