using System.IdentityModel.Tokens.Jwt;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = "Cookies";
        options.DefaultChallengeScheme = "oidc";
    })
    .AddCookie("Cookies")
    .AddOpenIdConnect("oidc", options =>
    {
        options.Authority = "http://localhost:5000";
        options.RequireHttpsMetadata = false;

        options.ClientId = "mvc";
        options.ClientSecret = "secret";
        options.ResponseType = "code";

        options.SaveTokens = true;

        options.Scope.Add("api1");
        options.Scope.Add("offline_access");
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
else  app.UseDeveloperExceptionPage();

app.UseStaticFiles();

app.UseRouting();
app.UseCookiePolicy(new CookiePolicyOptions
{
    Secure = CookieSecurePolicy.Always
});
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute()
        .RequireAuthorization();
});

app.Run();
