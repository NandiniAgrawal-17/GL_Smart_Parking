using SmartParking.DataAccess.Services.AdminServices;
using SmartParking.DataAccess.Services.EmployeeServices;
using SmartParking.DataAccess.Services;
using SmartParking.Service.Interface.AdminInterface;
using SmartParking.Service.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using SmartParking.Service.Interface.EmployeeInterface;
using SmartParking.Service.Interface.OperatorInterface;
using SmartParking.DataAccess.Services.OperatorServices;
using AspNetCoreRateLimit;
using SmartParking.Service.Interface.VehicleInterface;
using SmartParking.DataAccess.Services.VehicleServices;
{

    var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


#region Dependency Injection

builder.Services.AddScoped<IAdminAuthentication, AdminAuthentication>();
builder.Services.AddScoped<IEmployeeAuthentication, EmployeeAuthentication>();
builder.Services.AddScoped<IOperatorAuthentication, OperatorAuthentication>();
builder.Services.AddScoped<IGetEmployeeInformation, GetEmployeeDetails>();
builder.Services.AddScoped<IViewEmployeeDetails, ViewEmployeeDetails>();
builder.Services.AddScoped<IViewOperatorDetails, ViewOperatorDetails>();
builder.Services.AddScoped<IUnbookSlot, UnbookSlot>();
builder.Services.AddScoped<IAssign, AssignService>();
builder.Services.AddScoped<ICount, SlotCountService>();
builder.Services.AddScoped<IManualVehicleNumber, EnterVehicleNumberManually>();
builder.Services.AddScoped<IRegisterVehicles, RegisterNewVehicle>();
builder.Services.AddScoped<IGetVehicleInformation,GetVehicleInformation>();
builder.Services.AddScoped<ViewSlotHistory, GenerateSlotReport>();
builder.Services.AddScoped<IBookSlotOnPriority, BookSlotOnSpecialRequest>();
builder.Services.AddScoped<IAddSlot, AddSlotsRequired>();

    #endregion


    var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);
//builder.Services.AddSingleton(TokenValidationParameters);
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
    };
});

builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
                     {
                         {
                               new OpenApiSecurityScheme
                                 {
                                     Reference = new OpenApiReference
                                     {
                                         Type = ReferenceType.SecurityScheme,
                                         Id = "Bearer"
                                     }
                                 },
                                 new string[] {}
                          }
      });

});





//brute force attack services
builder.Services.AddMemoryCache();

builder.Services.Configure<IpRateLimitOptions>(options =>
{
    options.GeneralRules = new List<RateLimitRule>
            {
                new RateLimitRule
                {
                    Endpoint = "*",
                    Limit = 10, // Number of requests allowed within the time span
                    Period = "1m" // Time span for rate limit (1 minute in this example)
                }
            };
});

builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();






var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
    }