using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitializer : MonoBehaviour {

    [SerializeField] GameObject spawnerPosition;
    [SerializeField] GameConfig[] configs;
    [SerializeField] MouseController controllerPrefab; // Could load prefabs from asset database to support remote assets
    [SerializeField] GameManager gameManagerPrefab; // Could load prefabs from asset database to support remote assets
    [SerializeField, Tooltip("Editor override for testing")] int configOverride;
    [SerializeField] bool applyOverride;

    MouseController controller;
    GameManager gameManager;

    void Start(){
        StartGame();
    }

    public void StartGame(){
        GameConfig config = configs[applyOverride ? configOverride : configOverride];

        // controller = new MouseController(); // for now this is a monobehaviour so we have to instantiate and setup
        controller = Instantiate<MouseController>(controllerPrefab);
        gameManager = Instantiate<GameManager>(gameManagerPrefab);

        PieceMergerManager merger = new PieceMergerManager(config.AllowTripleMerge);
        Spawner spawner = new Spawner(config, controller, merger, spawnerPosition.transform.position);

        gameManager.Setup(merger);
        controller.SetSpawner(spawner);
        spawner.SpawnInitialPieces();
    }

    public void EndGame(){}

    public void ResetGame(){}
}
