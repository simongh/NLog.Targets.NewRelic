using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NLog.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace NLog.Targets.NewRelic
{
	public enum Location
	{
		US,
		EU
	}

	[Target("NewRelic")]
	public sealed class LoggingTarget : AsyncTaskTarget
	{
		private readonly HttpClient _client;
		private readonly JsonSerializerSettings _settings;

		public string Url => EndpointLocation == Location.US ? "https://log-api.newrelic.com/log/v1" : "https://log-api.eu.newrelic.com/log/v1";

		public Location EndpointLocation { get; set; }

		public string LicenceKey { get; set; }

		public string InsertKey { get; set; }

		public LoggingTarget()
		{
			_client = new HttpClient();
			_settings = new JsonSerializerSettings
			{
				ContractResolver = new CamelCasePropertyNamesContractResolver(),
			};
		}

		protected override Task WriteAsyncTask(LogEventInfo logEvent, CancellationToken cancellationToken)
		{
			return WriteAsyncTask(new[]
			{
				logEvent
			}, cancellationToken);
		}

		protected async override Task WriteAsyncTask(IList<LogEventInfo> logEvents, CancellationToken cancellationToken)
		{
			var message = new Message();

			foreach (var item in logEvents)
			{
				var logItem = new Log
				{
					TimeStamp = new DateTimeOffset(item.TimeStamp).ToUnixTimeSeconds(),
					Message = RenderLogEvent(Layout, item),
					Attributes = ContextProperties.ToDictionary(k => k.Name, v => (object)v.Layout.Render(item))
				};

				logItem.Attributes.Add("logger", item.LoggerName);
				logItem.Attributes.Add("level", item.Level);

				message.Logs.Add(logItem);
			}

			var requestMessage = new HttpRequestMessage(HttpMethod.Post, Url)
			{
				Content = new StringContent(JsonConvert.SerializeObject(message, _settings)),
			};

			if (!string.IsNullOrEmpty(InsertKey))
			{
				InternalLogger.Info("Using insert key");
				requestMessage.Headers.Add("X-Insert-Key", InsertKey);
			}
			else if (!string.IsNullOrEmpty(LicenceKey))
			{
				InternalLogger.Info("Using licence key");
				requestMessage.Headers.Add("X-License-Key", LicenceKey);
			}
			else
			{
				InternalLogger.Warn("No licence key found");
				return;
			}

			requestMessage.Content.Headers.ContentType.MediaType = "application/json";

			if (InternalLogger.IsTraceEnabled)
				InternalLogger.Trace("json:{0}", await requestMessage.Content.ReadAsStringAsync());

			var result = await _client.SendAsync(requestMessage, cancellationToken);
			if (result.IsSuccessStatusCode)
				return;

			InternalLogger.Error("Send failed: {0} - {1}", result.StatusCode, await result.Content.ReadAsStringAsync());
		}
	}
}