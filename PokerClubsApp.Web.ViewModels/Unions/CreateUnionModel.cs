using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PokerClubsApp.Common.EntityValidationConstants.Union;

namespace PokerClubsApp.Web.ViewModels.Unions
{
    public class CreateUnionModel
    {
        [Required]
        [MinLength(UnionNameMinLength)]
        [MaxLength(UnionNameMaxLength)]
        public string Name { get; set; } = null!;
    }
}
