# AspNet.Identity.InMemory
This is an ASP.NET identity provider using only memory as storage.

It is a replacement for the default Entity Framework identity provider when you just need something simpler for a demo or quick prototype.

## Usage

In Startup.cs, bring in the `AspNet.Identity.InMemory` namespace and in the `ConfigureServices()` function, just change `.AddEntityFrameWorkStores()` to `.AddInMemoryStores()`, like this:

```c#
    using AspNet.Identity.InMemory;
    ...

    public void ConfigureServices(IServiceCollection services)
    {
        ...

        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddInMemoryStores()
            .AddDefaultTokenProviders();

        ...
    }
```

Then, just make sure the user class class implements `AspNet.Identity.InMemory.IIdentityUser` and the role class implements `AspNet.identity.InMemory.IIdentityRole`.

You can use `AspNet.Identity.InMemory.IdentityUser` and `AspNet.Identity.InMemory.IdentityRole` if you want or create your own, as long as they implement the `IIdentityUser` and `IIdentityRole` interfaces or inherit from `IdentityUser` and `IdentityRole`.

## Advanced Usage (Creating default users and roles)

If you want to pre-populate the user and role stores with users and roles, you can do that using `InMemoryIdentityFactory`.

First, register the service in `ConfigureServices()`:

```c#
    public void ConfigureServices(IServiceCollection services)
    {
        ...

        services.AddTransient<InMemoryIdentityFactory<ApplicationUser, IdentityRole>>();
    }
```

And then inject it into `Configure()` and use it in there to create users and roles:

```c#
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app, 
            IHostingEnvironment env, 
            ILoggerFactory loggerFactory, 
            InMemoryIdentityFactory<ApplicationUser, IdentityRole> identityFactory)
        {
            ...

            // Add some default roles and users
            Task.WaitAll(
                identityFactory.AddRoleAsync("admin", "admin"),
                identityFactory.AddUserAsync("1", "admin@example.com", "Admin.1234", new [] { "admin" }),
                identityFactory.AddUserAsync("2", "user@example.com", "User.1234")
            );
        }
```

You can also set claims for roles and users this way.

