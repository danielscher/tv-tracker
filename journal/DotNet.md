## .NET Core Philosophy

[TODO add a sentence or two to introduce the subject matter of this section.]: # 

### ASP.NET web app architectures:

ASP.NET Core supports several ways to build web applications. Conceptually, they fall into server-side rendering and client-side (SPA) models.

- #### ASP.NET Core + SPA (React, Angular, Vue): 
    This is a typical layer separation of front- and backend  where Client SPA handles rendering and routing, and backend exposes an API.

- #### MVC:
    In this architecture UI components are declared and rendered on the server in by the View. Besides that there is little conceptual difference between this and the previous architecture in terms of the Model and controller parts.

- #### Razor Pages: 
    This architecture follows a page driven design, where a pages is essentially a feature based combination of View and Controller. Here a `PageModel` handles request mapping and routing*. the views are defined in a separate `.cshtml` file and which binds it to the page model file (`<...>Model.cshtml.cs`) 

    *kind of reminds me of tsx files in react.


### Dependency Injection

DI in the .NET frameworks is explicit meaning any class that you want to be injected must be registered in the app using the app builder `WebApplicationBuilder` by passing the class type `T` as generic type in  `AddScope()`

```C#
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScope<MovieService>()
```

Now the class `MovieService` can be injected into class throughout the app:

```C#
    public MoviesController(MovieService service)
    {
        _service = service;
    }
```

# APS.NET HTTP pipeline: Middleware & Filters

