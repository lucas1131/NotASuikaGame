public partial class PieceController {

    public static bool operator == (PieceController p1, IPieceController p2){
        bool isP1Null = ReferenceEquals(p1, null);
        bool isP2Null = ReferenceEquals(p2, null);
        bool areBothNull = isP1Null && isP2Null;
        bool isOneNull = isP1Null || isP2Null;

        if (areBothNull) return true;
        if (isOneNull) return false;

        return p1.Id == p2.Id;
    }

    public static bool operator != (PieceController p1, IPieceController p2) => !(p1 == p2);

	public bool Equals(IPieceController other){
        if (other == null) return false;
        return Id == other.Id;
    }

    public override int GetHashCode(){
        return Id;
    }

    public int CompareTo(IPieceController other){
        // Make null behave as the biggest value to work with hash calculation later
        if (other == null) return -1;
        return Id.CompareTo(other.Id);
    }
}
