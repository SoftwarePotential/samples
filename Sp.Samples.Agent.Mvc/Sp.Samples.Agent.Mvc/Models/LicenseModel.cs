using System;

namespace Sp.Samples.Agent.Mvc.Models
{
	public class LicenseModel
	{
		public string ActivationKey { get; set; }
		public DateTime? ExpirationDate { get; set; }
		public string Features { get; set; }
	}
}