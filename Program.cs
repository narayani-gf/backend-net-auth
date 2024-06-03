using Microsoft.EntityFrameworkCore;
using backendnet.Data;
using backendnet.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using backendnet.Middlewares;
using backendnet.Services;

var builder = WebApplication.CreateBuilder(args);

//Soporte para generar JWT
builder.Services.AddScoped<JwtTokenService>();

//Agregar el soporte para MySQL
var connectionString = builder.Configuration.GetConnectionString("DataContext");
builder.Services.AddDbContext<IdentityContext>(options =>{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

//Soporte para identity
builder.Services.AddIdentity<CustomIdentityUser, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
    //Cambiar como manejar contrasenas
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
})
.AddEntityFrameworkStores<IdentityContext>();

//Soporte para JWT
builder.Services
    .AddHttpContextAccessor() //Para acceder al HttpContext()
    .AddAuthorization() // Para autorizar en cada método el acceso
    .AddAuthentication(options => //Para autenticar con JWT
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"], //Leido desde appSettings
            ValidAudience = builder.Configuration["JWT:Audience"], //Leido desde appSetting
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!))
        };
    });
//Soporte cors
builder.Services.AddCors(options =>{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins("http://localhost:3001", "http://localhost:8080")
                              .AllowAnyHeader()
                              .WithMethods("GET", "POST", "PUT", "DELETE");
        }
    );
});

//Funcionalidad de controladores
builder.Services.AddControllers();
//DOcumentacion de api
builder.Services.AddSwaggerGen();
//Construye aplicacion web
var app = builder.Build();
//Mostrar documentacion solo en ambiente de desarrollo
if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Middleware para manejo errores
app.UseExceptionHandler("/error");
//rutas para endpotins de controladores
app.UseRouting();

//Autenticacion
app.UseAuthentication();
//Autorizacion
app.UseAuthorization();
//Middleware de refresco token
app.UseSlidingExpirationJwt();

app.UseCors();
//Uso de rutas sin una default
app.MapControllers();

app.Run();
