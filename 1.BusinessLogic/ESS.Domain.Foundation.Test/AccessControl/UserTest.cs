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
    public class UserTest : BDDTest<User>
    {
        private Guid id;
        [SetUp]
        public void Setup()
        {
            id = ObjectId.GetNextGuid();
        }

        [Test]
        public void CanCreateUser()
        {
            Test(
                Given(),
                When(new CreateUser()
                {
                    Id = id,
                    UserName = "zf",
                    UserNo = "z",
                    Password = "1"

                }),
                Then(new UserCreated
                {
                    Id = id,
                    UserName = "zf",
                    UserNo = "z",
                    Password = "1"

                }));
        }

        [Test]
        public void CanChangeUserInfo()
        {
            Test(
                Given(new UserCreated
                {
                    Id = id,
                    UserName = "zf",
                    UserNo = "z",
                    Password = "1"

                }),
                When(new ChangeUserInfo()
                {
                    Id = id,
                    UserName = "zff",
                    UserNo = "zf",

                }),
                Then(new UserInfoChanged
                {
                    Id = id,
                    UserName = "zff",
                    UserNo = "zf"

                }));
        }

        [Test]
        public void CanChangeUserPassword()
        {
            Test(
                Given(new UserCreated
                {
                    Id = id,
                    UserName = "zf",
                    UserNo = "z",
                    Password = "1"

                }),
                When(new ChangePassword()
                {
                    Id = id,
                    NewPassword = "22",
                    Password = "1",

                }),
                Then(new UserPasswordChanged
                {
                    Id = id,
                    NewPassword = "22",
                    Password = "1",

                }));
        }

        [Test]
        public void CanLockUser()
        {
            Test(
                Given(new UserCreated
                {
                    Id = id,
                    UserName = "zf",
                    UserNo = "z",
                    Password = "1"

                }),
                When(new LockUser()
                {
                    Id = id
                }),
                Then(new UserLocked
                {
                    Id = id
                }));
        }

        [Test]
        public void CanUnlockUser()
        {
            Test(
                Given(new UserCreated
                {
                    Id = id,
                    UserName = "zf",
                    UserNo = "z",
                    Password = "1"

                }, new UserLocked
                {
                    Id = id
                }),
                When(new UnlockUser()
                {
                    Id = id
                }),
                Then(new UserUnlocked
                {
                    Id = id
                }));
        }

    }
}
