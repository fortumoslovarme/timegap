using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using TimeGap.ApiDemo.Dtos;
using Xunit;

namespace TimeGap.ApiDemo.Tests
{
    public class MonthlySaleControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        private const string MonthlySaleEndpoint = "api/monthlysale";

        public MonthlySaleControllerTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async void Get_WhenFromDuodecimDateMissing_ReturnExpectedBadResult()
        {
            // Arrange
            var url = $"{MonthlySaleEndpoint}?toDuodecimDate=2018-01";
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);
            var responseContent = await GetStringFromResponse(response);

            // Assert
            var responseStatusCode = response.StatusCode;
            Assert.Equal(HttpStatusCode.BadRequest, responseStatusCode);
            Assert.Contains("The fromDuodecimDate field is required.", responseContent);
        }

        [Fact]
        public async void Get_WhenToDuodecimDateMissing_ReturnExpectedBadResult()
        {
            // Arrange
            var url = $"{MonthlySaleEndpoint}?fromDuodecimDate=2018-01";
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);
            var responseContent = await GetStringFromResponse(response);

            // Assert
            var responseStatusCode = response.StatusCode;
            Assert.Equal(HttpStatusCode.BadRequest, responseStatusCode);
            Assert.Contains("The toDuodecimDate field is required.", responseContent);
        }

        [Fact]
        public async void Get_WhenFromDuodecimDateInvalid_ReturnExpectedBadResult()
        {
            // Arrange
            const string someInvalidDuodecimDate = "InvalidDuodecimDate";
            var url = $"{MonthlySaleEndpoint}?fromDuodecimDate={someInvalidDuodecimDate}&toDuodecimDate=2018-01";
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);
            var responseContent = await GetStringFromResponse(response);

            // Assert
            var responseStatusCode = response.StatusCode;
            Assert.Equal(HttpStatusCode.BadRequest, responseStatusCode);
            Assert.Contains($"The value '{someInvalidDuodecimDate}' is not valid.", responseContent);
        }

        [Fact]
        public async void Get_WhenToDuodecimDateInvalid_ReturnExpectedBadResult()
        {
            // Arrange
            const string someInvalidDuodecimDate = "InvalidDuodecimDate";
            var url = $"{MonthlySaleEndpoint}?fromDuodecimDate=2018-01&toDuodecimDate={someInvalidDuodecimDate}";
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);
            var responseContent = await GetStringFromResponse(response);

            // Assert
            var responseStatusCode = response.StatusCode;
            //response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal(HttpStatusCode.BadRequest, responseStatusCode);
            Assert.Contains($"The value '{someInvalidDuodecimDate}' is not valid.", responseContent);
        }

        [Fact]
        public async void Get_WhenValidInput_ReturnExpectedResult()
        {
            // Arrange
            var expectedSalesList = new List<MonthlySale>
            {
                new MonthlySale {Year = 2018, Month = 5, Value = 200},
                new MonthlySale {Year = 2018, Month = 12, Value = 300},
                new MonthlySale {Year = 2019, Month = 2, Value = 400},
                new MonthlySale {Year = 2019, Month = 11, Value = 500},
            };
            var expectedSalesCount = expectedSalesList.Count;

            var url = $"{MonthlySaleEndpoint}?fromDuodecimDate=2018-03&toDuodecimDate=2020-01";
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var responseContent = await GetStringFromResponse(response);

            // Assert
            var responseStatusCode = response.StatusCode;
            Assert.Equal(HttpStatusCode.OK, responseStatusCode);
            var monthlySalesResult = JsonConvert.DeserializeObject<List<MonthlySale>>(responseContent);
            Assert.Equal(expectedSalesCount, monthlySalesResult.Count);
            foreach (var monthlySale in expectedSalesList)
            {
                Assert.Contains(monthlySalesResult,
                    sale => sale.Year == monthlySale.Year && sale.Month == monthlySale.Month &&
                            sale.Value == monthlySale.Value);
            }
        }

        protected async Task<string> GetStringFromResponse(HttpResponseMessage httpResponseMessage)
        {
            return await httpResponseMessage.Content.ReadAsStringAsync();
        }
    }
}