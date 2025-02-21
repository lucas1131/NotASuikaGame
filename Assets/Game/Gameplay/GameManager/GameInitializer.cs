using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] Canvas viewCanvas;
    [SerializeField] GameObject rightWall;
    [SerializeField] GameObject leftWall;
    [SerializeField] GameObject spawnerPosition;
    [SerializeField] DeathPlane deathPlane;
    [SerializeField] ScoreView scoreViewPrefab;
    [SerializeField] GameConfigLibrary configsLibrary;

    [SerializeField] ScoreView endGameScreen;
    
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
        endGameScreen.SetActive(false);
        GameConfig config = configsLibrary.GetConfig(applyOverride ? configOverride : configsLibrary.selectedConfigIndex);

        controller ??= Instantiate<MouseController>(controllerPrefab);
        gameManager ??= Instantiate<GameManager>(gameManagerPrefab);

        ILogger logger = LoggerFactory.Create();
        IObjectInstantiator instantiator = new ObjectInstantiator();
        IViewControllerFactory vcFactory = new ViewControllerFactory(instantiator, prefabLibrary, viewCanvas, scoreViewPrefab);
        IScoreController scoreController = vcFactory.CreateScoreController();
        IRng rng = new Rng();
        IPieceMerger merger = new PieceMerger(config, logger, scoreController);
        ISpawner spawner = new Spawner(config, controller, merger, vcFactory, rng, spawnerPosition.transform.position);
        
        deathPlane.OnPlayerLost += controller.DisableControls;
        deathPlane.OnPlayerLost += scoreController.HideScore;
        deathPlane.OnPlayerLost += () => EndGame(scoreController);
        
        gameManager.Setup(spawner);
        controller.Setup(spawner, deathPlane, leftWall, rightWall);
        controller.EnableControls();
        
        spawner.SpawnInitialPieces();
    }

    public void EndGame(IScoreController gameScoreController)
    {
        ScoreController loseScreenScoreController = new ScoreController(endGameScreen);
        loseScreenScoreController.SetScore(gameScoreController.Score);
        endGameScreen.gameObject.SetActive(true);
    }

    public void ResetGame()
    {
        // TODO need to properly clean up pieces and leftover gameObjects to finish reset - could be an opportunity to implement an object pool
        deathPlane = null;
        StartGame();
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}