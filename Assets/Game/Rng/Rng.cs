using UnityEngine;

public class Rng : IRng
{
    public float Range(int minInclusive, int maxExclusive) => Random.Range(minInclusive, maxExclusive);
    public int SelectIndex(float[] weights) => WeightedRandom.SelectIndex(weights);
}
