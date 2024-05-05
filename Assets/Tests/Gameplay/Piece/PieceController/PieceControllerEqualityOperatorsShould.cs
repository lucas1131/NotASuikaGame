using NUnit.Framework;
using FluentAssertions;

public class PieceControllerEqualityOperatorsShould
{
    // This file is testing the equality overrides and are not any from any design requirement, so it will be testing
    // specifically code implementation and will be somewhat lower level testing
    // These equality comparers are from the equatable interfaces that piece controller implements

    PieceController CreateNewTestPiece(int id, int order) => new PieceController(null, null, null, id, order, 0f, 0f);

    [Test]
    public void BeSamePieceIfIdIsSame()
    {
        var piece1 = CreateNewTestPiece(id: 123, order: 2);
        var piece2 = CreateNewTestPiece(id: 123, order: 321);

        Assert.IsTrue(piece1 == piece2);
    }

    [Test]
    public void BeDifferentPieceIfIdIsDifferent()
    {
        var piece1 = CreateNewTestPiece(id: 123, order: 2);
        var piece2 = CreateNewTestPiece(id: 1, order: 2);

        Assert.IsFalse(piece1 == piece2);
    }

    [Test]
    public void BeSamePieceIfBothAreNull()
    {
        PieceController piece1 = null;
        PieceController piece2 = null;

        Assert.IsTrue(piece1 == piece2); // There is no way to use FluentAssertions here :c
    }

    [Test]
    public void BeDifferentIfOneIsNull()
    {
        var piece1 = CreateNewTestPiece(id: 123, order: 2);
        PieceController piece2 = null;

        Assert.IsFalse(piece1 == piece2);
    }

    [Test]
    public void TryCompareWithOtherPiecesWithEquals()
    {
        var piece1 = CreateNewTestPiece(id: 123, order: 2);
        var piece2 = CreateNewTestPiece(id: 123, order: 321);
        var piece3 = CreateNewTestPiece(id: 321, order: 2);

        bool equals12Result = piece1.Equals(piece2);
        bool equality12OperatorResult = piece1 == piece2;

        equals12Result.Should().Be(equality12OperatorResult);

        bool equals13Result = piece1.Equals(piece3);
        bool equality13OperatorResult = piece1 == piece3;
        equals13Result.Should().Be(equality13OperatorResult);
    }

    [Test]
    public void TryCompareWithOtherPiecesWithCompareTo()
    {
        var piece1 = CreateNewTestPiece(id: 123, order: 2);
        var piece2 = CreateNewTestPiece(id: 123, order: 5);
        var piece3 = CreateNewTestPiece(id: 321, order: 2);

        int compareTo12Result = piece1.CompareTo(piece2);

        compareTo12Result.Should().Be(0);

        int compareTo13Result = piece1.CompareTo(piece3);
        compareTo13Result.Should().NotBe(0);
    }

    [Test]
    public void ReturnNegativeWhenComparingIfNull()
    {
        var piece1 = CreateNewTestPiece(id: 123, order: 2);
        PieceController piece2 = null;

        piece1.CompareTo(piece2).Should().BeNegative();
    }

    [Test]
    public void UseIdAsHashCode()
    {
        var piece1 = CreateNewTestPiece(id: 123, order: 2);
        var piece2 = CreateNewTestPiece(id: 321, order: 2);

        piece1.GetHashCode().Should().Be(123);
        piece2.GetHashCode().Should().Be(321);
    }
}
