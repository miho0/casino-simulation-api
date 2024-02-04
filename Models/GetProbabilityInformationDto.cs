namespace CasinoSimulationApi.Models
{
    public class GetProbabilityInformationDto
    {
        public decimal InitialBalance { get; set; }
        public decimal BettingAmount { get; set; }
        public decimal Goal { get; set; }
        public int Itterations { get; set; }

        public GetProbabilityInformationDto(decimal initialBalance, decimal bettingAmount, decimal goal, int itterations)
        {
            InitialBalance = initialBalance;
            BettingAmount = bettingAmount;
            Goal = goal;
            Itterations = itterations;
        }
    }
}
