using Ecommerce.Backend.Common.Configurations;
using Ecommerce.Backend.Entities;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Entities;

namespace Ecommerce.Backend.API.Helpers
{
  public static class Extensions
  {
    // public static IWebHost MigrateDatabase<T>(this IWebHost webHost) where T : DbContext
    // {
    //   var serviceScopeFactory = (IServiceScopeFactory) webHost.Services.GetService(typeof(IServiceScopeFactory));
    //   using(var scope = serviceScopeFactory.CreateScope())
    //   {
    //     var services = scope.ServiceProvider;
    //     var dbContext = services.GetRequiredService<T>();
    //     dbContext.Database.Migrate();
    //   }
    //   return webHost;
    // }

    public static IServiceCollection InitiateDbConnection(this IServiceCollection services, IDatabaseSetting dbSetting)
    {
      switch (dbSetting.Env)
      {
        case "Dev":
          services.AddMongoDBEntities(dbSetting.DatabaseName);
          break;
        case "Prod":
          services.AddMongoDBEntities(dbSetting.DbSettings, dbSetting.DatabaseName);
          break;
      }
      DB.Migrate();
      return services;
    }

    public static IServiceCollection RegisterAllIndex(this IServiceCollection services)
    {
      // Product Index
      DB.Index<Product>()
        .Key(x => x.Title, KeyType.Ascending)
        .Option(o => o.Unique = true)
        .Create();
      DB.Index<Product>()
        .Key(x => x.SKU, KeyType.Ascending)
        .Option(o => o.Unique = true)
        .Create();
      
      // Customer Index
      DB.Index<Customer>()
        .Key(x => x.PhoneNo, KeyType.Descending)
        .Option(o => o.Unique = true)
        .Create();
      DB.Index<Customer>()
        .Key(x => x.Email, KeyType.Descending)
        .Option(o => o.Unique = true)
        .Create();
      return services;
    }
  }
}