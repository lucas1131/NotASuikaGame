public interface IRng
{
    float Range(int minInclusive, int maxExclusive);
    int SelectIndex(float[] weights);
}