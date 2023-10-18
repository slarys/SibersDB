using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using Sibers.BL;
using Sibers.BL.Factories;
using Sibers.Domain;
using Sibers.Entities.Interfaces;
using Sibers.Entities.Models;
using SibersApi;
using System.Text.Json.Serialization;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<SibersContext>(opt => opt.UseInMemoryDatabase("Test"));

builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
builder.Services.AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>));

builder.Services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();

builder.Services.AddMediator();

builder.Services.AddAutoMapper(typeof(ProjectProfile));

builder.Services.AddSingleton<IProjectFactory, ProjectFactory>();

var modelBuilder = new ODataConventionModelBuilder();
modelBuilder.EntitySet<Project>("projects");

builder.Services.AddControllers().AddOData(options => options
        .SkipToken()
        .EnableQueryFeatures(50)
        .AddRouteComponents("api", modelBuilder.GetEdmModel()))
    .AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "AspNet6WithOData", Version = "v1" });
});
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sibers"));
}

await app.Services.SeedData();

app.UseRouting();

app.UseEndpoints(endpoints => endpoints.MapControllers());

app.Run();