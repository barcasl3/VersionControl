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
        [
            Test,
            TestCase("abcd1234", false),
            TestCase("irf@uni-corvinus", false),
            TestCase("irf.uni-corvinus.hu", false),
            TestCase("irf@uni-corvinus.hu", true)
        ]
        public void TestValidateEmail(string email, bool expectedResult)
        {
            var accountController = new AccountController();

            bool result = accountController.ValidateEmail(email);

            Assert.AreEqual(expectedResult, result);
        }

        [
            Test,
            TestCase("abCdefghi", false),
            TestCase("ABCDE1234", false),
            TestCase("abcde1234", false),
            TestCase("abcd", false),
            TestCase("Abcd1234", true)
        ]
        public void TestValidatePassword(string password, bool expectedResult)
        {
            var accountController = new AccountController();

            bool result = accountController.ValidatePassword(password);

            Assert.AreEqual(expectedResult, result);
        }
    }
}
