using MassTransit;
using MassTransitTest;
using Microsoft.AspNetCore.HttpLogging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddLogging(opt =>
{
    opt.AddSimpleConsole(c =>
    {
        c.TimestampFormat = "[HH:mm:ss.fff zzz] ";
    });
});

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;

});

////////////////CONSUMER////////////////////
builder.Services.AddMassTransit(x =>
{
    x.UsingInMemory((context, cfg) =>
    {
        cfg.ConfigureEndpoints(context);
    });

    x.AddRider(rider =>
    {
        rider.AddConsumers(typeof(Program).Assembly);

        rider.UsingKafka((context, k) =>
        {
            k.Host("192.168.1.8:9094");

            k.TopicEndpoint<MessageProcessed>(nameof(MessageProcessed), BrokerGroups.Messages, e =>
            {
                e.ConfigureConsumer<MessageProcessedConsumer>(context);
            });

            //k.TopicEndpoint<MessageType>(nameof(TopicName), BrokerGroups.MyGroupForThisTypeOfMessages, e =>
            //{
            //    e.ConfigureConsumer<MessageConsumer>(context);
            //});
        });
    });
});
////////////////CONSUMER////////////////////

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseHttpLogging();
// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();


public static class BrokerGroups
{
    public static string Messages => nameof(Messages);
}