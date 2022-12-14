// See https://aka.ms/new-console-template for more information
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Formatter;

Console.WriteLine("Publisher");

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
    return Task.CompletedTask;
};

client.DisconnectedAsync += e =>
{
    Console.WriteLine("Disconnected");
    return Task.CompletedTask;
};

await client.ConnectAsync(options);

Console.WriteLine("Please press a key to publish a message");
Console.ReadLine();

string messagePayload = "Example";
var message = new MqttApplicationMessageBuilder()
    .WithTopic("test/write")
    .WithPayload(messagePayload)
    .Build();

if (client.IsConnected)
{
    await client.PublishAsync(message, CancellationToken.None);
}

await client.DisconnectAsync();


