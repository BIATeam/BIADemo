// BIADemo only
// <copyright file="UnitTestExample.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>
namespace TheBIADevCompany.BIADemo.Test.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Template of unit test.
    /// </summary>
    /// <seealso cref="AbstractUnitTest" />
    [TestClass]
    public class UnitTestExample : AbstractUnitTest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnitTestExample"/> class.
        /// </summary>
        public UnitTestExample()
            : base(false)
        {
            // Initialize AbstractUnitTest.isInitDB, which is used to either start each test of this test suite:
            // - with an empty DB
            // - or with some default data in the DB (inserted through IMockEntityFramework.InitDefaultData())
        }

        /// <summary>
        /// Initialize the test suite.
        /// This method is executed only once for the whole test suite.
        /// </summary>
        /// <param name="context">The context.</param>
        [ClassInitialize]
        public static void InitTestSuite(TestContext context)
        {
            // Add test suite initialization code here.
        }

        /// <summary>
        /// Cleanup at the end of the test suite.
        /// This method is executed only once for the whole test suite.
        /// </summary>
        [ClassCleanup]
        public static void CleanupTestSuite()
        {
            // Add test suite cleanup code here.
        }

        /// <summary>
        /// Initialize the context before each test.
        /// </summary>
        [TestInitialize]
        public void InitTest()
        {
            // Add test initialization code here.
        }

        /// <summary>
        /// Cleanup after each test.
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
            // Add test cleanup code here.
        }

        /// <summary>
        /// A basic test method.
        /// </summary>
        [TestMethod("UnitTestExample.TestMethod")]
        public void TestMethod()
        {
            // Code your test here.
            Assert.IsTrue(true);
        }

        /// <summary>
        /// A test method that can be run with different inputs.
        /// Each DataRow represents an execution of this method for the specified values.
        /// </summary>
        /// <param name="value">The value to test.</param>
        /// <param name="expectedResult">The expected result.</param>
        [DataTestMethod]
        [DataRow(-1, true)]
        [DataRow(0, true)]
        [DataRow(1, true)]
        [DataRow(2, false)]
        public void TestMethodFactorized(int value, bool expectedResult)
        {
            // Code your test here.
            // This method allows to run this test several times with different input values each time.
            // Each 'DataRow' annotation will create an execution of this test with the given values.
            Assert.AreEqual(expectedResult, value < 2);
        }
    }
}
