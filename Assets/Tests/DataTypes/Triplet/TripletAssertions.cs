using System;
using FluentAssertions;
using FluentAssertions.Primitives;
using FluentAssertions.Execution;

public class TripletAssertions : ReferenceTypeAssertions<Triplet, TripletAssertions>
{
    public TripletAssertions(Triplet instance) : base(instance) { }

    protected override string Identifier => "triplet";

    public AndConstraint<TripletAssertions> Contain(object item, string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .Given(() => Subject.Contains(item))
            .ForCondition(isContained => isContained)
            .FailWith("Expected {context:triplet} to contain {0}{reason}, but that object was not found.", _ => item);

        return new AndConstraint<TripletAssertions>(this);
    }

    public AndConstraint<TripletAssertions> NotContain(object item, string because = "", params object[] becauseArgs)
    {
        Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .Given(() => !Subject.Contains(item))
            .ForCondition(isContained => isContained)
            .FailWith("Expected {context:triplet} to not contain {0}{reason}, but that object was found.", item);

        return new AndConstraint<TripletAssertions>(this);
    }

}

public class TripletAssertions<T> : TripletAssertions where T : IEquatable<T>, IComparable<T>
{
    public TripletAssertions(Triplet<T> instance) : base(instance) { }
}
