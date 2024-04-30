using DAL.Repositories.Implements;
using DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Infrastructure
{
    public class UnitOfWork: IUnitOfWork
    {
        private BagMoreDbContext _dbContext;
        private IBrandRepository _brandRepository;
        private ICartProductRepository _cartProductRepository;
        private ICartRepository _cartRepository;
        private ICategoryRepository _categoryRepository;
        private IColorRepository _colorRepository;
        private IDeliveryMethodRepository _deliveryMethodRepository;
        private IDescribeProductRepository _describeProductRepository;
        private IOrderRepository _orderRepository;
        private IProductImageRepository _productImageRepository;
        private IProductRepository _productRepository;
        private IProductOrderRepository _productOrderRepository;
        private IRoleRepository _roleRepository;
        private IShippingAddressRepository _shippingAddressRepository;
        private ISizeRepository _sizeRepository;
        private ISuppliedProductRepository _suppliedProductRepository;
        private ISupplierRepository _supplierRepository;
        private IUserRepository _userRepository;
        private IUserTokenRepository _userTokenRepository;
        private IWishListRepository _wishListRepository;

        public UnitOfWork(BagMoreDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        #region WishListRepository
        public IWishListRepository WishListRepository
        {
            get
            {
                if(this._wishListRepository == null)
                {
                    this._wishListRepository = new WishListRepository(this._dbContext);
                }
                return this._wishListRepository;
            }
        }
        #endregion

        #region UserTokenRepository
        public IUserTokenRepository UserTokenRepository
        {
            get
            {
                if(this._userTokenRepository == null)
                {
                    this._userTokenRepository = new UserTokenRepository(this._dbContext);
                }
                return this._userTokenRepository;
            }
        }
        #endregion

        #region UserRepository
        public IUserRepository UserRepository
        {
            get
            {
                if (this._userRepository == null)
                {
                    this._userRepository = new UserRepository(this._dbContext);
                }
                return this._userRepository;
            }
        }
        #endregion

        #region SupplierRepository
        public ISupplierRepository SupplierRepository
        {
            get
            {
                if (this._supplierRepository == null)
                {
                    this._supplierRepository = new SupplierRepository(this._dbContext);
                }
                return this._supplierRepository;
            }
        }
        #endregion

        #region SuppliedProductRepository
        public ISuppliedProductRepository SuppliedProductRepository
        {
            get
            {
                if (this._suppliedProductRepository == null)
                {
                    this._suppliedProductRepository = new SuppliedProductRepository(this._dbContext);
                }
                return this._suppliedProductRepository;
            }
        }
        #endregion

        #region SizeRepository
        public ISizeRepository SizeRepository
        {
            get
            {
                if (this._sizeRepository == null)
                {
                    this._sizeRepository = new SizeRepository(this._dbContext);
                }
                return this._sizeRepository;
            }
        }
        #endregion

        #region ShippingAddressRepository
        public IShippingAddressRepository ShippingAddressRepository
        {
            get
            {
                if (this._shippingAddressRepository == null)
                {
                    this._shippingAddressRepository = new ShippingAddressRepository(this._dbContext);
                }
                return this._shippingAddressRepository;
            }
        }
        #endregion

        #region RoleRepository
        public IRoleRepository RoleRepository
        {
            get
            {
                if (this._roleRepository == null)
                {
                    this._roleRepository = new RoleRepository(this._dbContext);
                }
                return this._roleRepository;
            }
        }
        #endregion

        #region RoleRepository
        public IProductRepository ProductRepository
        {
            get
            {
                if (this._productRepository == null)
                {
                    this._productRepository = new ProductRepository(this._dbContext);
                }
                return this._productRepository;
            }
        }
        #endregion

        #region ProductOrderRepository
        public IProductOrderRepository ProductOrderRepository
        {
            get
            {
                if (this._productOrderRepository == null)
                {
                    this._productOrderRepository = new ProductOrderRepository(this._dbContext);
                }
                return this._productOrderRepository;
            }
        }
        #endregion

        #region ProductImageRepository
        public IProductImageRepository ProductImageRepository
        {
            get
            {
                if (this._productImageRepository == null)
                {
                    this._productImageRepository = new ProductImageRepository(this._dbContext);
                }
                return this._productImageRepository;
            }
        }
        #endregion

        #region OrderRepository
        public IOrderRepository OrderRepository
        {
            get
            {
                if (this._orderRepository == null)
                {
                    this._orderRepository = new OrderRepository(this._dbContext);
                }
                return this._orderRepository;
            }
        }
        #endregion

        #region DescribeProductRepository
        public IDescribeProductRepository DescribeProductRepository
        {
            get
            {
                if (this._describeProductRepository == null)
                {
                    this._describeProductRepository = new DescribeProductRepository(this._dbContext);
                }
                return this._describeProductRepository;
            }
        }
        #endregion

        #region DeliveryMethodRepository
        public IDeliveryMethodRepository DeliveryMethodRepository
        {
            get
            {
                if (this._deliveryMethodRepository == null)
                {
                    this._deliveryMethodRepository = new DeliveryMethodRepository(this._dbContext);
                }
                return this._deliveryMethodRepository;
            }
        }
        #endregion

        #region ColorRepository
        public IColorRepository ColorRepository
        {
            get
            {
                if (this._colorRepository == null)
                {
                    this._colorRepository = new ColorRepository(this._dbContext);
                }
                return _colorRepository;
            }
        }
        #endregion

        #region CategoryRepository
        public ICategoryRepository CategoryRepository
        {
            get
            {
                if (this._categoryRepository == null)
                {
                    this._categoryRepository = new CategoryRepository(this._dbContext);
                }
                return this._categoryRepository;
            }
        }
        #endregion

        #region CartRepository
        public ICartRepository CartRepository
        {
            get
            {
                if (this._cartRepository == null)
                {
                    this._cartRepository = new CartRepository(this._dbContext);
                }
                return _cartRepository;
            }
        }
        #endregion

        #region CartProductRepository
        public ICartProductRepository CartProductRepository
        {
            get
            {
                if (this._cartProductRepository == null)
                {
                    this._cartProductRepository = new CartProductRepository(this._dbContext);
                }
                return _cartProductRepository;
            }
        }
        #endregion

        #region BrandRepository
        public IBrandRepository BrandRepository
        {
            get
            {
                if (this._brandRepository == null)
                {
                    this._brandRepository = new BrandRepository(this._dbContext);
                }
                return _brandRepository;
            }
        }
        #endregion
        public void SaveChange()
        {
            this._dbContext.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await this._dbContext.SaveChangesAsync();
        }
    }
}
