using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using UnitTestExample.Controllers;

namespace UnitTestExample.Test
{
    public class AccountControllerTextFixture
    {
        [Test]
        public void TestValidateEmail(string email, bool expectedResult)
        {
            var accountController = new AccountController();

            bool result = accountController.ValidateEmail(email);

            Assert.AreEqual(expectedResult, result);
        }
    }
}
