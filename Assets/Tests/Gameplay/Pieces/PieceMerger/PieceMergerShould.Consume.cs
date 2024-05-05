using NUnit.Framework;
using FluentAssertions;

public partial class PieceMergerShould
{
    Triplet<IPieceController> WhenConsumingFromQueue() => merger.Consume();

    void ThenTripletShoudNotBeNull(Triplet<IPieceController> triplet)
    {
        triplet.Should().NotBeNull();
    }
    void ThenTripletShoudBeNull(Triplet<IPieceController> triplet)
    {
        triplet.Should().BeNull();
    }

    [Test]
    public void ReturnNullIfNoPiecesWereRegistered()
    {
        var triplet = WhenConsumingFromQueue();

        ThenTripletShoudBeNull(triplet);
    }

    [Test]
    [TestCase(0)]
    [TestCase(2)]
    [TestCase(18)]
    [TestCase(321)]
    public void ReturnSameAmountOfTripletsThatWereRegistered(int numberOfTripletsToRegister)
    {
        Triplet<IPieceController> triplet;

        for (int i = 0; i < numberOfTripletsToRegister; i++)
        {
            var piece1 = GivenPiece(i, 1);
            var piece2 = GivenPiece(i, 1);
            GivenPiecesWereRegisteredForMerging(piece1, piece2);

            triplet = WhenConsumingFromQueue();

            ThenTripletShoudNotBeNull(triplet);
        }

        triplet = WhenConsumingFromQueue();
        ThenTripletShoudBeNull(triplet);
    }
}
