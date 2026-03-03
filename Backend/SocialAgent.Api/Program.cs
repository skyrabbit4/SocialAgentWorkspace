using Tweetinvi;
using Tweetinvi.Models;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SocialAgent.Api.Data;
using Microsoft.Extensions.DependencyInjection;
using SocialAgent.Api.Models;





var builder = WebApplication.CreateBuilder(args);
string apiKey = builder.Configuration["AI:ApiKey"] ?? "";
var kernelBuilder = Kernel.CreateBuilder();
kernelBuilder.AddOpenAIChatCompletion("gpt-4o-mini",apiKey);
var kernel=kernelBuilder.Build();
builder.Services.AddSingleton(kernel);
builder.Services.AddDbContext<AppDbContext>(options=>
options.UseSqlite("Data Source=socialagent.db"));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowedAngularDev",
    policy =>
    {
        policy.WithOrigins("http://localhost:4200")
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

var app = builder.Build();
app.UseCors("AllowedAngularDev");


// Configure the HTTP request pipeline..\
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



app.MapGet("/api/status", () =>
{
    return new {message="Api is sending the data"};
})
.WithName("GetStatus")
.WithOpenApi();


app.MapGet("/api/chat" ,async(string userPrompt,Kernel kernel) =>
{
    var chatService=kernel.GetRequiredService<IChatCompletionService>();
    var history=new ChatHistory();
    history.AddSystemMessage("You are Social Media Agent for Shubham. You are witty, tech-savvy, and knowledgeable about .NET, Angular, and F1. Help him draft engaging content for X (Twitter)");
    history.AddUserMessage(userPrompt);
    var result=await chatService.GetChatMessageContentAsync(history);
    return new{answer=result.Content};
})
.WithName("GetAiResponse")
.WithOpenApi();

app.MapPost("/api/tweet", async (string tweetText, IConfiguration config,AppDbContext db) =>
{
    var client = new TwitterClient(
        config["X:ApiKey"] ?? "",
        config["X:ApiSecret"] ?? "",
        config["X:AccessToken"] ?? "",
        config["X:AccessSecret"] ?? ""
    );

    var result = await client.Execute.AdvanceRequestAsync(
        (ITwitterRequest request) =>
        {
            var jsonBody = client.Json.Serialize(new { text = tweetText });
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            request.Query.Url = "https://api.twitter.com/2/tweets";
            request.Query.HttpMethod = Tweetinvi.Models.HttpMethod.POST;
            request.Query.HttpContent = content;
        }
    );

    var post =new SocialPost
    {
        Content=tweetText,
        IsPublished=true,
        CreatedAt=DateTime.UtcNow
    };
    db.SocialPosts.Add(post);
    await db.SaveChangesAsync();

    return new { status = "Success" ,id=post.Id};
}); 

app.MapGet("/api/history",async(AppDbContext db) =>
{
    return await db.SocialPosts.OrderByDescending(p=>p.CreatedAt).ToListAsync();
});

app.Run();

