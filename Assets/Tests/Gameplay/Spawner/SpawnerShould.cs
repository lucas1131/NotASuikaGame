using NUnit.Framework;
using FluentAssertions;
using NSubstitute;
using UnityEngine;
using System.Collections.Generic;

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
}
