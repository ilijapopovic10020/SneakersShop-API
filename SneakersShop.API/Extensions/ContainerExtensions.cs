using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SneakersShop.API.Core;
using SneakersShop.API.Jwt;
using SneakersShop.Application.UseCases.Commands.Addresses;
using SneakersShop.Application.UseCases.Commands.Brands;
using SneakersShop.Application.UseCases.Commands.Carts;
using SneakersShop.Application.UseCases.Commands.Favorites;
using SneakersShop.Application.UseCases.Commands.Orders;
using SneakersShop.Application.UseCases.Commands.Password;
using SneakersShop.Application.UseCases.Commands.Products;
using SneakersShop.Application.UseCases.Commands.Reviews;
using SneakersShop.Application.UseCases.Commands.Users;
using SneakersShop.Application.UseCases.Queries.Addresses;
using SneakersShop.Application.UseCases.Queries.Brands;
using SneakersShop.Application.UseCases.Queries.Carts;
using SneakersShop.Application.UseCases.Queries.Categories;
using SneakersShop.Application.UseCases.Queries.Cities;
using SneakersShop.Application.UseCases.Queries.Colors;
using SneakersShop.Application.UseCases.Queries.Favorites;
using SneakersShop.Application.UseCases.Queries.Orders;
using SneakersShop.Application.UseCases.Queries.Products;
using SneakersShop.Application.UseCases.Queries.Reviews;
using SneakersShop.Application.UseCases.Queries.Sizes;
using SneakersShop.Application.UseCases.Queries.Users;
using SneakersShop.DataAccess;
using SneakersShop.Domain;
using SneakersShop.Implementation.UseCases.Commands.Addresses;
using SneakersShop.Implementation.UseCases.Commands.Brands;
using SneakersShop.Implementation.UseCases.Commands.Carts;
using SneakersShop.Implementation.UseCases.Commands.Favorites;
using SneakersShop.Implementation.UseCases.Commands.Orders;
using SneakersShop.Implementation.UseCases.Commands.Password;
using SneakersShop.Implementation.UseCases.Commands.Products;
using SneakersShop.Implementation.UseCases.Commands.Reviews;
using SneakersShop.Implementation.UseCases.Commands.Users;
using SneakersShop.Implementation.UseCases.Queries.Addresses;
using SneakersShop.Implementation.UseCases.Queries.Brands;
using SneakersShop.Implementation.UseCases.Queries.Carts;
using SneakersShop.Implementation.UseCases.Queries.Categories;
using SneakersShop.Implementation.UseCases.Queries.Cities;
using SneakersShop.Implementation.UseCases.Queries.Colors;
using SneakersShop.Implementation.UseCases.Queries.Favorites;
using SneakersShop.Implementation.UseCases.Queries.Orders;
using SneakersShop.Implementation.UseCases.Queries.Products;
using SneakersShop.Implementation.UseCases.Queries.Reviews;
using SneakersShop.Implementation.UseCases.Queries.Sizes;
using SneakersShop.Implementation.UseCases.Queries.Users;
using SneakersShop.Implementation.Validators.Brands;
using SneakersShop.Implementation.Validators.Products;
using SneakersShop.Implementation.Validators.Reviews;
using SneakersShop.Implementation.Validators.Users;

namespace SneakersShop.API.Extensions;

public static class ContainerExtensions
{
    public static void AddJwt(this IServiceCollection services, AppSettings settings)
    {
        services.AddTransient(x =>
        {
            var context = x.GetService<SneakersShopDbContext>();
            var settings = x.GetService<AppSettings>();

            return new JwtManager(context, settings.JwtSettings);
        });

        services.AddAuthentication(options =>
    {
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(cfg =>
    {
        cfg.RequireHttpsMetadata = false;
        cfg.SaveToken = true;
        cfg.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = settings.JwtSettings.Issuer,
            ValidateIssuer = true,
            ValidAudience = "Any",
            ValidateAudience = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.JwtSettings.SecretKey)),
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });
    }

    public static void AddApplicationUser(this IServiceCollection services)
    {
        services.AddTransient<IApplicationUser>(x =>
        {
            var accessor = x.GetService<IHttpContextAccessor>();
            var header = accessor.HttpContext.Request.Headers["Authorization"];

            // Payload
            var claims = accessor.HttpContext.User;

            if (claims == null || claims.FindFirst("Id") == null)
            {
                return new AnonymousActor();
            }

            var actor = new JwtActor
            {
                Email = claims.FindFirst("Email").Value,
                Id = Int32.Parse(claims.FindFirst("Id").Value),
                Identity = claims.FindFirst("Username").Value,
                Role = claims.FindFirst("Role").Value,
                UseCaseIds = JsonConvert.DeserializeObject<List<int>>(claims.FindFirst("UseCases").Value)
            };

            return actor;
        });
    }

    public static void AddUseCases(this IServiceCollection services)
    {
        // Queries

        // Brands
        services.AddTransient<IGetBrandsQuery, EfGetBrandsQuery>();
        services.AddTransient<IFindBrandQuery, EfFindBrandQuery>();

        // Users
        // services.AddTransient<IGetUsersQuery, EfGetUsersQuery>();
        services.AddTransient<IFindUserQuery, EfFindUserQuery>();

        // Products
        services.AddTransient<IGetProductsQuery, EfGetProductsQuery>();
        services.AddTransient<IFindProductQuery, EfFindProductQuery>();

        // Categories
        services.AddTransient<IGetCategoriesQuery, EfGetCategoriesQuery>();
        services.AddTransient<IFindCategoryQuery, EfFindCategoryQuery>();

        // Sizes
        services.AddTransient<IGetSizesQuery, EfGetSizesQuery>();
        services.AddTransient<IFindSizeQuery, EfFindSizeQuery>();

        // Colors
        services.AddTransient<IGetColorsQuery, EfGetColorsQuery>();
        services.AddTransient<IFindColorQuery, EfFindColorQuery>();

        // Reivews
        services.AddTransient<IGetReviewsQuery, EfGetReviewsQuery>();

        // Orders
        services.AddTransient<IGetOrdersQuery, EfGetOrdersQuery>();
        services.AddTransient<IFindOrderQuery, EfFindOrderQuery>();

        // Cities
        services.AddTransient<IGetCitiesQuery, EfGetCitiesQuery>();

        // Favorites
        services.AddTransient<IGetFavoritesQuery, EfGetFavoritesQuery>();

        // Addresses
        services.AddTransient<IGetAddressesQuery, EfGetAddressesQuery>();

        // Carts
        services.AddTransient<IFindCartQuery, EfFindCartQuery>();

        // Commands

        // Brands
        services.AddTransient<ICreateBrandCommand, EfCreateBrandCommand>();

        // Users
        services.AddTransient<ICreateUserCommand, EfCreateUserCommand>();
        services.AddTransient<IUpdateUserCommand, EfUpdateUserCommand>();
        services.AddTransient<IDeleteUserCommand, EfDeleteUserCommand>();

        // Products
        services.AddTransient<ICreateProductCommand, EfCreateProductCommand>();

        // Reivews
        services.AddTransient<ICreateReviewCommand, EfCreateReviewCommand>();
        services.AddTransient<IUpdateReviewCommand, EfUpdateReviewCommand>();
        services.AddTransient<IDeleteReviewCommand, EfDeleteReviewCommand>();

        // Orders
        services.AddTransient<ICreateOrderCommand, EfCreateOrderCommand>();
        services.AddTransient<IUpdateOrderCommand, EfUpdateOrderCommand>();
        services.AddTransient<IDeleteOrderCommand, EfDeleteOrderCommand>();

        // Addresses
        services.AddTransient<ICreateAddressCommand, EfCreateAddressCommand>();
        services.AddTransient<IUpdateAddressCommand, EfUpdateAddressCommand>();
        services.AddTransient<IDeleteAddressCommand, EfDeleteAddressCommand>();

        // Favorites
        services.AddTransient<ICreateFavoriteCommand, EfCreateFavoriteCommand>();
        services.AddTransient<IDeleteFavoriteCommand, EfDeleteFavoriteCommand>();

        // Carts
        services.AddTransient<IUpsertCartCommand, EfUpsertCartCommand>();

        // Password
        services.AddTransient<IUpdatePasswordCommand, EfUpdatePasswordCommand>();
    }

    public static void AddValidators(this IServiceCollection services)
    {
        // Validators

        // Brands
        services.AddTransient<CreateBrandValidator>();

        // Users
        services.AddTransient<CreateUserValidator>();
        services.AddTransient<UpdateUserValidator>();

        // Products
        services.AddTransient<CreateProductValidator>();

        // Reviews
        services.AddTransient<CreateReviewValidator>();
        services.AddTransient<UpdateReviewValidator>();
    }
}
