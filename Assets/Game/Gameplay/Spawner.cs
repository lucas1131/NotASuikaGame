using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : ISpawner {

    GameConfig config;
    MouseController controller; // probably move this to some game manager to comunicate between these two
    Vector3 spawnOrigin;

    List<Piece> piecesList;

    public Spawner(GameConfig config, MouseController controller, Vector3 spawnOrigin){
        this.config = config;
        this.controller = controller;
        this.spawnOrigin = spawnOrigin;
    }

    public void SpawnInitialPieces(){
        piecesList = new List<Piece>(config.NextPiecesListSize);
        Vector3 positionOffset = new Vector3(2f, 0f, 0f);

        int pieceId = SelectNext();

        Piece piece = Object.Instantiate(config.Prefabs[pieceId], spawnOrigin, Quaternion.identity);
        piece.Setup(this, pieceId, config.GravityScale);
        piece.gameObject.transform.localScale *= Mathf.Pow(config.PieceSizeFactor, pieceId+1);
        controller.SetSpawner(this); // interface this
        controller.SetControlledObject(piece);

        for(int i = 0; i < config.NextPiecesListSize; i++){
            pieceId = SelectNext();
            var prefab = config.Prefabs[pieceId];
            piece = Object.Instantiate(prefab, spawnOrigin + positionOffset, Quaternion.identity);
            piece.Setup(this, pieceId, config.GravityScale);

            piece.transform.position = spawnOrigin + positionOffset;

            piecesList.Add(piece);
            positionOffset.y -= 0.5f; // TODO convert this position to screen space position for properly position -- TODO more: make a queue view/handler/whatever to do this
        }
    }

    int SelectNext() => WeightedRandom.SelectIndex(config.Chance);

    public void SpawnPieceFromMerge(int pieceId, Vector3 position){
        if(pieceId >= config.GetHighestPieceId()) return; // this is last piece, probably need a piece merger who knows this to decouple this logic

        Piece piece = Object.Instantiate(config.Prefabs[pieceId], position, Quaternion.identity);
        piece.Setup(this, pieceId, config.GravityScale);
        piece.PlayPiece();
        piece.gameObject.transform.localScale *= Mathf.Pow(config.PieceSizeFactor, pieceId+1);
    }

    public Piece SpawnPiece(){
        Piece nextPiece = piecesList[0];
        piecesList.RemoveAt(0);
        nextPiece.gameObject.transform.position = spawnOrigin;
        nextPiece.gameObject.transform.localScale *= Mathf.Pow(config.PieceSizeFactor, nextPiece.PieceId+1);
        controller.SetControlledObject(nextPiece);

        ShiftListUp();
        int pieceId = SelectNext();
        Piece newPiece = Object.Instantiate(config.Prefabs[pieceId], spawnOrigin, Quaternion.identity);
        newPiece.Setup(this, pieceId, config.GravityScale);
        newPiece.transform.position = spawnOrigin + new Vector3(2f, config.NextPiecesListSize * -0.5f, 0f);
        piecesList.Add(newPiece);

        return nextPiece;
    }

    // TODO need a proper piece queue/list to do this
    void ShiftListUp(){
        Vector3 positionOffset = new Vector3(2f, 0f, 0f);
        foreach(var piece in piecesList){

            piece.transform.position = spawnOrigin + positionOffset;
            positionOffset.y -= 0.5f; // todo convert this position to screen space position
        }
    }
}
