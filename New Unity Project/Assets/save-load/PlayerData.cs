using UnityEngine;
[System.Serializable]
public class PlayerData
{
    public string playerName;
    public PlayerStats playerStats;
    public Inventory playerInventory;
    public PlayerEquipment playerEquipment;

    public float[] position;
    public int sceneLocation;

    public PlayerData(Player player) {
        playerName = player.playerName;
        playerStats = player.playerStats;

        playerInventory = player.playerInventory;
        playerEquipment = player.playerEquipment;

        position = new float[3];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;
        sceneLocation = player.sceneLocation;
    }
}
