using NUnit.Framework;
using FluentAssertions;
using NSubstitute;
using UnityEngine;
using System.Collections.Generic;

public partial class SpawnerShould
{
    void WhenConsumingMerges() => spawner.ConsumeMerges();

    [Test]
    public void ConsumeMerges()
    {

        WhenConsumingMerges();

        Assert.Fail();
    }
}


