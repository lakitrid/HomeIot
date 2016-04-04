using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using WebSite.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebSite.Controllers
{
    [Route("services/[controller]")]
    public class DashboardController : Controller
    {
        // GET: api/values
        [HttpGet, Route("cards")]
        public IEnumerable<Card> Get()
        {
            Card[] cards = new Card[]
            {
                new Card {
                    Label = "HP",
                    Icon = "trending_up",
                    Value = 40000,
                    UnitLabel = "KWh",
                    ActionRoute = "Power"
                },

                new Card {
                    Label = "HC",
                    Icon = "trending_up",
                    Value = 42000,
                    UnitLabel = "KWh",
                    ActionRoute = "Power"
                }
            };

            return cards;
        }
    }
}
