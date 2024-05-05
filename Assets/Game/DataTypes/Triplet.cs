using System;

/*
    Triplet structure made to work as a Set -- maybe call this TripletSet? the order of the values
    doesn't matter for this structure, ie: triple(1, 2, 3) is the same as triple(3, 1, 2) and triple(3, 2, 1)
*/

// TODO: make the generic class inherit from non generic Triplet for better usage and scalability
public class Triplet : IEquatable<Triplet> {

    public object v1;
    public object v2;
    public object v3;

    public Triplet(object v1, object v2, object v3){
        this.v1 = v1;
        this.v2 = v2;
        this.v3 = v3;
    }

    public Triplet(object v1, object v2){
        this.v1 = v1;
        this.v2 = v2;
        this.v3 = default(object);
    }

    public bool AddValueOverNull(object val){
        bool added = false;
        if(ReferenceEquals(v1, null)){
            v1 = val;
            added = true;
        }
        if(ReferenceEquals(v2, null)){
            v2 = val;
            added = true;
        }
        if(ReferenceEquals(v3, null)){
            v3 = val;
            added = true;
        }

        return added;
    }

    public bool Contains(object val){
        bool isValNull = ReferenceEquals(val, null);
        return ((this.v1?.Equals(val) ?? isValNull) ||
                (this.v2?.Equals(val) ?? isValNull) ||
                (this.v3?.Equals(val) ?? isValNull));
    }

    public bool Equals(Triplet other){
        if (ReferenceEquals(other, null))
            return false;
        if (ReferenceEquals(this, other))
            return true;

        return other.Contains(v1) &&
               other.Contains(v2) &&
               other.Contains(v3);
    }

    public override int GetHashCode(){
        object[] set = new object[] { v1, v2, v3 };

        int hash = 0;
        for(int i = 0; i < set.Length; i++){
            object val = set[i];
            if(val == null) {
                hash += Int32.MaxValue/3; // Maximum value becomes triplet of 3 nulls
            } else {
                hash += val.GetHashCode() << i;
            }
        }

        return hash;
    }

    public override string ToString(){
        return $"({v1?.ToString() ?? "null"}, {v2?.ToString() ?? "null"}, {v3?.ToString() ?? "null"})";
    }
}

public class Triplet<T> : Triplet where T : IEquatable<T>, IComparable<T> {

    new public T v1 { get => (T) base.v1; set => base.v1 = (T) value; }
    new public T v2 { get => (T) base.v2; set => base.v2 = (T) value; }
    new public T v3 { get => (T) base.v3; set => base.v3 = (T) value; }

    public Triplet(T v1, T v2, T v3) : base(v1, v2, v3) { }
    public Triplet(T v1, T v2) : base(v1, v2) { }

    public override int GetHashCode(){
        T[] set = new T[] { v1, v2, v3 };

        Array.Sort(set);

        int hash = 0;
        for(int i = 0; i < set.Length; i++){
            T val = set[i];
            if(val == null) {
                hash += Int32.MaxValue/3; // Maximum value becomes triplet of 3 nulls
            } else {
                hash += val.GetHashCode() << i;
            }
        }

        return hash;
    }
}
