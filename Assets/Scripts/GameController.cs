using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    public GameObject water;
    public GameObject player, cow;
    public GameObject playerSpawnPoint, cowSpawnPoint;
    void Awake() {
        GameObject spawnedPlayer = Instantiate(player, playerSpawnPoint.transform.position, Quaternion.identity);
        PlayerController playerController = spawnedPlayer.GetComponent<PlayerController>();
        
        WaterController waterController = water.GetComponent<WaterController>();
        waterController.player = spawnedPlayer;
        waterController.playerController = playerController;
        
        GameObject spawnedCow = Instantiate(cow, cowSpawnPoint.transform.position, Quaternion.identity);
        CowController cowController = spawnedCow.GetComponent<CowController>();
        cowController.player = spawnedPlayer;
        cowController.playerController = playerController;
    }
}
