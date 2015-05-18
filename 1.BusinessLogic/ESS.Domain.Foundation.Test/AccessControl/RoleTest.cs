using System;
using ESS.Domain.Foundation.AccessControl.Commands;
using ESS.Domain.Foundation.AccessControl.Domain;
using ESS.Domain.Foundation.AccessControl.Events;
using ESS.Framework.Common.Utilities;
using ESS.Framework.CQRS;
using NUnit.Framework;

namespace ESS.Domain.Foundation.Test.AccessControl
{
    [TestFixture]
    public class RoleTest : BDDTest<Role>
    {
        private Guid id;
        [SetUp]
        public void Setup()
        {
            id = ObjectId.GetNextGuid();
        }


        [Test]
        public void CanCreateRole()
        {
            Test(
                Given(),
                When(new CreateRole
                {
                    Id = id,
                    Name = "zf",
                    Note = "z",
                }),
                Then(new RoleCreated
                {
                    Id = id,
                    Name = "zf",
                    Note = "z",
                }));
        }

        [Test]
        public void CanLockRole()
        {
            Test(
                Given(new RoleCreated
                {
                    Id = id,
                    Name = "zf",
                    Note = "z",
                }),
                When(new LockRole
                {
                    Id = id
                }), Then(new RoleLocked { Id = id }));
        }

        [Test]
        public void CannotLockUnlockRole()
        {
            Test(
                Given(new RoleCreated
                {
                    Id = id,
                    Name = "zf",
                    Note = "z",
                }, new RoleLocked { Id = id }),
                When(new LockRole
                {
                    Id = id
                }), ThenFailWith<InvalidOperationException>());
        }

        [Test]
        public void CannotUnlockNewRole()
        {
            Test(Given(),
                When(new UnlockRole { Id = id }),
                ThenFailWith<InvalidOperationException>()
                );
        }
    }
}