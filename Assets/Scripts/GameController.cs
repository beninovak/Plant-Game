using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    public GameObject player, cow, pumpkin;
    public GameObject playerSpawnPoint, cowSpawnPoint;
    private GameObject[] pumpkinSpawnPoints;

    [HideInInspector]
    public int pumpkinCount;
    public TextMeshProUGUI pumpkinCountText;

    public WaterController waterController;
    void Awake() {
        GameObject spawnedPlayer = Instantiate(player, playerSpawnPoint.transform.position, Quaternion.identity);
        PlayerController playerController = spawnedPlayer.GetComponent<PlayerController>();
        
        waterController.player = spawnedPlayer;
        waterController.playerController = playerController;
        
        GameObject spawnedCow = Instantiate(cow, cowSpawnPoint.transform.position, Quaternion.identity);
        CowController cowController = spawnedCow.GetComponent<CowController>();
        cowController.player = spawnedPlayer;
        cowController.playerController = playerController;
        
        pumpkinSpawnPoints = GameObject.FindGameObjectsWithTag("PumpkinSpawnPoint");
        foreach (GameObject spawnPoint in pumpkinSpawnPoints) {
            GameObject spawnedPumpkin = Instantiate(pumpkin, spawnPoint.transform.position, Quaternion.identity, spawnPoint.transform);
            spawnedPumpkin.GetComponent<PlantController>().gameController = this;
        }
    }

    public void AddPumpkin() {
        pumpkinCount++;
        pumpkinCountText.text = "Pumpkin count: " + pumpkinCount;
    }
}
