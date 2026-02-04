This file includes everything I learned about C# / .NET while working on this project.

### Fields and Properties
In many programming languages, the terms field and property are often used interchangeably: basically, a class-level variable that stores data.
In C#, however, there's a distinction. A field follows the classic definition in OOP like the `id` in the following code.

```C#
public class Movie {
    public int id;
}
```

Now if we want to controller access we'd want to define setters and getter with some logic and modify the visibility of the field:

```C#
public class Movie {
    private int id;

    public int GetId() => id;
    public void SetId(int newId) {
        if (newId < 0) {
            throw new ArgumentOutOfRangeException();
        }
        id = newId;
    }
}
```

C# provides a more idiomatic and convenient way to do this using properties.

A property is a public interface for a class field that implicitly defines getter and setter logic. It allows controlled access while letting you read and write the value with normal syntax (`movie.Id`) rather than method calls. Properties also integrate better with tools like linters, serializers, and ORMs (like EF Core).

```C#
public class Movie {
    private int _id;

    public int Id {
        get => _id;
        set {
            if (value < 0) {
                throw new ArgumentOutOfRangeException();
            }
            _id = value;
        }
    }
}
```

### Auto-properties 
C# offers auto-properties for the common case where you don’t need custom logic in the getter or setter. The compiler automatically generates a hidden backing field for you:

```C#
public class Movie
{
    public int Id { get; set; }  // compiler generates hidden backing field
}
```

A question one might have is why do i need a property if i don't need access control?

1. Future-proofing. By declaring an auto prop you can easily extend
the property to handle access control with minimal refactoring. 

2. Tooling compatibility : Many frameworks and tools in the .NET ecosystem (EF Core, Razor Pages model binding, and JSON serializers) expect properties, not fields. Using properties ensures smooth integration with these tools.


#### Naming conventions

| variable type | naming convention | 
|--------------------|-------|
| private field | _camelCase |
| public property | PascalCase |
| local variable | camelCase |

*methods also use PascalCase in C#.

### Nullability and initialization

C# apparently does not enforce member initialization which requires you to constantly guard against `null`s 
but hey at least we can bypass the builder pattern by making every class a builder class. great. 

This means the following code is completely valid:

```C#
public class Movie {
    public int Id {get;set;}
    public string Title {get;set;}
}

var mov1 = new Movie {  // valid, all members are assigned
        Id = 0,
        Title = "Titanic",
    }
var mov2 = new Movie(); // also valid, members are null
```

However, one way to enforce invariants are via constructors. By defining a constructor with the parameters the class cannot be instantiated without passing values.
But this doesn't guarantee that the values passed are not null.


```C#
public class Movie {
    public int Id {get;set;}
    public string Title {get;set;}

    public Movie(int id, string title) {
        this.Id = id;
        this.Title = title;
    }
}

    var mov1 = new Movie(0,"Titanic");          // valid
    var mov2 = new Movie(null, null);           // valid
    var mov3 = new Movie();                     // compile error
    var mov4 = new Movie { Title = "Titanic" }  // compile error, parameterless object initializer are not allowed.
```

Another way to guard a bit against nullability is using `required`, which again does not prevent nulls from being passed.

```c#
public class Movie {
    public int Id {get;set;}
    public required string Title {get;set;}

    public Movie(int id) {
        this.Id = id;
    }
}

    var mov1 = new Movie(0,"Titanic");      // valid
    var mov2 = new Movie(null, null);       // valid
    var mov3 = new Movie(0);                // compile error, Title is required and must be set.
    var mov4 = new Movie(0) {"Titanic"}     // valid
```

Notice that the constructor and initializer are both called it is important to know that constructors run first and initializers afterwards.
