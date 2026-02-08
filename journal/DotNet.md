## .NET Core Philosophy

[TODO add a sentence or two to introduce the subject matter of this section.]: # 

### ASP.NET web app architectures:

ASP.NET Core supports several ways to build web applications. Conceptually, they fall into server-side rendering and client-side (SPA) models.

- #### ASP.NET Core + SPA (React, Angular, etc): 
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

# ASP.NET HTTP pipeline: Middleware & Filters

In ASP.NET, middleware is code that executes at the HTTP request pipeline level. Each middleware component can inspect or modify the incoming HTTP request, perform some task, and then optionally pass control to the next component in the pipeline via a [request delegates](#request-delegates). Middleware is framework-agnostic and does not depend on MVC concepts such as controllers, pages, or models.

Filters, on the other hand, execute within the ASP.NET MVC / Razor Pages framework, surrounding the execution of a controller action or page handler. They are concerned with endpoint-level behavior, such as authorization, model validation and exception handling.

#### Request Delegates
Request flow down the pipeline and encapsulated within the [RequestContext](). Each middle ware is registered via the following delegates:

- `Use` : for middleware that sits somewhere in the pipeline and invokes the next middleware with `next.Invoke()`.
- `Map` : allows to diverge from the pipeline path.
- `Run` : are for terminal middleware of the pipeline, and no other middleware is executed after it.

After the execution of the last component of the pipeline responses flow in the other direction, meaning the code following the `next.Invoke()` call is executed in
the opposite order of the middleware declaration as shown below:
###### ASP.NET request pipeline:
```
---> Request
        MiddlewareA            
            MiddlewareB
                Controller  // filters
            MiddlewareB
        MiddlewareA
<--- Response       
```

# Razor Pages - closer look

Razor Pages trade feature-centric discoverability for locality of behavior. This means the project structure is organized based on pages rendered on the browser, thus, instead of having the traditional feature based controllers it spreads such a controller between pages that use the specific HTTP operations. This design is better suited for SSR and form heavy UI. In addition, this architecture couples more tightly the view and the controller layers making it difficult to introduce many changes to either layer.

The documentation refers to a **page** as the conceptual rendered page consisting of :
- `PageModel` : The request handler (essentially a mini, paged-scoped controller) explicitly defines the allowed HTTP verbs.
- Razor page : includes the html code and links to the PageModel via `@model`. At first I wanted to classify this component strictly as view component however it is also responsible for declaring and registering an endpoint. A page model without a razor page will lead to 404.a 


Example - url path matching:
```
└── Pages
    ├── Index.cshtml            // "/"
    ├── Home.cshtml             // "/home"
    └── Profile
        ├─ Index.cshtml         // "/profile"
        └─ Settings.cshtml      // "/profile/settings"
```