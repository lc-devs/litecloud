using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WashALoadService.Common
{
    public class Common
    {
		public Common() { }

		public enum LaundryStatus : ushort
		{
			All = 0,
			InProgress = 1,
			Completed = 2,
			ForwardedToLogistics = 3,
			ForReceive = 4,
			Received = 5
		}

		public enum BillingType : ushort
		{
			ByBillingReference = 0,
			ByInvoice = 1,
			ByFloat = 2
		}
	}
}
