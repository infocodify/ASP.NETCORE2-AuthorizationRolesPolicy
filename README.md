# ASP.NETCORE2-AuthorizationRolesPolicy
#### Note: Start learning https://github.com/infocodify/ASP.NETCORE2-IdentityAuthorization and then the actual tutorial

Allow access to .Net Core 2 Web Application by Roles and Policy

1.  Create Policy in Sartup.cs file
```csharp
          // Policy allow only users from a specific country
            services.AddAuthorization(configure =>
            {
                //Policy name = CanadianOnly
                //configure.AddPolicy("CanadianOnly", policy => 
                //policy.RequireClaim(ClaimTypes.Country,"Canada"));

                //Policy name = CanadianOrAdmin, allow only Canadian or Admin
                configure.AddPolicy("CanadianOrAdmin", policy =>
                policy.AddRequirements(new CanadianRequirement()));
            });
```

2.  Add authorization by policy in the Controller
```csharp
 [Authorize(Policy = "CanadianOrAdmin")]
    public class TutorialsModel : PageModel
    {
        public string Message { get; set; }

        public void OnGet()
        {
            Message = "Your application description page.";
        }
    }
```

3.  Inside Pages create Requirements folder, create CanadianRequirement.cs and add the following code
```csharp
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityAuth
{
    internal class CanadianRequirement : AuthorizationHandler<CanadianRequirement>, 
        IAuthorizationRequirement
    {
        public CanadianRequirement()
        {

        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, 
            CanadianRequirement requirement)
        {
            if(context.User.IsInRole("Admin"))
            {
                context.Succeed(requirement);
            }

            if(context.User.HasClaim(claim => 
            claim.ValueType == ClaimTypes.Country && claim.Value == "Canada"))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
```

4.  Open the _Layout, inject authorization and implement the if condition to make visible the Tutorials page after Login
```csharp
@inject IAuthorizationService AuthorizationService
@using Microsoft.AspNetCore.Authorization

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>@ViewData["Title"] - IdentityAuth</title>

    <environment include="Development">
        <link rel="stylesheet" href="~/lib/bootstrap/dist/css/bootstrap.css" />
        <link rel="stylesheet" href="~/css/site.css" />
    </environment>
    <environment exclude="Development">
        <link rel="stylesheet" href="https://ajax.aspnetcdn.com/ajax/bootstrap/3.3.7/css/bootstrap.min.css"
              asp-fallback-href="~/lib/bootstrap/dist/css/bootstrap.min.css"
              asp-fallback-test-class="sr-only" asp-fallback-test-property="position" asp-fallback-test-value="absolute" />
        <link rel="stylesheet" href="~/css/site.min.css" asp-append-version="true" />
    </environment>
</head>
<body>
    <nav class="navbar navbar-inverse navbar-fixed-top">
        <div class="container">
            <div class="navbar-header">
                <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-collapse">
                    <span class="sr-only">Toggle navigation</span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                </button>
                <a asp-page="/Index" class="navbar-brand">IdentityAuth</a>
            </div>
            <div class="navbar-collapse collapse">
                <ul class="nav navbar-nav">
                    <li><a asp-page="/Index">Home</a></li>

                    @if((await AuthorizationService.AuthorizeAsync(User, "CanadianOrAdmin")).Succeeded){

                        <li><a asp-page="/Tutorials">Tutorials</a></li>
                    }
  
                    <li><a asp-page="/Contact">Contact</a></li>
                </ul>
                @await Html.PartialAsync("_LoginPartial")
            </div>
        </div>
    </nav>
    <div class="container body-content">
        @RenderBody()
        <hr />
        <footer>
            <p>&copy; 2017 - IdentityAuth</p>
        </footer>
    </div>

    <environment include="Development">
        <script src="~/lib/jquery/dist/jquery.js"></script>
        <script src="~/lib/bootstrap/dist/js/bootstrap.js"></script>
        <script src="~/js/site.js" asp-append-version="true"></script>
    </environment>
    <environment exclude="Development">
        <script src="https://ajax.aspnetcdn.com/ajax/jquery/jquery-2.2.0.min.js"
                asp-fallback-src="~/lib/jquery/dist/jquery.min.js"
                asp-fallback-test="window.jQuery"
                crossorigin="anonymous"
                integrity="sha384-K+ctZQ+LL8q6tP7I94W+qzQsfRV2a+AfHIi9k8z8l9ggpc8X+Ytst4yBo/hH+8Fk">
        </script>
        <script src="https://ajax.aspnetcdn.com/ajax/bootstrap/3.3.7/bootstrap.min.js"
                asp-fallback-src="~/lib/bootstrap/dist/js/bootstrap.min.js"
                asp-fallback-test="window.jQuery && window.jQuery.fn && window.jQuery.fn.modal"
                crossorigin="anonymous"
                integrity="sha384-Tc5IQib027qvyjSMfHjOMaLkfuWVxZxUPnCJA7l2mCWNIpG9mGCD8wGNIcPD7Txa">
        </script>
        <script src="~/js/site.min.js" asp-append-version="true"></script>
    </environment>

    @RenderSection("Scripts", required: false)
</body>
</html>

```
