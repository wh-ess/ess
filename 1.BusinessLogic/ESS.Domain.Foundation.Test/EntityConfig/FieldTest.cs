using System;
using ESS.Domain.Foundation.EntityConfig.Commands;
using ESS.Domain.Foundation.EntityConfig.Domain;
using ESS.Domain.Foundation.EntityConfig.Events;
using ESS.Framework.Common.Utilities;
using ESS.Framework.CQRS;
using NUnit.Framework;

namespace ESS.Domain.Foundation.Test.EntityConfig
{
    [TestFixture]
    public class FieldTest : BDDTest<Field>
    {
        private Guid id;
        [SetUp]
        public void Setup()
        {
            id = ObjectId.GetNextGuid();
        }


        


    }
}