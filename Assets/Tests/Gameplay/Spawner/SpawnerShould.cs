using NUnit.Framework;
using NSubstitute;
using UnityEngine;

public partial class SpawnerShould
{
    ISpawner spawner;

    IGameConfig gameConfigMock;
    IMouseController mouseControllerMock;
    IPieceMerger pieceMergerMock;
    IViewControllerFactory vcFactory;
    IRng rngMock;

    [SetUp]
    public void SetupSpawner()
    {
        gameConfigMock = Substitute.For<IGameConfig>();
        // Calling other mocks for the config will override its return value anyway, so its not much of a problem to
        // start mocking this on Setup. This is needed just because we preallocate nextPieceListSize for performance
        GivenDefaultTripleMergeEnabledGameConfig();

        mouseControllerMock = Substitute.For<IMouseController>();
        pieceMergerMock = Substitute.For<IPieceMerger>();
        vcFactory = Substitute.For<IViewControllerFactory>();
        rngMock = Substitute.For<IRng>();

        spawner = new Spawner(
            gameConfigMock,
            mouseControllerMock,
            pieceMergerMock,
            vcFactory,
            rngMock,
            new Vector3(0f, 0f, 0f));
    }

    void GivenNewPieceIsInstantiated(IPieceController piece)
    {
        vcFactory.CreatePiece(
                Arg.Any<ISpawner>(),
                Arg.Any<IPieceMerger>(),
                Arg.Any<int>(),
                Arg.Any<int>(),
                Arg.Any<Vector3>(),
                Arg.Any<float>(),
                Arg.Any<float>(),
                Arg.Any<float>(),
                Arg.Any<bool>())
            .Returns(piece);
    }


    void GivenDefaultTripleMergeEnabledGameConfig()
    {
        GivenTripleMergeEnabledGameConfig();
        GivenGameConfigPieceSizeFactor(1.4f);
        GivenGameConfigPieceMassFactor(1.4f);
        GivenGameConfigNumberOfNextPiecesShown(2);
        GivenGameConfigPiecesList();
    }

    void GivenDefaultTripleMergeDisabledGameConfig()
    {
        GivenTripleMergeDisabledGameConfig();
        GivenGameConfigPieceSizeFactor(1.4f);
        GivenGameConfigPieceMassFactor(1.4f);
        GivenGameConfigNumberOfNextPiecesShown(2);
        GivenGameConfigPiecesList();
    }

    void GivenTripleMergeDisabledGameConfig() => gameConfigMock.AllowTripleMerge.Returns(false);
    void GivenTripleMergeEnabledGameConfig() => gameConfigMock.AllowTripleMerge.Returns(true);
    void GivenGameConfigPieceSizeFactor(float factor) => gameConfigMock.PieceSizeFactor.Returns(factor);
    void GivenGameConfigPieceMassFactor(float factor) => gameConfigMock.PieceMassFactor.Returns(factor);
    void GivenGameConfigNumberOfNextPiecesShown(int n) => gameConfigMock.NextPiecesListSize.Returns(n);
    void GivenGameConfigPiecesList()
    {
        gameConfigMock.Prefabs.Returns(new PieceGraphics[] { defaultTestPieceGraphics, defaultTestPieceGraphics, defaultTestPieceGraphics });
        gameConfigMock.Chance.Returns(new float[] { 100f });
    }

    static int defaultTestPieceId;
    static int pieceIdGenerator() => defaultTestPieceId++;
    static int defaultTestPieceOrder;
    static int pieceOrderGenerator() => defaultTestPieceOrder++;

    IPieceController defaultTestPieceController
    {
        get
        {
            IPieceController piece = Substitute.For<IPieceController>();
            piece.Id.Returns(_ => pieceIdGenerator());
            piece.Order.Returns(_ => pieceOrderGenerator());

            // piece.Id.Returns(123);
            // piece.Order.Returns(321);
            return piece;
        }
    }

    PieceGraphics defaultTestPieceGraphics
    {
        get
        {
            // Mock minimal piece object, aside from creating components, no engine code should run in this
            GameObject gameObject = new GameObject();
            gameObject.AddComponent<Rigidbody2D>();
            gameObject.AddComponent<CircleCollider2D>();
            return gameObject.AddComponent<PieceGraphics>();
        }
    }
}
