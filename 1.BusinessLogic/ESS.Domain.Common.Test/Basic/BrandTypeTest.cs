using System;
using ESS.Domain.Common.Basic.Commands;
using NUnit.Framework;
using ESS.Domain.Common.Basic.Domain;
using ESS.Domain.Common.Basic.Events;
using ESS.Framework.Common.Utilities;
using ESS.Framework.CQRS;

namespace ESS.Domain.Common.Test.Basic
{
    [TestFixture]
    public class BrandTypeTest : BDDTest<BrandType>
    {
        private Guid _id;
        [SetUp]
        public void Setup()
        {
            _id = ObjectId.GetNextGuid();
        }


        [Test]
        public void CanCreateBrandType()
        {
            Test(
                Given(),
                When(new CreateBrandType
                {
                    Id = _id,
                    Name = "zf",
                }),
                Then(new BrandTypeCreated
                {
                    Id = _id,
                    Name = "zf",
                }));
        }

        [Test]
        public void CanEditBrandType()
        {
            Test(
                Given(new BrandTypeCreated
                {
                    Id = _id,
                    Name = "zf",
                }),
                When(new EditBrandType()
                {
                    Id = _id,
                    Name = "z"
                }), Then(new BrandTypeEdited
                {
                    Id = _id,
                    Name = "z"
                }));
        }



        [Test]
        public void CanDeleteBrandType()
        {
            Test(Given(),
                When(new DeleteBrandType { Id = _id }),
                Then(new BrandTypeDeleted
                {
                    Id = _id
                })
                );
        }
    }
}