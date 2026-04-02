using Microsoft.EntityFrameworkCore;
using CineRank.Data;
using CineRank.Services; // Ajuste para o namespace real da sua pasta Data

var builder = WebApplication.CreateBuilder(args);

// 1. Configuração do Banco de Dados (O CORAÇÃO DO PROBLEMA)
// Esta linha resolve o erro "Unable to resolve service..."
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConexaoPadrao")));

// 2. Registrar os Services para Injeção de Dependência
builder.Services.AddScoped<FilmeService>();
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<AvaliacaoService>();

// 3. Adiciona serviços ao contêiner (Controllers e Swagger)
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Esta linha mágica impede o loop infinito
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 4. Configura o pipeline de requisições HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();