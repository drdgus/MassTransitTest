using MassTransit;
using MassTransitTest;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

////////////////PUBLISHER////////////////////
builder.Services.AddMassTransit(x =>
{
    x.UsingInMemory((context, cfg) =>
    {
        cfg.ConfigureEndpoints(context);
    });

    x.AddRider(rider =>
    {
        rider.AddProducer<MessageProcessed>(nameof(MessageProcessed));
        //rider.AddProducer<MessageType>(nameof(TopicName));

        rider.UsingKafka((context, k) => { k.Host("192.168.1.8:9094"); });
    });
});
////////////////PUBLISHER////////////////////

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();