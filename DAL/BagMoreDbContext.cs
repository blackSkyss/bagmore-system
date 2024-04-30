using DAL.DataSeeding;
using DAL.Entities;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class BagMoreDbContext: DbContext
    {
        public BagMoreDbContext(DbContextOptions<BagMoreDbContext> opts):base(opts)
        {
        }

        public DbSet<Brand> Brands { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<DeliveryMethod> DeliveryMethods { get; set; }
        public DbSet<DescribeProduct> DescribeProducts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductOrder> ProductOrders { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<ShippingAddress> ShippingAddresses { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<SuppliedProduct> SuppliedProducts { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<CartProduct> UserCarts { get; set; }
        public DbSet<UserToken> UserTokens { get; set; }
        public DbSet<WishList> WishLists { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Brands
            modelBuilder.Entity<Brand>().HasKey("Id");
            modelBuilder.Entity<Brand>().Property("Name").HasColumnType("nvarchar(100)").IsRequired();
            modelBuilder.Entity<Brand>().Property("Logo").HasColumnType("varbinary(MAX)").IsRequired();
            modelBuilder.Entity<Brand>().Property("Status").HasColumnType("int").IsRequired();
            #endregion

            #region Users
            modelBuilder.Entity<User>().HasKey("Id");
            modelBuilder.Entity<User>().Property("Email").HasColumnType("varchar(100)").IsRequired();
            modelBuilder.Entity<User>().Property("Password").HasColumnType("varchar(MAX)").IsRequired(false);
            modelBuilder.Entity<User>().Property("Gender").HasColumnType("bit").IsRequired(false);
            modelBuilder.Entity<User>().Property("FirstName").HasColumnType("nvarchar(15)").IsRequired();
            modelBuilder.Entity<User>().Property("LastName").HasColumnType("nvarchar(35)").IsRequired();
            modelBuilder.Entity<User>().Property("BirthDay").HasColumnType("DateTime").IsRequired(false);
            modelBuilder.Entity<User>().Property("Phone").HasColumnType("varchar(10)").IsRequired(false);
            modelBuilder.Entity<User>().Property("FirstAddress").HasColumnType("nvarchar(100)").IsRequired(false);
            modelBuilder.Entity<User>().Property("SecondAddress").HasColumnType("nvarchar(100)").IsRequired(false);
            modelBuilder.Entity<User>().Property("Avatar").HasColumnType("varbinary(MAX)").IsRequired(false);
            modelBuilder.Entity<User>().Property("Status").HasColumnType("int").IsRequired();
            modelBuilder.Entity<User>().Property("CreatedDate").HasColumnType("DateTime").IsRequired();
            modelBuilder.Entity<User>()
                .HasOne<Role>(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId);
            #endregion

            #region Roles
            modelBuilder.Entity<Role>().HasKey("Id");
            modelBuilder.Entity<Brand>().Property("Name").HasColumnType("nvarchar(20)").IsRequired();
            #endregion

            #region ShippingAddress
            modelBuilder.Entity<ShippingAddress>().HasKey("Id");
            modelBuilder.Entity<ShippingAddress>().Property("FirstName").HasColumnType("nvarchar(15)").IsRequired();
            modelBuilder.Entity<ShippingAddress>().Property("LastName").HasColumnType("nvarchar(35)").IsRequired();
            modelBuilder.Entity<ShippingAddress>().Property("Phone").HasColumnType("varchar(10)").IsRequired();
            modelBuilder.Entity<ShippingAddress>().Property("Address").HasColumnType("nvarchar(100)").IsRequired();
            modelBuilder.Entity<ShippingAddress>().Property("Status").HasColumnType("int").IsRequired();
            modelBuilder.Entity<ShippingAddress>()
                .HasOne<User>(s => s.User)
                .WithMany(u => u.ShippingAddresses)
                .HasForeignKey(s => s.IdUser);
            #endregion

            #region Orders
            modelBuilder.Entity<Order>().HasKey("Id");
            modelBuilder.Entity<Order>().Property("DeliveryStatus").HasColumnType("int").IsRequired();
            modelBuilder.Entity<Order>().Property("ShippingAddress").HasColumnType("nvarchar(100)").IsRequired();
            modelBuilder.Entity<Order>().Property("Status").HasColumnType("int").IsRequired();
            modelBuilder.Entity<Order>().Property("OrderedDate").HasColumnType("DateTime").IsRequired();
            modelBuilder.Entity<Order>().Property("DeliveryMethodPrice").HasColumnType("decimal").IsRequired();
            modelBuilder.Entity<Order>()
                .HasOne<User>(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserID);
            modelBuilder.Entity<Order>()
                .HasOne<DeliveryMethod>(o => o.DeliveryMethod)
                .WithMany(d => d.Orders)
                .HasForeignKey(o => o.DeliveryMethodId);

            #endregion

            #region DeliveryMethods
            modelBuilder.Entity<DeliveryMethod>().HasKey("Id");
            modelBuilder.Entity<DeliveryMethod>().Property("Name").HasColumnType("nvarchar(50)").IsRequired();
            modelBuilder.Entity<DeliveryMethod>().Property("Price").HasColumnType("decimal").IsRequired();
            modelBuilder.Entity<DeliveryMethod>().Property("Status").HasColumnType("int").IsRequired();
            modelBuilder.Entity<DeliveryMethod>().Property("Description").HasColumnType("nvarchar(MAX)").IsRequired();
            #endregion

            #region ProductOrders
            modelBuilder.Entity<ProductOrder>().HasKey("Id");
            modelBuilder.Entity<ProductOrder>().Property("Size").HasColumnType("varchar(5)").IsRequired();
            modelBuilder.Entity<ProductOrder>().Property("Color").HasColumnType("varchar(15)").IsRequired();
            modelBuilder.Entity<ProductOrder>().Property("Price").HasColumnType("decimal").IsRequired();
            modelBuilder.Entity<ProductOrder>().Property("Amount").HasColumnType("int").IsRequired();
            modelBuilder.Entity<ProductOrder>()
                .HasOne<Order>(p => p.Order)
                .WithMany(o => o.ProductOrders)
                .HasForeignKey(p => p.IdOrder);
            modelBuilder.Entity<ProductOrder>()
                .HasOne<Product>(p => p.Product)
                .WithMany(pr => pr.ProductOrders)
                .HasForeignKey(p => p.IdProduct);
            #endregion

            #region Products
            modelBuilder.Entity<Product>().HasKey("Id");
            modelBuilder.Entity<Product>().Property("Name").HasColumnType("nvarchar(100)").IsRequired();
            modelBuilder.Entity<Product>().Property("Composition").HasColumnType("nvarchar(MAX)").IsRequired();
            modelBuilder.Entity<Product>().Property("Description").HasColumnType("nvarchar(MAX)").IsRequired();
            modelBuilder.Entity<Product>().Property("Discount").HasColumnType("float").IsRequired();
            modelBuilder.Entity<Product>().Property("TotalProduct").HasColumnType("int").IsRequired();
            modelBuilder.Entity<Product>().Property("Status").HasColumnType("int").IsRequired();
            modelBuilder.Entity<Product>()
                .HasOne<Brand>(p => p.Brand)
                .WithMany(b => b.Products)
                .HasForeignKey(p => p.BrandId);
            modelBuilder.Entity<Product>()
                .HasOne<Category>(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);
            #endregion

            #region Suppliers
            modelBuilder.Entity<Supplier>().HasKey("Id");
            modelBuilder.Entity<Supplier>().Property("Name").HasColumnType("nvarchar(100)").IsRequired();
            modelBuilder.Entity<Supplier>().Property("Email").HasColumnType("varchar(100)").IsRequired();
            modelBuilder.Entity<Supplier>().Property("Status").HasColumnType("int").IsRequired();
            modelBuilder.Entity<Supplier>().Property("Phone").HasColumnType("varchar(10)").IsRequired();
            #endregion
            
            #region SuppliedProducts
            modelBuilder.Entity<SuppliedProduct>().HasKey("Id");
            modelBuilder.Entity<SuppliedProduct>().Property("Amount").HasColumnType("int").IsRequired();
            modelBuilder.Entity<SuppliedProduct>()
                .HasOne<Supplier>(s => s.Supplier)
                .WithMany(sp => sp.SuppliedProducts)
                .HasForeignKey(s => s.SupplierId);
            modelBuilder.Entity<SuppliedProduct>()
                .HasOne<Product>(s => s.Product)
                .WithMany(p => p.SuppliedProducts)
                .HasForeignKey(s => s.ProductId);
            #endregion

            #region Categories
            modelBuilder.Entity<Category>().HasKey("Id");
            modelBuilder.Entity<Category>().Property("Name").HasColumnType("nvarchar(50)").IsRequired();
            modelBuilder.Entity<Category>().Property("Status").HasColumnType("int").IsRequired();
            #endregion

            #region ProductImages
            modelBuilder.Entity<ProductImage>().HasKey("Id");
            modelBuilder.Entity<ProductImage>().Property("Source").HasColumnType("varbinary(MAX)").IsRequired();
            modelBuilder.Entity<ProductImage>()
                .HasOne<Product>(pi => pi.Product)
                .WithMany(p => p.ProductImages)
                .HasForeignKey(pi => pi.ProductId);
            #endregion

            #region Colors
            modelBuilder.Entity<Color>().HasKey("Id");
            modelBuilder.Entity<Color>().Property("Name").HasColumnType("nvarchar(100)").IsRequired();
            modelBuilder.Entity<Color>().Property("ColorCode").HasColumnType("varchar(15)").IsRequired();
            modelBuilder.Entity<Color>().Property("Status").HasColumnType("int").IsRequired();
            #endregion
            
            #region Sizes
            modelBuilder.Entity<Size>().HasKey("Id");
            modelBuilder.Entity<Size>().Property("Name").HasColumnType("nvarchar(5)").IsRequired();
            modelBuilder.Entity<Size>().Property("Status").HasColumnType("int").IsRequired();
            #endregion

            #region DescribeProducts
            modelBuilder.Entity<DescribeProduct>().HasKey("Id");
            modelBuilder.Entity<DescribeProduct>().Property("Price").HasColumnType("decimal").IsRequired();
            modelBuilder.Entity<DescribeProduct>().Property("Amount").HasColumnType("int").IsRequired();
            modelBuilder.Entity<DescribeProduct>().Property("OriginalPrice").HasColumnType("decimal").IsRequired();
            modelBuilder.Entity<DescribeProduct>().Property("providerId").HasColumnType("int").IsRequired();
            modelBuilder.Entity<DescribeProduct>()
                .HasOne<Color>(dp => dp.Color)
                .WithMany(c => c.DescribeProducts)
                .HasForeignKey(dp => dp.ColorId);
            modelBuilder.Entity<DescribeProduct>()
                .HasOne<Size>(dp => dp.Size)
                .WithMany(s => s.DescribeProducts)
                .HasForeignKey(dp => dp.SizeId);
            modelBuilder.Entity<DescribeProduct>()
                .HasOne<Product>(dp => dp.Product)
                .WithMany(p => p.DescribeProducts)
                .HasForeignKey(dp => dp.ProductId);
            #endregion

            #region Carts
            modelBuilder.Entity<Cart>().HasKey("Id");
            modelBuilder.Entity<Cart>()
                .HasOne<User>(c => c.User)
                .WithOne(u => u.Cart)
                .HasForeignKey<Cart>(c => c.UserId);
            #endregion

            #region CartProducts
            modelBuilder.Entity<CartProduct>().HasKey("Id");
            modelBuilder.Entity<CartProduct>().Property("Color").HasColumnType("varchar(15)").IsRequired();
            modelBuilder.Entity<CartProduct>().Property("Amount").HasColumnType("int").IsRequired();
            modelBuilder.Entity<CartProduct>().Property("Size").HasColumnType("nvarchar(100)").IsRequired();
            modelBuilder.Entity<CartProduct>().Property("Status").HasColumnType("int").IsRequired();
            modelBuilder.Entity<CartProduct>()
                .HasOne<Cart>(cp => cp.Cart)
                .WithMany(c => c.CartProducts)
                .HasForeignKey(cp => cp.CartId);
            modelBuilder.Entity<CartProduct>()
                .HasOne<Product>(cp => cp.Product)
                .WithMany(p => p.CartProducts)
                .HasForeignKey(cp => cp.ProductId);
            #endregion

            #region WishLists
            modelBuilder.Entity<WishList>().HasKey("Id");
            modelBuilder.Entity<WishList>()
                .HasOne<User>(w => w.User)
                .WithMany(u => u.WishLists)
                .HasForeignKey(w => w.UserId);
            modelBuilder.Entity<WishList>()
                .HasOne<Product>(w => w.Product)
                .WithMany(p => p.WishLists)
                .HasForeignKey(w => w.ProductId);
            #endregion

            #region UserTokens
            modelBuilder.Entity<UserToken>().HasKey("Id");
            modelBuilder.Entity<UserToken>().Property("JwtId").HasColumnType("varchar(MAX)").IsRequired();
            modelBuilder.Entity<UserToken>().Property("Token").HasColumnType("varchar(MAX)").IsRequired();
            modelBuilder.Entity<UserToken>().Property("IsUsed").HasColumnType("bit").IsRequired();
            modelBuilder.Entity<UserToken>().Property("IsRevoked").HasColumnType("bit").IsRequired();
            modelBuilder.Entity<UserToken>().Property("IssuedAt").HasColumnType("DateTime").IsRequired();
            modelBuilder.Entity<UserToken>().Property("ExpiredAt").HasColumnType("DateTime").IsRequired();
            modelBuilder.Entity<UserToken>()
                .HasOne<User>(ut => ut.User)
                .WithMany(u => u.UserTokens)
                .HasForeignKey(ut => ut.UserId);
            #endregion

            modelBuilder.SeedingRole();
            modelBuilder.SeedingColor();
            modelBuilder.SeedingSize();
        }
    }
}
