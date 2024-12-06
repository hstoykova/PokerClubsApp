using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerClubsApp.Common
{
	public class ValidationException : Exception
	{
		public string Field { get; set; } = string.Empty;

		public ValidationException(string message, string field = "")
			: base(message)
		{
			this.Field = field;
		}
	}
}
