using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    [SerializeField, Range(0f, 2f)] float pieceSizeMultiplier = 0.6f; // maybe use pow instead of multiplier?
    [SerializeField] int listSize;
    [SerializeField] SelectorConfig config;
    [SerializeField] MouseController controller; // probably move this to some game manager to comunicate between these two

    List<Piece> piecesList;

    int SelectNext() => WeightedRandom.GetIndex(config.chance);

    void Start(){
        piecesList = new List<Piece>(listSize);
        Vector3 positionOffset = new Vector3(2f, 0f, 0f);

        int pieceId = SelectNext();
        Debug.Log($"Instantiating object at {transform.position + positionOffset}");

        Piece piece = Instantiate(config.prefabs[pieceId], transform.position, Quaternion.identity);
        piece.Setup(pieceId, this);
        piece.gameObject.transform.localScale *= (pieceId*pieceSizeMultiplier + 1f);
        controller.SetSpawner(this); // interface this
        controller.SetControlledObject(piece);

        for(int i = 0; i < listSize; i++){
            pieceId = SelectNext();
            var prefab = config.prefabs[pieceId];
            piece = Instantiate(prefab, transform.position + positionOffset, Quaternion.identity);
            piece.Setup(pieceId, this);

            // maybe only scale piece when not in actual world/mouse? not on holder
            // piece.gameObject.transform.localScale *= (pieceId*pieceSizeMultiplier + 1f);
            piece.transform.position = transform.position + positionOffset;

            piecesList.Add(piece);
            positionOffset.y -= 0.5f; // todo convert this position to screen space position
        }
    }

    public void SpawnPieceFromMerge(int pieceId, Vector3 position){
        if(pieceId >= config.GetHighestPieceId()) return; // this is last piece, probably need a piece merger who knows this to decouple this logic

        Debug.Log($"Spawning piece with ID {pieceId}");
        Piece piece = Instantiate(config.prefabs[pieceId], position, Quaternion.identity);
        piece.Setup(pieceId, this);
        piece.SetGravityScale();
        piece.gameObject.transform.localScale *= (pieceId*pieceSizeMultiplier + 1f);
    }

    public Piece GetNextPiece(){
        Piece nextPiece = piecesList[0];
        piecesList.RemoveAt(0);
        nextPiece.gameObject.transform.position = transform.position;
        nextPiece.gameObject.transform.localScale *= (nextPiece.PieceId*pieceSizeMultiplier + 1f);
        controller.SetControlledObject(nextPiece);

        ShiftListUp();
        int pieceId = SelectNext();
        Piece newPiece = Instantiate(config.prefabs[pieceId], transform.position, Quaternion.identity);
        newPiece.Setup(pieceId, this);
        newPiece.transform.position = transform.position + new Vector3(2f, listSize * -0.5f, 0f);
        piecesList.Add(newPiece);

        return nextPiece;
    }

    // TODO need a proper piece queue/list to do this
    void ShiftListUp(){
        Vector3 positionOffset = new Vector3(2f, 0f, 0f);
        foreach(var piece in piecesList){

            piece.transform.position = transform.position + positionOffset;
            positionOffset.y -= 0.5f; // todo convert this position to screen space position
        }
    }
}
