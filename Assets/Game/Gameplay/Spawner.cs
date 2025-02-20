using System.Collections.Generic;
using UnityEngine;

public class Spawner : ISpawner
{
    public static readonly int maximumSpawnedObjects = 100;

    int pieceListId = 0;

    readonly IGameConfig config;
    readonly IMouseController controller;
    readonly IPieceMerger merger;
    readonly IViewControllerFactory vcFactory;
    readonly IRng rng;
    readonly Vector3 spawnOrigin;

    List<IPieceController> nextPiecesList;
    List<IPieceController> spawnedPiecesList;

    public Spawner(
        IGameConfig config,
        IMouseController controller,
        IPieceMerger merger,
        IViewControllerFactory vcFactory,
        IRng rng,
        Vector3 spawnOrigin
    ){
        this.config = config;
        this.merger = merger;
        this.controller = controller;
        this.vcFactory = vcFactory;
        this.spawnOrigin = spawnOrigin;
        this.rng = rng;
        spawnedPiecesList = new List<IPieceController>();
        nextPiecesList = new List<IPieceController>(config.NextPiecesListSize);
    }

    int SelectNext() => rng.SelectIndex(config.Chance);

    IPieceController SpawnPiece(PieceGraphics prefab, Vector3 position, int order, bool applyObjectScale)
    {
        if (spawnedPiecesList.Count > maximumSpawnedObjects)
        {
            // TODO notify game manager to lose game at this point
            return null;
        }

        IPieceController piece = vcFactory.CreatePiece(
            this,
            merger,
            pieceListId,
            order,
            position,
            config.PieceSizeFactor,
            config.PieceMassFactor,
            config.GravityScale,
            false
        );

        if (applyObjectScale)
        {
            piece.ApplyScaleFactor();
        }

        pieceListId++;

        spawnedPiecesList.Add(piece);
        return piece;
    }

    public void RemoveFromList(int id)
    {
        spawnedPiecesList.RemoveAll(piece => piece.Id == id);
    }

    public (IPieceController, List<IPieceController>) SpawnInitialPieces()
    {
        Vector3 positionOffset = new Vector3(2f, 0f, 0f);

        int pieceOrder = SelectNext();
        IPieceController startingPiece = SpawnPiece(config.Prefabs[pieceOrder], spawnOrigin, pieceOrder, true);
        controller.SetControlledObject(startingPiece);

        for (int i = 0; i < config.NextPiecesListSize; i++)
        {
            pieceOrder = SelectNext();
            var prefab = config.Prefabs[pieceOrder];
            IPieceController piece = SpawnPiece(prefab, spawnOrigin + positionOffset, pieceOrder, false);

            piece.Position = spawnOrigin + positionOffset;

            nextPiecesList.Add(piece);
            positionOffset.y -= 0.5f; // TODO convert this position to screen space position for properly position -- TODO more: make a queue view/handler/whatever to do this
        }

        return (startingPiece, nextPiecesList);
    }

    public void ConsumeMerges()
    {
        Triplet<IPieceController> triplet = merger.Consume();
        while (triplet != null)
        {
            merger.Merge(this, triplet);
            triplet = merger.Consume();
        }
    }

    public IPieceController SpawnAndPlayPiece(int pieceOrder, Vector3 position)
    {
        IPieceController piece = SpawnPiece(config.Prefabs[pieceOrder], position, pieceOrder, true);
        piece?.Play();
        return piece ?? null;
    }

    public IPieceController SpawnPiece()
    {
        IPieceController nextPiece = nextPiecesList[0];
        nextPiecesList.RemoveAt(0);
        nextPiece.Position = spawnOrigin;
        nextPiece.ApplyScaleFactor();
        controller.SetControlledObject(nextPiece);

        int pieceOrder = SelectNext();
        IPieceController newPiece = SpawnPiece(config.Prefabs[pieceOrder], spawnOrigin, pieceOrder, false);

        if (newPiece == null) return null;

        newPiece.Position = spawnOrigin + new Vector3(2f, config.NextPiecesListSize * -0.5f, 0f);
        nextPiecesList.Add(newPiece);

        ShiftListUp();

        return nextPiece;
    }

    public void DestroyPiece(IPieceController piece){
        RemoveFromList(piece.Id);
        piece.Destroy();
        piece = null;
    }


    // TODO need a proper piece queue/list to do this
    void ShiftListUp()
    {
        Vector3 positionOffset = new Vector3(2f, 0f, 0f);
        foreach (var piece in nextPiecesList)
        {
            piece.Position = spawnOrigin + positionOffset;
            positionOffset.y -= 0.5f;
        }
    }
}
