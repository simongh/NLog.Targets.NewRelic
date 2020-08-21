# NLog.Targets.NewRelic
[![Build status](https://ci.appveyor.com/api/projects/status/sdnvx7eo58o1dv2g?svg=true)](https://ci.appveyor.com/project/SimonHalsey/nlog-targets-newrelic)


An NLog target for the NewRelic logging platform.
This target allos you to ship logs to the NewRelic platform. You'll need to make sure logging is included as part of your subscription. Information on logging in NewRelic can be found [Here](https://docs.newrelic.com/docs/logs/log-management/get-started/get-started-log-management)

The endpoint location can be set to EU if that's where you're located.

System allows to identify the system generating log messages.

Multiple ContextProperty elements can be defined. These are sent as attributes to NewRelic. The layout will be rendered by NLog before it's sent.

Switch LogNamedProperties to true to have property names nad values logged as attributes. This allows you to use the structured logging aspects of NLog.

Install pacakge from from [NuGet](https://www.nuget.org/packages/nlog.targets.newrelic/)

## Sample Configuration

```xml
<extensions>
    <add assembly="nlog.targets.newrelic" />
</extensions>

<targets>
    <target xsi:type="NewRelic" name="newrelic" layout="${message}" licenceKey="your licence key" endpointLocation="EU|US" system="test system" logNamedProperties="false">
      <ContextProperty name="environment" layout="test" />
    </target>
</targets>
```
