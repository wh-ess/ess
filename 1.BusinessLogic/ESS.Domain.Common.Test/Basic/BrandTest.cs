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
    public class BrandTest : BDDTest<Brand>
    {
        private Guid _id;
        [SetUp]
        public void Setup()
        {
            _id = ObjectId.GetNextGuid();
        }


        [Test]
        public void CanCreateBrand()
        {
            Test(
                Given(),
                When(new CreateBrand
                {
                    Id = _id,
                    Name = "zf",
                }),
                Then(new BrandCreated
                {
                    Id = _id,
                    Name = "zf",
                }));
        }

        [Test]
        public void CanEditBrand()
        {
            Test(
                Given(new BrandCreated
                {
                    Id = _id,
                    Name = "zf",
                }),
                When(new EditBrand()
                {
                    Id = _id,
                    Name = "z"
                }), Then(new BrandEdited
                {
                    Id = _id,
                    Name = "z"
                }));
        }



        [Test]
        public void CanDeleteBrand()
        {
            Test(Given(),
                When(new DeleteBrand { Id = _id }),
                Then(new BrandDeleted
                {
                    Id = _id
                })
                );
        }
    }
}