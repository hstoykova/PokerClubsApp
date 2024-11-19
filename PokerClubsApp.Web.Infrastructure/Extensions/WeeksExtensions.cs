using Itenso.TimePeriod;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PokerClubsApp.Common.EntityValidationConstants.GameResult;

namespace PokerClubsApp.Web.Infrastructure.Extensions
{
    public static class WeeksExtensions
    {
        public static IList<SelectListItem> ToSelectList(this IEnumerable<Week> weeks)
        {
            List<SelectListItem> result = new();

            foreach (Week week in weeks) 
            {
                string value = $"{week.FirstDayOfWeek.ToString(FromDateFormat)} - {week.LastDayOfWeek.ToString(ToDateFormat)}";

                result.Add(new SelectListItem()
                {
                    Text = value,
                    Value = value
                });
            }

            return result;
        }
    }
}
