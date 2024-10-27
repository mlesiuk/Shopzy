using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Shopzy.Notifier.Abstractions.Interfaces;
using Shopzy.Notifier.Configurations;
using Shopzy.Notifier.Consumers;
using Shopzy.Notifier.Persistence;
using Shopzy.Notifier.Services;
using Quartz;
using Shopzy.Notifier.Job;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("Shopzy"),
    b =>
    {
        b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
        b.MigrationsHistoryTable(
            tableName: HistoryRepository.DefaultTableName,
            schema: "dbo");
    })
);

var rabbitMqCfg = new RabbitMqConfiguration();
configuration.GetSection(nameof(Configuration.RabbitMq)).Bind(rabbitMqCfg);

builder.Services.AddMassTransit(configuration =>
{
    configuration.SetKebabCaseEndpointNameFormatter();
    configuration.AddConsumer<UserCreatedConsumer>();
    configuration.UsingRabbitMq((context, cfg) =>
    {
        cfg.ConfigureEndpoints(context);
        cfg.Host(new Uri(rabbitMqCfg.Uri), rmhc =>
        {
            rmhc.Username(rabbitMqCfg.Username);
            rmhc.Password(rabbitMqCfg.Password);
        });
    });
});

builder.Services.AddQuartz(configure =>
{
    var jobKey = new JobKey(nameof(OutgoingMessageJob));
    configure
        .AddJob<OutgoingMessageJob>(jobKey)
        .AddTrigger(trigger =>
        {
            trigger
                .ForJob(jobKey)
                .WithSimpleSchedule(schedule =>
                {
                    schedule
                        .WithIntervalInSeconds(10)
                        .RepeatForever();
                });
        });
});

builder.Services.AddQuartzHostedService();

var emailSenderSection = configuration.GetSection(nameof(Configuration.EmailSender));
builder.Services.AddOptions<EmailSenderConfiguration>()
    .Bind(emailSenderSection)
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddScoped<IEmailSenderService, EmailSenderService>();

var app = builder.Build();
app.UseHttpsRedirection();
app.Run();
