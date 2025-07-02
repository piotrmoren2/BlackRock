using BlackRockTask.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlackRockTask.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InvestorsDataController : ControllerBase
    {

        [HttpGet]
        [Route("getSampleData")]
        public async Task<IActionResult> GetSampleData()
        {
            var result = new List<InvestorDataRowGroupedDTO>();

            try
            {
                var investorsData = ReadCSV();
                result = investorsData
                    .GroupBy(x => new
                    {
                        x.InvestorName,
                        x.InvestorType,
                        x.InvestorDateAdded,
                        x.InvestorCountry
                    },
                    y => y,
                    (key, obj) => new InvestorDataRowGroupedDTO
                    {
                        DateAdded = key.InvestorDateAdded.ToShortDateString(),
                        Name = key.InvestorName,
                        Address = key.InvestorCountry,
                        Type = key.InvestorType,
                        TotalCommitment = obj.Sum(x => x.CommitmentAmount)
                    }).ToList();
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                Console.Write(ex.StackTrace);
                return StatusCode(500, new { message = ex.Message });
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("getByInvestor/{investorId}/{assetClass}")]
        public async Task<IActionResult> GetDataByInvestor([FromRoute] string investorId, [FromRoute] string assetClass)
        {
            try
            {
                var investorsData = ReadCSV().Where(x => x.InvestorName == investorId).ToList();

                if (assetClass != "All")
                {
                    investorsData = investorsData.Where(x => x.CommitmentAssetClass == assetClass).ToList();
                }
                var result = investorsData.Where(x => x.InvestorName == investorId).Select(x => x.ToCommitmentDTO()).ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.Write(ex.StackTrace);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet]
        [Route("getInvestorSummaries/{investorId}")]
        public async Task<IActionResult> GetInvestorSummaries([FromRoute] string investorId)
        {
            try
            {
                var investorsData = ReadCSV().Where(x => x.InvestorName == investorId).ToList();
                var result = new Dictionary<string, decimal>();
                
                result.Add("All", investorsData.Sum(x => x.CommitmentAmount));                
                investorsData.GroupBy(x => x.CommitmentAssetClass).ToDictionary(x=>x.Key, y=>y.Sum(x=>x.CommitmentAmount)).ToList().ForEach(x=> result.Add(x.Key, x.Value));

                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.Write(ex.StackTrace);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        private List<InvestorDataRow> ReadCSV()
        {
            var result = new List<InvestorDataRow>();
            using (var reader = new StreamReader("./SampleData/data.csv"))
            {
                // Skip header line
                reader.ReadLine();

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    if (values.Length == 8)
                    {
                        var investor = new InvestorDataRow
                        {
                            InvestorName = values[0].Trim(),
                            InvestorType = values[1].Trim(),
                            InvestorCountry = values[2].Trim(),
                            InvestorDateAdded = DateTime.Parse(values[3].Trim()),
                            InvestorLastUpdated = DateTime.Parse(values[4].Trim()),
                            CommitmentAssetClass = values[5].Trim(),
                            CommitmentAmount = decimal.Parse(values[6].Trim()),
                            CommitmentCurrency = values[7].Trim()
                        };
                        result.Add(investor);
                    }
                }
            }

            return result;
        }
    }
}
