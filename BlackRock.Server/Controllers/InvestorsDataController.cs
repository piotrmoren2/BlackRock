using BlackRock.Server.Services;
using BlackRockTask.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlackRockTask.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InvestorsDataController : ControllerBase
    {
        private IFileService _fileService;

        public InvestorsDataController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpGet]
        [Route("getSampleData")]
        public async Task<IActionResult> GetSampleData()
        {
            var result = new List<InvestorDataRowGroupedDTO>();

            try
            {
                var investorsData = _fileService.ReadCSV();
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
                var investorsData = _fileService.ReadCSV().Where(x => x.InvestorName == investorId).ToList();

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
                var investorsData = _fileService.ReadCSV().Where(x => x.InvestorName == investorId).ToList();
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

        
    }
}
