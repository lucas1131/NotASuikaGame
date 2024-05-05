using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] GameObject rightWall;
    [SerializeField] GameObject leftWall;
    [SerializeField] GameObject spawnerPosition;
    [SerializeField] DeathPlane deathPlane;
    [SerializeField] GameConfig[] configs;

    [SerializeField] PrefabsLibrary prefabLibrary; // Could load SO from addressables to support remote assets
    [SerializeField] MouseController controllerPrefab; // Could load prefabs from addressables to support remote assets
    [SerializeField] GameManager gameManagerPrefab; // Could load prefabs from addressables to support remote assets

    [SerializeField, Tooltip("Editor override for testing")] int configOverride;
    [SerializeField] bool applyOverride;

    MouseController controller;
    GameManager gameManager;

    void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        GameConfig config = configs[applyOverride ? configOverride : configOverride];

        controller = Instantiate<MouseController>(controllerPrefab);
        gameManager = Instantiate<GameManager>(gameManagerPrefab);

        ILogger logger = LoggerFactory.Create();
        IObjectInstantiator instantiator = new ObjectInstantiator();
        IViewControllerFactory vcFactory = new ViewControllerFactory(instantiator, prefabLibrary);
        IRng rng = new Rng();
        IPieceMerger merger = new PieceMerger(config, logger);
        ISpawner spawner = new Spawner(config, controller, merger, vcFactory, rng, spawnerPosition.transform.position);

        gameManager.Setup(spawner);
        controller.Setup(spawner, deathPlane, leftWall, rightWall);
        spawner.SpawnInitialPieces();
    }

    public void EndGame() { }

    public void ResetGame() { }
}
