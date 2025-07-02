using Xunit;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlackRockTask.Server.Controllers;
using BlackRockTask.Server.Models;

public class InvestorsDataControllerTests
{
    private class FailingInvestorsDataController : InvestorsDataController
    {
        public override List<InvestorDataRow> ReadCSV()
        {
            throw new Exception("Simulated data error");
        }
    }

    [Fact]
    public async Task GetSampleData_WhenExceptionThrown_ReturnsStatus500()
    {
        var controller = new FailingInvestorsDataController();

        var result = await controller.GetSampleData();

        var statusResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusResult.StatusCode);
        Assert.Contains("Simulated data error", statusResult.Value.ToString());
    }

    [Fact]
    public async Task GetDataByInvestor_WhenExceptionThrown_ReturnsStatus500()
    {
        var controller = new FailingInvestorsDataController();

        var result = await controller.GetDataByInvestor("any", "All");

        var statusResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusResult.StatusCode);
        Assert.Contains("Simulated data error", statusResult.Value.ToString());
    }

    [Fact]
    public async Task GetInvestorSummaries_WhenExceptionThrown_ReturnsStatus500()
    {
        var controller = new FailingInvestorsDataController();

        var result = await controller.GetInvestorSummaries("any");

        var statusResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusResult.StatusCode);
        Assert.Contains("Simulated data error", statusResult.Value.ToString());
    }

    private class MockInvestorsDataController : InvestorsDataController
    {
        public override List<InvestorDataRow> ReadCSV()
        {
            // Representative sample from data.csv
            return new List<InvestorDataRow>
            {
                new InvestorDataRow
                {
                    InvestorName = "Ioo Gryffindor fund",
                    InvestorType = "fund manager",
                    InvestorCountry = "Singapore",
                    InvestorDateAdded = new DateTime(2000, 7, 6),
                    InvestorLastUpdated = new DateTime(2024, 2, 21),
                    CommitmentAssetClass = "Infrastructure",
                    CommitmentAmount = 15000000,
                    CommitmentCurrency = "GBP"
                },
                new InvestorDataRow
                {
                    InvestorName = "Ioo Gryffindor fund",
                    InvestorType = "fund manager",
                    InvestorCountry = "Singapore",
                    InvestorDateAdded = new DateTime(2000, 7, 6),
                    InvestorLastUpdated = new DateTime(2024, 2, 21),
                    CommitmentAssetClass = "Hedge Funds",
                    CommitmentAmount = 31000000,
                    CommitmentCurrency = "GBP"
                },
                new InvestorDataRow
                {
                    InvestorName = "Ibx Skywalker ltd",
                    InvestorType = "asset manager",
                    InvestorCountry = "United States",
                    InvestorDateAdded = new DateTime(1997, 7, 21),
                    InvestorLastUpdated = new DateTime(2024, 2, 21),
                    CommitmentAssetClass = "Infrastructure",
                    CommitmentAmount = 31000000,
                    CommitmentCurrency = "GBP"
                }
            };
        }
    }

    [Fact]
    public async Task GetSampleData_ReturnsGroupedData()
    {
        var controller = new MockInvestorsDataController();

        var result = await controller.GetSampleData();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var data = Assert.IsAssignableFrom<List<InvestorDataRowGroupedDTO>>(okResult.Value as List<InvestorDataRowGroupedDTO> ?? new List<InvestorDataRowGroupedDTO>());
        Assert.NotEmpty(data);
    }

    [Fact]
    public async Task GetDataByInvestor_ReturnsInvestorCommitments()
    {
        var controller = new MockInvestorsDataController();

        var result = await controller.GetDataByInvestor("Ioo Gryffindor fund", "All");

        var okResult = Assert.IsType<OkObjectResult>(result);
        var data = Assert.IsAssignableFrom<List<CommitmentDTO>>(okResult.Value as List<CommitmentDTO> ?? new List<CommitmentDTO>());
        Assert.NotEmpty(data);
    }

    [Fact]
    public async Task GetDataByInvestor_WithAssetClassFilter_ReturnsFilteredCommitments()
    {
        var controller = new MockInvestorsDataController();

        var result = await controller.GetDataByInvestor("Ioo Gryffindor fund", "Hedge Funds");

        var okResult = Assert.IsType<OkObjectResult>(result);
        var data = Assert.IsAssignableFrom<List<CommitmentDTO>>(okResult.Value as List<CommitmentDTO> ?? new List<CommitmentDTO>());
        Assert.Single(data); // Only one matching asset class in mock data
    }

    [Fact]
    public async Task GetInvestorSummaries_ReturnsSummary()
    {
        var controller = new MockInvestorsDataController();

        var result = await controller.GetInvestorSummaries("Ioo Gryffindor fund");

        var okResult = Assert.IsType<OkObjectResult>(result);
        var data = Assert.IsAssignableFrom<Dictionary<string, decimal>>(okResult.Value);
        Assert.True(data.ContainsKey("All"));
        Assert.True(data["All"] > 0);
    }
}