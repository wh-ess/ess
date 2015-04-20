using System;
using ESS.Domain.Common.Basic.Commands;
using NUnit.Framework;
using ESS.Framework.CQRS;
using ESS.Domain.Common.Basic.Domain;
using ESS.Domain.Common.Basic.Events;
using ESS.Framework.Common.Utilities;

namespace ESS.Domain.Common.Test.Basic
{
    [TestFixture]
    public class FloorTest : BDDTest<Floor>
    {
        private Guid _id;
        [SetUp]
        public void Setup()
        {
            _id = ObjectId.GetNextGuid();
        }


        [Test]
        public void CanCreateFloor()
        {
            Test(
                Given(),
                When(new CreateFloor
                {
                    Id = _id,
                    Name = "zf",
                }),
                Then(new FloorCreated
                {
                    Id = _id,
                    Name = "zf",
                }));
        }

        [Test]
        public void CanEditFloor()
        {
            Test(
                Given(new FloorCreated
                {
                    Id = _id,
                    Name = "zf",
                }),
                When(new EditFloor()
                {
                    Id = _id,
                    Name = "z"
                }), Then(new FloorEdited
                {
                    Id = _id,
                    Name = "z"
                }));
        }



        [Test]
        public void CanDeleteFloor()
        {
            Test(Given(),
                When(new DeleteFloor { Id = _id }),
                Then(new FloorDeleted
                {
                    Id = _id
                })
                );
        }
    }
}