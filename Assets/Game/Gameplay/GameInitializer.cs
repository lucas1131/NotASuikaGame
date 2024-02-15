using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitializer : MonoBehaviour {

    [SerializeField] GameObject spawnerPosition;
    [SerializeField] GameConfig[] configs;
    [SerializeField] MouseController controllerPrefab; // Could load prefabs from asset database to support remote assets
    [SerializeField, Tooltip("Editor override for testing")] int selectedConfigOverride;
    [SerializeField] bool applyOverride;

    void Start(){
        StartGame();
    }

    public void StartGame(){
        var merger = new PieceMergerManager(false);
        ResetGame();
    }

    public void EndGame(){}

    public void ResetGame(){
        int selectedConfig = selectedConfigOverride;
        // IMouseController controller = new MouseController(); // for now this is a monobehaviour so we have to instantiate and setup
        MouseController controller = Instantiate<MouseController>(controllerPrefab);
        Spawner spawner = new Spawner(configs[selectedConfig], controller, spawnerPosition.transform.position);

        controller.SetSpawner(spawner);
        spawner.SpawnInitialPieces();
    }
}
