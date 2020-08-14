using Newtonsoft.Json;
using System.Collections.Generic;

namespace NLog.Targets.NewRelic
{
	internal class Message
	{
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public Common Common { get; set; }

		public ICollection<Log> Logs { get; set; } = new List<Log>();
	}
}