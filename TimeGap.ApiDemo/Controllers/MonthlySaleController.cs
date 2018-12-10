using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TimeGap.ApiDemo.Dtos;

namespace TimeGap.ApiDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MonthlySaleController : ControllerBase
    {
        private static readonly List<MonthlySale> MonthlySales = new List<MonthlySale>
        {
            new MonthlySale {Year = 2018, Month = 1, Value = 100},
            new MonthlySale {Year = 2018, Month = 5, Value = 200},
            new MonthlySale {Year = 2018, Month = 12, Value = 300},
            new MonthlySale {Year = 2019, Month = 2, Value = 400},
            new MonthlySale {Year = 2019, Month = 11, Value = 500},
            new MonthlySale {Year = 2020, Month = 10, Value = 600}
        };
        
        // GET api/monthlysale
        [HttpGet]
        public ActionResult<IEnumerable<MonthlySale>> Get([Required] DuodecimDate fromDuodecimDate,
            [Required] DuodecimDate toDuodecimDate)
        {
            var salesForYearFromDatabase = MonthlySales
                .Where(monthlySale => monthlySale.Year >= fromDuodecimDate.Year)
                .Where(monthlySale => monthlySale.Year <= toDuodecimDate.Year)
                .ToList();

            var matchingSales = salesForYearFromDatabase
                .Where(monthlySale => monthlySale.DuodecimDate >= fromDuodecimDate)
                .Where(monthlySale => monthlySale.DuodecimDate <= toDuodecimDate);

            return Ok(matchingSales);
        }
    }
}