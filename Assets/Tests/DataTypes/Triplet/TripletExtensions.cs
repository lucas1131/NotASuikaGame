public static class TripletExtensions
{
    public static TripletAssertions Should(this Triplet instance)
    {
        return new TripletAssertions(instance);
    }
}
