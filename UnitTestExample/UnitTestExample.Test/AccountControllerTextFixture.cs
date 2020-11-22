using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using UnitTestExample.Controllers;
using System.Activities;

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

        [
            Test,
            TestCase("irf@uni-corvinus.hu", "Abcd1234"),
            TestCase("irf@uni-corvinus.hu", "Abcd1234567")
        ]
        public void TestRegisterHappyPath(string email, string password)
        {
            var accountController = new AccountController();

            var result = accountController.Register(email, password);

            Assert.AreEqual(email, result.Email);
            Assert.AreEqual(password, result.Password);
            Assert.AreNotEqual(Guid.Empty, result.ID);
        }

        [
            Test,
            TestCase("irf@uni-corvinus", "Abcd1234"),
            TestCase("irf.uni-corvinus.hu", "Abcd1234"),
            TestCase("irf@uni-corvinus.hu", "abcd1234"),
            TestCase("irf@uni-corvinus.hu", "ABCD12345"),
            TestCase("irf@uni-corvinus.hu", "abcdefgh"),
            TestCase("irf@uni-corvinus.hu", "Ab1234")
        ]
        public void TestRegisterValidateException(string email, string password)
        {
            var accountController = new AccountController();

            try
            {
                var result = accountController.Register(email, password);
                Assert.Fail();
            }
            catch(Exception ex)
            {
                Assert.IsInstanceOf<ValidationException>(ex);
            }
        }
    }
}
