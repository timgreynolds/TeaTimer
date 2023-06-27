global using System;
using com.mahonkin.tim.TeaApi.DataModel;
using com.mahonkin.tim.TeaApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddLogging();
builder.Services.AddControllers();
builder.Services.AddSingleton<IDataService<TeaModel>, TeaSqlService<TeaModel>>();

WebApplication app = builder.Build();
app.MapControllers();
app.Run();