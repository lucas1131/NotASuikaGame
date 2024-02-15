using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Suika/SelectorConfig")]
public class SelectorConfig : ScriptableObject {
    public float[] chance;
    public Piece[] prefabs;

    public int GetHighestPieceId() => prefabs.Length-1;
}
