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

Moving away from SPA application the server-client control flow changes. Instead of the browser making async requests to fetch data than reads the response with the necessary data running some JS and rendering the page. In SSR application, like Razor Pages, the DOM is rendered on the server meaning that once a request is made the server loads the data, attaches it to the html page, renders it and sends it as a response to the browser whose only responsibility is displaying the page. To this extent 1 request* = 1 URL = 1 render.

###### *excluding the request made to fetch the subsequent page resources such as the css style sheet.

With this philosophy, Razor Pages trade feature-centric discoverability for locality of behavior. This means the project structure is organized based on pages rendered on the browser, thus, instead of having the traditional feature based controllers, it spreads the controller between pages that use the specific HTTP operations. While this approach is better for SSR one obvious limitation that arises the the tighter coupling between view and controller logic as a result from this page centric structure means that changes affecting an entire feature require changes across many files (e.g., adding logging to every user profile edit.)

The documentation refers to a **page** as the conceptual rendered page consisting of :
- `PageModel` : The request handler (essentially a mini, paged-scoped controller) explicitly defines the allowed HTTP verbs.
- Razor page : includes the html code and links to the PageModel via `@model`. At first I wanted to classify this component strictly as view component however it is also responsible for declaring and registering an endpoint. A page model without a razor page will lead to 404. In other words the razor page declares the intent. 


Example - url path matching:
```
└── Pages
    ├── Index.cshtml            // "/"
    ├── Home.cshtml             // "/home"
    └── Profile
        ├─ Index.cshtml         // "/profile"
        └─ Settings.cshtml      // "/profile/settings"
```


### Tag helpers

# Persistence: EF Core & DbContext

In the .NET ecosystem, EF Core provides support for ORM as well as managing database access, change tracking, and transactions through the `DbContext`.  

DbContext provides an interface to the underlying Data base similar to JPA in Spring. However, unlike JPA a dbContext instance also represents a **Unit-of-Work**, meaning that each change to the db is stored in the dbContext and is persisted atomically with a call to `SaveChanges()`. Another difference is that SQL queries are generated via calling `LINQ` query operators exposed by the `DbSet` in the form of methods such as, `Where()`, `Find()`, `Add()`, which performed on the `DbSet`. Essentially, the `DbSet` is a proxy to the underlying db table that tracks all the actions performed and the entities queried (not the all the table rows).


```C#
public class ProfileContext : DbContext {
    public DbSet<Profile> Profiles { get; set; } 
}

var db = new ProfileContext()
var profile = db.Profiles.Find(0) // get profile with id == 0.
```

## DbContext injection

To make a specific context available for DI, it is necessary to register it with `builder.Services.AddDbContext<ProfileContext>()`.

## Entities 

To make a class into an entity it has to be either annotated explicitly or implicitly by initializing a `DbSet` for it with as in the preceding code.

### Relationships

Relationships are defined implicitly. Unlike spring that requires annotation like `@OneToOne` or `@oneToMany`, EF infers the relationship based on the properties:

```C#
public class Season {
    public int Id{get;set;}
    public ICollection<Episode> Episodes{get;set;}
}
```
In the preceding code EF infers `Id` automatically as the primary key of Season as well as the one-to-many relationship between Season and Episode table. 
`Episodes` here is called a navigation property as it is used to navigate between the tables.

Per convention it is better to make such relationships explicit with **Fluent-API**:

```C#
public class SeasonContext: DbContext {
    public DbSet<Season> seasons{get;set;}

    protected override void onModelCreating(DbModelBuilder modelBuilder) {
        modalBuilder.Entity<Season>()
            .HasMany(s => s.episodes)   // defines relationship and navigation
            .WithOne()                  // wrong. omitted if one directional navigation is not required.
    }
}
```
Alternatively, data labels can also be used to makes things more explicit but i prefer Fluent API as it is less to remember and works great with linter. 

Using Fluent API we can define many configuration options such as cascading on delete:

```C#
public class SeasonContext: DbContext {
    public DbSet<Season> seasons{get;set;}

    protected override void onModelCreating(DbModelBuilder modelBuilder) {
        modalBuilder.Entity<Season>()
            .HasMany(s => s.episodes)  
            .WillCascadeOnDelete(true) // delete all episode on season delete.             
    }
}
```

It is important to note that unlike Hibernate EF, by default, does eager load to the child data. to enable lazy fetch one must add a table proxy support as a dependency and set the navigation property as virtual.

### Polymorphism

By default EF stores all derived types in a single table (table-per-hierarchy) and uses an additional (discriminator) column to distinguish between them and missing columns are filled with nulls. However, tables per type (TPT) can be enabled using Fluent api for each type or once per base class:

```C#
public abstract class Media {/*..*/}
public class Movie : Media {/*..*/}
public class Season : Media {/*..*/}

// per class declaration:
modelBuilder.Entity<Season>().ToTable("Seasons");
modelBuilder.Entity<Movie>().ToTable("Movies");

// once per base:
modelBuilder.Entity<Media>().UseTptMappingStrategy();

// table per concrete class:
modelBuilder.Entity<Media>().UseTpcMappingStrategy();

```
TPC strategy is another approach that doesn't create a table for the base class and performs joins to obtain the data from the base class props. Instead it creates a table for each concrete class and includes columns for all properties including of the base class.

##### Interfaces 
EF core does not support mapping of interface types meaning it is not possible to have a navigation property of an interface type, for example:

```C#
public class Car {
    public int Id {get;set;}
    public IEngine Engine {get;set;} // ef core error on type.
}
```

### Validation

### Migrations

Migrations are essentially version control for db schema changes. To commit a change run the following command:

```
dotnet ef migrations add <CommitName>
```

After running the preceding command the Database file still doesn't exist this only creates the instructions for generating db tables. Next we want to apply those instruction to generate the tables:

```
dotnet ef database update
```



