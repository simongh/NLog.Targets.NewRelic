using Newtonsoft.Json;
using System.Collections.Generic;

namespace NLog.Targets.NewRelic
{
	internal class Common
	{
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public Dictionary<string, string> Attributes { get; set; }
	}
}