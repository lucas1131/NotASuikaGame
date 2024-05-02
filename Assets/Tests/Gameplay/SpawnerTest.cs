using NUnit.Framework;
using FluentAssertions;
using NSubstitute;
using UnityEngine;
using System.Collections.Generic;

public class SpawnerTest
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


    #region SpawnInitialPieces tests

    void GivenNewPieceIsInstantiated(IPieceController piece){
        vcFactory.CreatePieceController(
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

    void GivenRngSelectedPiece(int index) => rngMock.SelectIndex(Arg.Any<float[]>()).Returns(index);


    (IPieceController, List<IPieceController>) WhenSpawningInitialPieces() => spawner.SpawnInitialPieces();

    void ThenCurrentPieceShouldNotBeNull(IPieceController currentPiece)
    {
        currentPiece.Should().NotBeNull();
    }

    void ThenNextPiecesShouldHaveSize(List<IPieceController> nextPieces, int expectedSize)
    {
        nextPieces.Should().NotBeNull();
        nextPieces.Count.Should().Be(expectedSize);
    }

    [Test]
    public void SpawnInitialPieces()
    {
        GivenDefaultTripleMergeEnabledGameConfig();
        // GivenNewPieceIsInstantiated(defaultTestPiece);
        GivenRngSelectedPiece(0);

        (IPieceController currentPiece, List<IPieceController> nextPieces) = WhenSpawningInitialPieces();

        ThenCurrentPieceShouldNotBeNull(currentPiece);
        ThenNextPiecesShouldHaveSize(nextPieces, 1);
    }

    #endregion

    #region SpawnPieceFromMerge tests
    [Test]
    public void SpawnPieceFromMerge()
    {
        Assert.Fail();
    }
    #endregion

    #region ConsumeMerges tests
    [Test]
    public void ConsumeMerges()
    {
        Assert.Fail();
    }
    #endregion

    #region SpawnPiece tests
    [Test]
    public void SpawnPiece()
    {
        Assert.Fail();
    }
    #endregion

    #region RemoveFromList tests
    [Test]
    public void RemoveFromList()
    {
        Assert.Fail();
    }
    #endregion

    #region Mocked values

    void GivenDefaultTripleMergeEnabledGameConfig()
    {
        GivenTripleMergeEnabledGameConfig();
        GivenGameConfigPieceSizeFactor(1.4f);
        GivenGameConfigPieceMassFactor(1.4f);
        GivenGameConfigNumberOfNextPiecesShown(1);
        GivenGameConfigPiecesList();
    }

    void GivenDefaultTripleMergeDisabledGameConfig()
    {
        GivenTripleMergeDisabledGameConfig();
        GivenGameConfigPieceSizeFactor(1.4f);
        GivenGameConfigPieceMassFactor(1.4f);
        GivenGameConfigNumberOfNextPiecesShown(1);
        GivenGameConfigPiecesList();
    }

    void GivenTripleMergeDisabledGameConfig() => gameConfigMock.AllowTripleMerge.Returns(false);
    void GivenTripleMergeEnabledGameConfig() => gameConfigMock.AllowTripleMerge.Returns(true);
    void GivenGameConfigPieceSizeFactor(float factor) => gameConfigMock.PieceSizeFactor.Returns(factor);
    void GivenGameConfigPieceMassFactor(float factor) => gameConfigMock.PieceMassFactor.Returns(factor);
    void GivenGameConfigNumberOfNextPiecesShown(int n) => gameConfigMock.NextPiecesListSize.Returns(n);
    void GivenGameConfigPiecesList()
    {
        gameConfigMock.Prefabs.Returns(new PieceGraphics[] { defaultTestPiece });
        gameConfigMock.Chance.Returns(new float[] { 100f });
    }

    PieceGraphics defaultTestPiece
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

    #endregion
}
