using BlackRockTask.Server.Models;

namespace BlackRock.Server.Services
{
    public interface IFileService
    {
        public List<InvestorDataRow> ReadCSV();
    }

    public class FileService : IFileService
    {
        public virtual List<InvestorDataRow> ReadCSV()
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
