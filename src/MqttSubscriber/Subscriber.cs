// See https://aka.ms/new-console-template for more information
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Formatter;

Console.WriteLine("Subscriber");

var mqttFactory = new MqttFactory();
var client = mqttFactory.CreateMqttClient();

var options = new MqttClientOptionsBuilder()
                    .WithTimeout(TimeSpan.FromSeconds(600))
                    .WithProtocolVersion(MqttProtocolVersion.V310)
                    .WithClientId(Guid.NewGuid().ToString())
                    .WithKeepAlivePeriod(TimeSpan.FromSeconds(6000))
                    .WithTcpServer("broker.hivemq.com", 1883)
                    .WithCleanSession()
                    .Build();

client.ConnectedAsync += e =>
{
    Console.WriteLine("Connected");
    var topicFilter = new MqttTopicFilterBuilder()
                        .WithTopic("test/write")                        
                        .Build();

    client.SubscribeAsync(topicFilter);

    return Task.CompletedTask;
};

client.DisconnectedAsync += e =>
{
    Console.WriteLine("Disconnected");
    return Task.CompletedTask;
};

client.ApplicationMessageReceivedAsync += e =>
{
    Console.WriteLine($"Received Message -> {e.ApplicationMessage.ConvertPayloadToString()}");
    return Task.CompletedTask;
};

await client.ConnectAsync(options);

Console.ReadLine();

await client.DisconnectAsync();
