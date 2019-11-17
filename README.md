# Arrange-Dependencies

The idea for this project is to simplify the arrange step when testing.

### Basic use

`nuget install ArrangeDependencies.Autofac`

```csharp
var arrange = Arrange.Dependencies(dependencies =>
{
    dependencies.UseImplementation<IUserService, UserService>();
});

var userService = arrange.Resolve<IUserService>();

// Act

// Assert
```

This will produce a `UserService` to start testing on. All the injected dependencies in `UserService` will be injected as `Mock.Of<T>()`, resulting in methods calls on any dependency will return the default value of the return type.

When adding more dependencies to the UserService you do not need to change all the tests setups that you have already created. You may, however, still need to change some tests if the behavior has changed.

This arrange block can be simplified by a one liner

```csharp
var arrange = Arrange.Dependencies<IUserService, UserService>();
```

This setup will call on `UseImplementation<IUserService, UserService>()` under the hood.

### Use Mock

`nuget install ArrangeDependencies.Autofac`

```csharp
var arrange = Arrange.Dependencies<IUserService, UserService>(dependencies =>
{
    dependencies.UseMock<IUserRepository>(mock => {
        mock.Setup(x => x.GetByName(It.IsAny<string>())).Returns(new Basis.Entites.User());
    });
});
```

Now `UserService` will use your mocked `IUserRepository`, instead of the default `Mock.Of<T>()`, and return a new `User` when `GetByName(name)` is called. The mock action is like any other Mock setup that you probably already is used to.

### Use DbContext

`nuget install ArrangeDependencies.Autofac.EntityFrameworkCore`

```csharp
var arrange = Arrange.Dependencies(dependencies =>
{
    dependencies.UseDbContext<TestDbContext>();
});
```

This will setup an in memory database for TestDbContext in your test.

### Use Entity

`nuget install ArrangeDependencies.Autofac.EntityFrameworkCore`

```csharp
var arrange = Arrange.Dependencies(dependencies =>
{
    dependencies.UseEntity<User, TestDbContext>((user) => user.SetName("Test name"));
});
```

This will add a new user into `TestDbContext` that can be used in testing. NOTE: `UseDbContext` is called under the hood here, so you don't need to define it.


```csharp
var arrange = Arrange.Dependencies(dependencies =>
{
    dependencies.UseEntity<Company, TestDbContext>((company) => company.SetName("Test name"), out var company);
    dependencies.UseEntity<User, TestDbContext>((user) => {
        user.SetName("Test name");
        user.SetCompany(company);
    });
});
```

You can also couple the entites by using the out parameter.


### Use MemoryCache

`nuget install ArrangeDependencies.Autofac.MemoryCache`

```csharp
var user = "Cache test user"

var arrange = Arrange.Dependencies<IUserService, UserService>(dependencies =>
{
    dependencies.UseMemoryCache("Cache test", user);
});
```

With this you can test your cache, where the first parameter is the key and the second is the cached object.

### Use Logger

`nuget install ArrangeDependencies.Autofac.Logger`

```csharp
Mock<ILogger<LoggingService>> logger = null;

var arrange = Arrange.Dependencies<ILoggingService, LoggingService>(dependencies =>
{
    dependencies.UseLogger(out logger);
});

var loggingService = arrange.Resolve<ILoggingService>();
loggingService.LogError();

logger.Verify(LogLevel.Error, Times.Once());
```

With this you can verfy that you log errors (or any other Log Level).


### More examples

I suggest you to check out the Test in this repo, to get more ideas how to make your testing setup easier.


### Create your own extensions

This library is extensible by nature, so if you find yourself writing the same mock setup over and over again, I suggest you to create your own extension method for that mock.

```csharp
public static class UseMyCustomExtensionExtension
{
    public static IArrangeBuilder<ContainerBuilder> UseMyCustomExtension(this IArrangeBuilder<ContainerBuilder> arrangeBuilder, /* parameters... */)
    {
        if(arrangeBuilder is ArrangeBuilder builder)		
            builder.UseMock<IMyCustomExtension>(mock => /* setup mock here */);
		
        return arrangeBuilder;
    }
}
```