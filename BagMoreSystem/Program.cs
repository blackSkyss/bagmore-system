using BAL.AutoMapperProfile;
using BAL.Models;
using BAL.Services.Implements;
using BAL.Services.Interfaces;
using BAL.Validator;
using DAL;
using DAL.Entities;
using DAL.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer Scheme",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddDbContext<BagMoreDbContext>(opts =>
                opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<GoogleViewModel>(builder.Configuration.GetSection("Authentication:Google"));
builder.Services.Configure<FacebookViewModel>(builder.Configuration.GetSection("Authentication:Facebook"));

builder.Services.Configure<AppSetting>(builder.Configuration.GetSection("AppSettings"));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IBrandService, BrandService>();
builder.Services.AddScoped<ICartProductService, CartProductService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IColorService, ColorService>();
builder.Services.AddScoped<IDeliveryMethodService, DeliveryMethodService>();
builder.Services.AddScoped<IDescribeProductService, DescribeProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IProductImageService, ProductImageService>();
builder.Services.AddScoped<IProductOrderService, ProductOrderService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IShippingAddressService, ShippingAddressService>();
builder.Services.AddScoped<ISizeService, SizeService>();
builder.Services.AddScoped<ISuppliedProductService, SuppliedProductService>();
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserTokenService, UserTokenService>();
builder.Services.AddScoped<IWishListService, WishListService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

builder.Services.AddAutoMapper(typeof(UserProfile));

//JWT
var serrectKey = builder.Configuration.GetSection("AppSettings:SecretKey").Value;
var serectKeyBytes = Encoding.UTF8.GetBytes(serrectKey);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        //Tự cấp token nên phần này bỏ qua
        ValidateIssuer = false,
        ValidateAudience = false,
        //Ký vào token
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(serectKeyBytes),
        ClockSkew = TimeSpan.Zero
    };
});
builder.Services.AddScoped<IDashBoardService, DashBoardService>();
//add auto mapper
builder.Services.AddAutoMapper(typeof(UserProfile));
builder.Services.AddAutoMapper(typeof (ShippingAddressProfile));
builder.Services.AddAutoMapper(typeof(ProductProfile));
builder.Services.AddAutoMapper(typeof(ProductImageProfile));
builder.Services.AddAutoMapper(typeof(DescribeProductProfile));
builder.Services.AddAutoMapper(typeof(ColorProfile));
builder.Services.AddAutoMapper(typeof(SizeProfile));
builder.Services.AddAutoMapper(typeof(SuppliedProductProfile));
builder.Services.AddAutoMapper(typeof(BrandProfile));
builder.Services.AddAutoMapper(typeof(CategoryProfile));
builder.Services.AddAutoMapper(typeof(SupplierProfile));
builder.Services.AddAutoMapper(typeof(WishListViewModelProfile));
builder.Services.AddAutoMapper(typeof(OrderUserDetailItemProfile));
builder.Services.AddAutoMapper(typeof(ProductProfile),
                               typeof(CategoryProfile),
                               typeof(ColorProfile),
                               typeof(SizeProfile),
                               typeof(DeliveryMethodProfile),
                               typeof(SupplierProfile),
                               typeof(OrderProfile),
                               typeof(ProductOrderProfile));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
