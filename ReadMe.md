# NLog.Targets.NewRelic

An NLog target for the NewRelic logging platform.
This target allos you to ship logs to the NewRelic platform. You'll need to make sure logging is included as part of your subscription. Information on logging in NewRelic can be found [Here](https://docs.newrelic.com/docs/logs/log-management/get-started/get-started-log-management)

The endpoint location can be set to EU if that's where you're located.

Multiple ContextProperty elements can be defined. These are sent as attributes to NewRelic. The layout will be rendered by NLog before it's sent.

## Sample Configuration

```xml
<extensions>
    <add assembly="nlog.targets.newrelic" />
</extensions>

<targets>
    <target xsi:type="NewRelic" name="newrelic" layout="${message}" licenceKey="your licence key" endpointLocation="EU|US">
      <ContextProperty name="environment" layout="test" />
    </target>
</targets>
```