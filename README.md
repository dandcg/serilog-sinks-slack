# serilog-sinks-slack

A Serilog sink that sends events to Slack.

### Usage

<pre>
    Serilog.ILogger log = new LoggerConfiguration()
        .MinimumLevel.Verbose()
        .WriteTo.Slack(webhookUrl: "https://xxxx", channel: "xxxx", userName: "xxxx");
        .CreateLogger();
</pre>
