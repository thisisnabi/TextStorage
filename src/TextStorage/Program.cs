using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TextStorage;
using TextStorage.Features.PasteText;
using TextStorage.Features.Reading;
using TextStorage.Persistence;
using TextStorage.Workers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

builder.Services.AddSingleton(sp => {

    var connections = sp.GetRequiredService<IOptions<AppSettings>>().Value.ConnectionStrings;
    return new LoadBalancer([connections.Master1, connections.Master2, connections.Master3]);
});
 
builder.Services.AddScoped((sp) =>
{
    var balancer = sp.GetRequiredService<LoadBalancer>();
    var tenant = balancer.MoveNext();
    return new TenantPrincipal
    {
        ConnectionString = tenant.ConnectionString,
        Id = tenant.Id,
        Predicate = tenant.Predicate
    };
});

builder.Services.AddDbContext<TextStorageDbContext>((sp, configure) =>
{
    var tenant = sp.GetRequiredService<TenantPrincipal>();
    configure.UseSqlServer(tenant.ConnectionString);
});


builder.Services.AddStackExchangeRedisCache((configure) =>
{
    configure.Configuration = "localhost:6379";
});

builder.Services.AddHostedService<CleanUpPastesBackgroundService>();
 
builder.Services.AddDbContext<TextStorageDbContextReadOnly>((sp, configure) =>
{
    var httpContext = sp.GetRequiredService<IHttpContextAccessor>();
    var routeCode = httpContext.HttpContext.Request.RouteValues["code"].ToString();
    var balancer = sp.GetRequiredService<LoadBalancer>();

    var connectionString = balancer.GetConnectionStringByPredicateId(routeCode.First());

    configure.UseSqlServer(connectionString)
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<AppSettings>(builder.Configuration);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapPasteTextEndpoint();
app.MapReadingEndpoint();

app.Run();