using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : ISpawner {

    public static readonly int maximumSpawnedObjects = 100;

    int pieceListId = 0;

    IGameConfig config;
    IMouseController controller;
    IPieceMerger merger;
    Vector3 spawnOrigin;

    List<Piece> nextPiecesList;
    List<Piece> spawnedPiecesList;

    public Spawner(IGameConfig config, IMouseController controller, IPieceMerger merger, Vector3 spawnOrigin){
        this.config = config;
        this.merger = merger;
        this.controller = controller;
        this.spawnOrigin = spawnOrigin;
        spawnedPiecesList = new List<Piece>();
    }

    Piece Instantiate(Piece prefab, Vector3 origin, int order, bool applyObjectScale) {
        if(spawnedPiecesList.Count >= maximumSpawnedObjects+1){
            // TODO notify game manager to lose game at this point
            return null;
        }

        Piece piece = Object.Instantiate(prefab, origin, Quaternion.identity);
        piece.Setup(
            this,
            merger,
            pieceListId,
            order,
            config.PieceSizeFactor,
            config.PieceMassFactor,
            config.GravityScale
        );

        if(applyObjectScale){
            piece.ApplyScaleFactor();
        }

        pieceListId++;

        spawnedPiecesList.Add(piece);
        return piece;
    }

    public void RemoveFromList(int id){
        spawnedPiecesList.RemoveAll(piece => piece.PieceId == id);
    }

    public void SpawnInitialPieces(){
        nextPiecesList = new List<Piece>(config.NextPiecesListSize);
        Vector3 positionOffset = new Vector3(2f, 0f, 0f);

        int pieceOrder = SelectNext();
        Piece piece = Instantiate(config.Prefabs[pieceOrder], spawnOrigin + positionOffset, pieceOrder, true);
        controller.SetControlledObject(piece);

        for(int i = 0; i < config.NextPiecesListSize; i++){
            pieceOrder = SelectNext();
            var prefab = config.Prefabs[pieceOrder];
            piece = Instantiate(prefab, spawnOrigin + positionOffset, pieceOrder, false);

            piece.Position = spawnOrigin + positionOffset;

            nextPiecesList.Add(piece);
            positionOffset.y -= 0.5f; // TODO convert this position to screen space position for properly position -- TODO more: make a queue view/handler/whatever to do this
        }
    }

    int SelectNext() => WeightedRandom.SelectIndex(config.Chance);

    public void ConsumeMerges(){

        Triplet<Piece> triplet = merger.Consume();
        while(triplet != null){
            merger.Merge(this, triplet);
            triplet = merger.Consume();
        }
    }

    public void SpawnPieceFromMerge(int pieceOrder, Vector3 position){
        Piece piece = Instantiate(config.Prefabs[pieceOrder], position, pieceOrder, true);
        if(piece == null) return;

        piece.PlayPiece();
    }

    public Piece SpawnPiece(){
        Piece nextPiece = nextPiecesList[0];
        nextPiecesList.RemoveAt(0);
        nextPiece.Position = spawnOrigin;
        nextPiece.ApplyScaleFactor();
        controller.SetControlledObject(nextPiece);

        ShiftListUp();
        int pieceOrder = SelectNext();
        Piece newPiece = Instantiate(config.Prefabs[pieceOrder], spawnOrigin, pieceOrder, false);
        if(newPiece == null) {
            return null;
        }

        newPiece.Position = spawnOrigin + new Vector3(2f, config.NextPiecesListSize * -0.5f, 0f);
        nextPiecesList.Add(newPiece);

        return nextPiece;
    }

    // TODO need a proper piece queue/list to do this
    void ShiftListUp(){
        Vector3 positionOffset = new Vector3(2f, 0f, 0f);
        foreach(var piece in nextPiecesList){

            piece.Position = spawnOrigin + positionOffset;
            positionOffset.y -= 0.5f; // todo convert this position to screen space position
        }
    }
}
