using System;
using System.Net;
using System.Net.Http;
using System.Text;
using CodeChallenge.Models;
using CodeCodeChallenge.Tests.Integration.Extensions;
using CodeCodeChallenge.Tests.Integration.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeChallenge.Tests.Integration.Controllers
{
    [TestClass]
    public class CompensationControllerTests
    {
        private static HttpClient _httpClient;
        private static TestServer _testServer;

        [ClassInitialize]
        // Attribute ClassInitialize requires this signature
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public static void InitializeClass(TestContext context)
        {
            _testServer = new TestServer();
            _httpClient = _testServer.NewClient();
        }

        [ClassCleanup]
        public static void CleanUpTest()
        {
            _httpClient.Dispose();
            _testServer.Dispose();
        }

      
        [TestMethod]
        public void Create_ReturnsCreatedAtRouteResult_WithCompensation()
        {
            const string employeeId = "b7839309-3348-463b-a7e3-5de1c168beb3";
            const decimal salary = 75000;
            DateTime effectiveDate = new DateTime(2025, 1, 1);

            // Arrange
            var compensation = new Compensation
            {
                EmployeeId = employeeId,
                Salary = salary,
                EffectiveDate = effectiveDate
            };
            var requestContent = new JsonSerialization().ToJson(compensation);

            

            // Act
            var postRequestTask = _httpClient.PostAsync("api/compensation", new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var response = postRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            var newCompensation = response.DeserializeContent<Compensation>();
            Assert.IsNotNull(newCompensation.EmployeeId);
            Assert.AreEqual(employeeId, newCompensation.EmployeeId);
            Assert.AreEqual(salary, newCompensation.Salary);
            Assert.AreEqual(effectiveDate, newCompensation.EffectiveDate);
        }

        [TestMethod]
        public void Create_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var requestContent = new JsonSerialization().ToJson(new Compensation());

            // Act
            var postRequestTask = _httpClient.PostAsync("api/compensation", new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var response = postRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void Create_ReturnsBadRequest_WhenEmployeeDoesNotExist()
        {
            // Arrange
            var compensation = new Compensation
            {
                EmployeeId = "1234",
                Salary = 75000,
                EffectiveDate = new DateTime(2025, 1, 1)
            };

            var requestContent = new JsonSerialization().ToJson(compensation);

            // Act
            var postRequestTask = _httpClient.PostAsync("api/compensation", new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var response = postRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
