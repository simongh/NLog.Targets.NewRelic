using System.Collections.Generic;

namespace NLog.Targets.NewRelic
{
	internal class Log
	{
		public long Timestamp { get; set; }

		public Dictionary<string, object> Attributes { get; set; } = new Dictionary<string, object>();

		public string Message { get; set; }
	}
}