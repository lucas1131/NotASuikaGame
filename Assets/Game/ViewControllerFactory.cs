using UnityEngine;

public class ViewControllerFactory : IViewControllerFactory
{
	// Naming conventions are: Graphics for world objects and View for UI elements, but the outcome is the same, separate logic from graphics/view

    private readonly IObjectInstantiator instantiator;
    private readonly PrefabsLibrary library;
    private readonly Canvas viewCanvas;
    private readonly ScoreView scoreViewPrefab;

    static readonly string addressablePrefabPath = ""; // TODO: this should become addressables path to load so we can distribute whatever assets are necessary remotely

    public ViewControllerFactory(IObjectInstantiator instantiator, PrefabsLibrary library, Canvas viewCanvas, ScoreView scoreViewPrefab)
    {
        this.instantiator = instantiator;
        this.library = library;
        this.viewCanvas = viewCanvas;
        this.scoreViewPrefab = scoreViewPrefab;
    }

    public IPieceController CreatePiece(
        ISpawner spawner,
        IPieceMerger merger,
        int pieceId,
        int pieceOrder,
        Vector3 position,
        float scaleFactor,
        float massFactor,
        float gravity,
        bool enablePhysics
    ){

        PieceGraphics pieceGraphics = instantiator.Instantiate<PieceGraphics>(
            library.piecesPrefabList[pieceOrder],
            position,
            Quaternion.identity
        );

        PieceController pieceController = new PieceController(
            pieceGraphics,
            spawner,
            merger,
            pieceId,
            pieceOrder,
            scaleFactor,
            massFactor
        );

        pieceGraphics.Setup(pieceController, enablePhysics, gravity);

        return pieceController;
    }

    public IScoreController CreateScoreController()
    {
        ScoreView scoreView = instantiator.Instantiate<ScoreView>(scoreViewPrefab, viewCanvas.gameObject.transform);

        ScoreController pieceController = new ScoreController(scoreView);

        return pieceController;
    }
}