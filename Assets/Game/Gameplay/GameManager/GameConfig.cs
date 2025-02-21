using UnityEngine;

[CreateAssetMenu(menuName = "Suika/GameConfig")]
public class GameConfig : ScriptableObject, IGameConfig
{
    [SerializeField] string displayName = "Default Game";
    [SerializeField] int baseScore = 10;
    [SerializeField] int tripleMergeBonusFactor = 1;
    [SerializeField] float gravityScale = 0.4f;
    [SerializeField] float pieceSizeFactor = 1.4f;
    [SerializeField] float pieceMassFactor = 1.4f;
    [SerializeField] bool allowTripleMerge = true;
    [SerializeField] int nextPiecesListSize;
    [SerializeField] float[] chance;
    [SerializeField] PieceGraphics[] prefabs;

    public string DisplayName => displayName;
    public int BaseScore => baseScore;
    public int TripleMergeBonusFactor => tripleMergeBonusFactor;
    public float GravityScale => gravityScale;
    public float PieceSizeFactor => pieceSizeFactor;
    public float PieceMassFactor => pieceMassFactor;
    public bool AllowTripleMerge => allowTripleMerge;
    public int NextPiecesListSize => nextPiecesListSize;
    public float[] Chance => chance;
    public PieceGraphics[] Prefabs => prefabs;

    public int GetHighestPieceOrder() => prefabs.Length;
}
