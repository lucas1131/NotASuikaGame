using NUnit.Framework;
using NSubstitute;

public partial class PieceControllerShould
{
    IPieceController piece;

    IPieceGraphics pieceGraphicsMock;
    ISpawner spawnerMock;
    IPieceMerger pieceMergerMock;

    [SetUp]
    public void Setup()
    {
        pieceGraphicsMock = Substitute.For<IPieceGraphics>();
        spawnerMock = Substitute.For<ISpawner>();
        pieceMergerMock = Substitute.For<IPieceMerger>();

        piece = new PieceController(
            pieceGraphicsMock,
            spawnerMock,
            pieceMergerMock,
            123,
            2,
            2f,
            2f
        );
    }

    PieceController CreateOtherCollidingPiece(int id, int order, bool isInPlay, bool isMerging){
        PieceController piece = new PieceController(
            pieceGraphicsMock,
            spawnerMock,
            pieceMergerMock,
            id,
            order,
            2f,
            2f
        );

        if(isInPlay) piece.Play();
        piece.IsMerging = isMerging;
        return piece;
    }
}
