using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    public GameObject player;
    public GameObject playerSpawnPoint;
    public GameObject water;
    void Awake() {
        GameObject spawnedPlayer = Instantiate(player, playerSpawnPoint.transform.position, Quaternion.identity);
        WaterController waterController = water.GetComponent<WaterController>();
        waterController.player = spawnedPlayer;
        waterController.playerController = spawnedPlayer.GetComponent<PlayerController>();
    }
}
