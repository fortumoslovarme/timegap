using Newtonsoft.Json;

namespace TimeGap.ApiDemo.Dtos
{
    public class MonthlySale
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Value { get; set; }

        [JsonIgnore]
        public DuodecimDate DuodecimDate => new DuodecimDate(Year, Month);
    }
}
