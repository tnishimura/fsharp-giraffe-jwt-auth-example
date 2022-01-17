module GiraffeJWTAuthExample.App

open System
open System.IO
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Cors.Infrastructure
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Microsoft.Extensions.DependencyInjection
open Giraffe
open Microsoft.AspNetCore.Authentication.JwtBearer 
open Microsoft.AspNetCore.Authorization 
open Microsoft.IdentityModel.Tokens // for SymmetricSecurityKey
open System.Text // for Encoding
open System.Security.Claims // For Claim, ClaimTypes

// ---------------------------------
// Web app
// ---------------------------------

let authenticate =
    requiresAuthentication (challenge JwtBearerDefaults.AuthenticationScheme >=> text "please authenticate")

let requireAdminRole : HttpHandler = 
    requiresRole "Admin" (RequestErrors.FORBIDDEN  "Permission denied. You must be an admin.")

let requireUserRole : HttpHandler = 
    requiresRoleOf ["Admin" ; "User"] (RequestErrors.FORBIDDEN  "Permission denied. You must be an admin or user.")

let requireAdminPolicy : HttpHandler = 
    authorizeByPolicyName "AdminPolicy" (RequestErrors.FORBIDDEN  "Permission denied. Failed Admin Policy")

let requireMustBe21ByPolicy : HttpHandler = 
    authorizeByPolicyName "MustBe21" (RequestErrors.FORBIDDEN  "Permission denied. Not old enough!")

let userMinimumAge (minAge : int) (user : ClaimsPrincipal) = 
    match user.Claims |> Seq.tryFind (fun c -> c.Type = ClaimTypes.DateOfBirth) with
    | Some dobClaim  -> 
        let dob = Convert.ToDateTime(dobClaim.Value)
        let age = DateTime.Today.Year - dob.Year
        // account for leap year https://stackoverflow.com/a/1404/
        let correctedAge = if dob > DateTime.Today.AddYears(age) then age - 1 else age
        correctedAge >= minAge
    | None -> 
        false

let requireMustBe21Manually : HttpHandler = 
    fun next ctx -> 
        if userMinimumAge 21 ctx.User then
            next ctx
        else 
            RequestErrors.FORBIDDEN "You have to be at least 21" next ctx

let webApp =
    choose [
        GET >=>
            choose [
                route "/" >=> text "unprotected content accessible by anyone"
                authenticate >=> choose [
                    route "/admin"         >=> requireAdminRole        >=> text "welcome, admin"
                    route "/user"          >=> requireUserRole         >=> text "welcome, user"
                    route "/tavern-claims" >=> requireMustBe21Manually >=> text "welcome, here's your gin and tonic!"
                    route "/admin-policy"  >=> requireAdminPolicy      >=> text "welcome, admin (by policy)"
                    route "/tavern-policy" >=> requireMustBe21ByPolicy >=> text "welcome, here's your gin and tonic and policy!"
                ]
            ]
        setStatusCode 404 >=> text "Not Found" ]

// ---------------------------------
// Error handler
// ---------------------------------

let errorHandler (ex : Exception) (logger : ILogger) =
    logger.LogError(ex, "An unhandled exception has occurred while executing the request.")
    clearResponse >=> setStatusCode 500 >=> text ex.Message

// ---------------------------------
// Config and Main
// ---------------------------------

let configureCors (builder : CorsPolicyBuilder) =
    builder
        .WithOrigins(
            "http://localhost:5000",
            "https://localhost:5001")
       .AllowAnyMethod()
       .AllowAnyHeader()
       |> ignore

let configureApp (app : IApplicationBuilder) =
    let env = app.ApplicationServices.GetService<IWebHostEnvironment>()
    (match env.IsDevelopment() with
    | true  ->
        app.UseDeveloperExceptionPage()
    | false ->
        app .UseGiraffeErrorHandler(errorHandler)
            .UseHttpsRedirection())
        .UseCors(configureCors)
        .UseStaticFiles()
        .UseAuthentication() // Before UseGiraffe
        .UseGiraffe(webApp)

let key = "pleaseReplaceWithYourSecretKeyRetrievedFromSomeSecureLocation"
let issuer = "https://certificateissuer.example.com/"
let audience = "https://backendserver.example.com"

let configureServices (services : IServiceCollection) =
    services.AddAuthentication(fun opt -> 
        opt.DefaultAuthenticateScheme <- JwtBearerDefaults.AuthenticationScheme
    ).AddJwtBearer(fun (opt : JwtBearerOptions )-> 
        // https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.authentication.jwtbearer.jwtbeareroptions?view=aspnetcore-5.0

        opt.TokenValidationParameters <- TokenValidationParameters(
            // https://docs.microsoft.com/en-us/dotnet/api/microsoft.identitymodel.tokens.tokenvalidationparameters?view=azure-dotnet
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            ValidIssuer = issuer,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = audience
        )) |> ignore

    services.AddAuthorization(fun (options : AuthorizationOptions) ->
        options.AddPolicy( "AdminPolicy", fun policy -> 
            policy.RequireClaim(ClaimTypes.Role, "Admin") |> ignore 
            // you can require other claims as well here
        ) |> ignore
        options.AddPolicy("MustBe21", fun policy -> 
            policy.RequireAssertion(
                fun (context : AuthorizationHandlerContext) -> 
                    userMinimumAge 21 context.User
            ) |> ignore
        ) |> ignore
    ) |> ignore

    services.AddCors()    |> ignore
    services.AddGiraffe() |> ignore

let configureLogging (builder : ILoggingBuilder) =
    builder.AddConsole()
           .AddDebug() |> ignore

[<EntryPoint>]
let main args =
    let contentRoot = Directory.GetCurrentDirectory()
    let webRoot     = Path.Combine(contentRoot, "WebRoot")
    Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(
            fun webHostBuilder ->
                webHostBuilder
                    .UseContentRoot(contentRoot)
                    .UseWebRoot(webRoot)
                    .Configure(Action<IApplicationBuilder> configureApp)
                    .ConfigureServices(configureServices)
                    .ConfigureLogging(configureLogging)
                    |> ignore)
        .Build()
        .Run()
    0
