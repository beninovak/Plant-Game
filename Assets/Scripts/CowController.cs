using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowController : MonoBehaviour {
    public GameObject shit;
    public GameObject shitSpawnPoint;

    [HideInInspector]
    public GameObject player;
    [HideInInspector]
    public PlayerController playerController;
    
    private void Start() {
        float time = Vars.GAME_LOADED_COUNT > 0 ? 3f : 13f;
        InvokeRepeating("MakeCowShit", time, 5f);
    }

    private void MakeCowShit() {
        Vector2 offset = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        GameObject spawnedShit = Instantiate(shit, (Vector2)shitSpawnPoint.transform.position + offset, Quaternion.identity, shitSpawnPoint.transform);
        spawnedShit.SetActive(true);
        ShitController shitController = spawnedShit.GetComponent<ShitController>(); 
        shitController.shit = shit;
        shitController.player = player;
        shitController.playerController = playerController;
    }
}
