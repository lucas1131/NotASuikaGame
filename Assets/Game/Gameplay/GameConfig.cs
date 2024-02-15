using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Suika/GameConfig")]
public class GameConfig : ScriptableObject, IGameConfig {

    [SerializeField] float gravityScale = 0.4f;
    [SerializeField] float pieceSizeFactor = 1.4f;
    [SerializeField] int nextPiecesListSize;
    [SerializeField] float[] chance;
    [SerializeField] Piece[] prefabs;

    public float GravityScale => gravityScale;
    public float PieceSizeFactor => pieceSizeFactor;
    public int NextPiecesListSize => nextPiecesListSize;
    public float[] Chance => chance;
    public Piece[] Prefabs => prefabs;

    public int GetHighestPieceId() => prefabs.Length;
}
