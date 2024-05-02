using UnityEngine;

public static class WeightedRandom
{

    /* Get an random Index weighted by the weights array. This assumes all weights are bigger than 0,
		otherwise behaviour is undefined.
	*/
    public static int SelectIndex(float[] weights)
    {
        if (weights == null || weights.Length == 0) return -1;

        float totalWeight = 0;
        foreach (var w in weights)
        {
            totalWeight += w;
        }

        float randomValue = Random.value;
        float cumulativeChance = 0f;

        for (int i = 0; i < weights.Length; i++)
        {
            if (weights[i] > 0)
            {
                cumulativeChance += weights[i] / totalWeight;
                if (cumulativeChance >= randomValue)
                {
                    return i;
                }
            }
        }

        return -1;
    }
}