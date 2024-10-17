using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    public GameObject player, cow, pumpkin;
    public GameObject playerSpawnPoint, cowSpawnPoint;
    private GameObject[] pumpkinSpawnPoints;

    [HideInInspector]
    public int pumpkinCount;
    public TextMeshProUGUI pumpkinCountText;
    public WaterController waterController;

    public Button cashOutButton;
    
    public Image finalMessageTextBg;
    public TextMeshProUGUI finalMessageText;
    public GameObject startingPanel;
    
    
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
        
        if (Vars.GAME_LOADED_COUNT < 0) { // TODO - change to == 0
            startingPanel.SetActive(true);
            Invoke("HidePanel", 10f);
        } else {
            Invoke("HideText", 1f);
        }
        
        Vars.GAME_LOADED_COUNT++;
    }

    private void HidePanel() {
        startingPanel.SetActive(false);
        Invoke("HideText", 1f);
    }
    
    private void HideText() {
        finalMessageText.enabled = false;
        finalMessageTextBg.enabled = false;
    }
    
    public void AddPumpkin() {
        pumpkinCount++;
        pumpkinCountText.text = "Pumpkin count: " + pumpkinCount;
    }
    
    public void CashOut() {
        if (Mathf.Abs(pumpkinCount) == 0) {
            finalMessageText.enabled = true;
            finalMessageTextBg.enabled = true;
            finalMessageText.text = "Seprav cash-ov boš out predn sploh veš o čem se gre game...Ne sj ne zamerm. kreten";
            Invoke("RestartGame", 5f);
            return;
        }

        string path = "C:\\Games\\Plant Game\\Assets\\Scripts\\HighScore.txt";
        StreamReader sr = new StreamReader(path);
        string line = sr.ReadLine();
        int previousBest = int.Parse(line);
        sr.Close();

        finalMessageText.enabled = true;
        finalMessageTextBg.enabled = true;
        if (pumpkinCount > previousBest) {
            finalMessageText.text = "Gz buddy. You beat your last score. Now go touch fucking grass or smth alneki idk...";
            File.WriteAllText(path, pumpkinCount.ToString());
        } else if (pumpkinCount < previousBest) {
            finalMessageText.text = "Porazno model...nisi mogu zbolšat score-a u fucking pumpkin farmerju. Js bse zamislu.";
        } else {
            finalMessageText.text = "Upam da je blo vredn farmat za isti score k prej. Must be proud!";
        }
        
        Invoke("RestartGame", 5f);
    }

    public void RestartGame() {
        SceneManager.LoadScene("MainScene");
    }
}
