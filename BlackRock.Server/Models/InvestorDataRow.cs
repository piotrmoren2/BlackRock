namespace BlackRockTask.Server.Models
{
    public class InvestorDataRow
    {
        public string InvestorName { get; set; }
        public string InvestorType { get; set; }
        public string InvestorCountry { get; set; }
        public DateTime InvestorDateAdded { get; set; }
        public DateTime InvestorLastUpdated { get; set; }
        public string CommitmentAssetClass { get; set; }
        public decimal CommitmentAmount { get; set; }
        public string CommitmentCurrency { get; set; }

        public CommitmentDTO ToCommitmentDTO()
        {
            return new CommitmentDTO()
            {
                Id = 1,
                AssetClass = CommitmentAssetClass,
                Currency = CommitmentCurrency,
                Amount = CommitmentAmount
            };
        }
    }


    public class InvestorDataRowGroupedDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string DateAdded { get; set; }
        public string Address { get; set; }
        public decimal TotalCommitment { get; set; }
    }

    public class CommitmentDTO
    {
        public int Id { get; set; }
        public string AssetClass { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
    }
}
