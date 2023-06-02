using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MovieRank.Integration.Tests.Setup
{
    // test setup: run this first time test are run and share it among other tests
    [CollectionDefinition("api")]
    public class CollectionFixture :  // this class is entry point when you run tests
        ICollectionFixture<TestContext>, //this means, run TestContext class before running tests. it will instantiate and run constructor
        ICollectionFixture<TestDataSetup> // it will run before running tests
    {
    }
}
