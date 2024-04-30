
using BAL.Models;
using BAL.Services.Interfaces;
using DAL.Entities;
using DAL.Enums;
using DAL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services.Implements
{
    public class CartProductService : ICartProductService
    {
        private UnitOfWork _unitOfWork;
        public CartProductService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = (UnitOfWork)unitOfWork;
        }

        public async Task AddToCart(CartProductViewModel cartProduct, string username)
        {
            try
            {
                if (cartProduct.Amount == null)
                {
                    throw new Exception("Product amount is required!");
                }
                if (cartProduct.Amount.Value <= 0)
                {
                    throw new Exception("Product amount must be more than 0!");
                }
                Product product = await this._unitOfWork.ProductRepository.GetProductDetail(cartProduct.ProductId);
                if (product == null)
                {
                    throw new Exception("Product does not exist in the system!");
                }
                if (product.TotalProduct <= 0)
                {
                    throw new Exception("Product is out of stock!");
                }
                else
                {
                    if(cartProduct.Amount > product.TotalProduct)
                    {
                        throw new Exception("Product amount is not suitable!");
                    }
                }
                //check user has cart
                User user = await this._unitOfWork.UserRepository.GetUserAsync(username);
                Cart cart = await this._unitOfWork.CartRepository.GetUserCart(user.Id);
                if (cart == null)
                {
                    //Create cartProduct
                    CartProduct newCartProduct = new CartProduct()
                    {
                        Amount = cartProduct.Amount.Value,
                        Color = cartProduct.Color,
                        Size = cartProduct.Size,
                        ProductId = cartProduct.ProductId,
                        Status = (int)CartProductEnum.Status.ACTIVE,//Active
                    };
                    // create new cart
                    Cart newCart = new Cart()
                    {
                        UserId = user.Id,
                        CartProducts = new List<CartProduct>() { newCartProduct },
                    };
                    await this._unitOfWork.CartRepository.CreateUserCart(newCart);
                    await this._unitOfWork.SaveChangesAsync();

                }
                else
                {
                    //product co trong cart chua
                    CartProduct cartProductInCart = await this._unitOfWork.CartProductRepository
                                                            .GetProductFromCart(cart.Id, cartProduct.ProductId, cartProduct.Color, cartProduct.Size);
                    if (cartProductInCart != null)
                    {
                        if (cartProductInCart.Status == (int)CartProductEnum.Status.INACTIVE)
                        {
                            cartProductInCart.Status = (int)CartProductEnum.Status.ACTIVE;
                        }
                        cartProductInCart.Amount += cartProduct.Amount.Value;
                        this._unitOfWork.CartProductRepository.UpdateProductFromCart(cartProductInCart);
                        await this._unitOfWork.SaveChangesAsync();
                    }
                    else
                    {
                        // add product to cart
                        CartProduct newCartProduct = new CartProduct()
                        {
                            Amount = cartProduct.Amount.Value,
                            CartId = cart.Id,
                            Color = cartProduct.Color,
                            Size = cartProduct.Size,
                            ProductId = cartProduct.ProductId,
                            Status = (int)CartProductEnum.Status.ACTIVE,//Active
                        };
                        await this._unitOfWork.CartProductRepository.AddToCart(newCartProduct);
                        await this._unitOfWork.SaveChangesAsync();
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task Checkout(CheckoutViewModel checkoutViewModel, string username)
        {
            try
            {
                User user = await this._unitOfWork.UserRepository.GetUserAsync(username);
                ShippingAddress shippingAddress = await this._unitOfWork.ShippingAddressRepository
                                                .GetShippingAddressByIdAsync(checkoutViewModel.ShippingAddressId, user.Id);
                if(shippingAddress == null)
                {
                    throw new Exception("Shipping address of user does not exist!");
                }
                DeliveryMethod deliveryMethod = await this._unitOfWork.DeliveryMethodRepository
                                                .GetDeliveryMethodByIdAsync(checkoutViewModel.DeliveryMethodId);
                if(deliveryMethod == null)
                {
                    throw new Exception("Delivery method does not exist!");
                }
                List<CartProductDetailsViewModel> cartProducts = await this.GetCartProducts(username);
                if (cartProducts == null)
                {
                    throw new Exception("User cart is empty!");
                }
                List<ProductOrder> productOrders = new List<ProductOrder>();
                foreach (var product in cartProducts)
                {
                    Product existedProduct = await this._unitOfWork.ProductRepository.GetProductDetail(product.ProductId);
                    if (existedProduct.TotalProduct <= 0)
                    {
                        throw new Exception($"{product.ProductName} is out of stock");
                    }
                    DescribeProduct describeProduct = await this._unitOfWork.DescribeProductRepository
                                                        .GetDescribeProductAsync(product.ProductId, product.ColorName, product.SizeName);
                    if(describeProduct.Amount <= 0)
                    {
                        throw new Exception($"{product.ProductName} with {product.ColorName} color and {product.SizeName} size is out of stock ");
                    }
                    productOrders.Add(new ProductOrder
                    {
                        IdProduct = product.ProductId,
                        Amount = product.Amount,
                        Size = product.SizeName,
                        Color = product.ColorName,
                        Price = product.Price - Convert.ToDecimal(Convert.ToSingle(product.Price) * product.Discount)
                    });

                }
                DateTime orderedDate = DateTime.Now;
                Order order = new Order()
                {
                    DeliveryMethodId = deliveryMethod.Id,
                    DeliveryMethodPrice = deliveryMethod.Price,
                    DeliveryStatus = (int)OrderEnum.DeliveryStatus.CONFIRMING,
                    ShippingAddress = shippingAddress.Address,
                    Status = (int)OrderEnum.Status.ACTIVE,
                    OrderedDate = orderedDate,
                    UserID = user.Id,
                    ProductOrders = productOrders
                };
                await this._unitOfWork.OrderRepository.CreateOrderAsync(order);
                List<Product> products = new List<Product>();
                foreach (var orderedProduct in productOrders)
                {
                    if(products.Count > 0)
                    {
                        if(products.FirstOrDefault(x => x.Id == orderedProduct.IdProduct) == null)
                        {
                            products.Add(new Product
                            {
                                Id = orderedProduct.IdProduct.Value,
                                TotalProduct = orderedProduct.Amount.Value
                            });
                        } else
                        {
                            products.FirstOrDefault(x => x.Id == orderedProduct.IdProduct).TotalProduct += orderedProduct.Amount.Value;
                        }
                    } else
                    {
                        products.Add(new Product
                        {
                            Id = orderedProduct.IdProduct.Value,
                            TotalProduct = orderedProduct.Amount.Value,
                        });
                    }
                }
                foreach (var orderedProduct in products)
                {
                    Product product = await this._unitOfWork.ProductRepository.GetProductDetail(orderedProduct.Id.Value);
                    product.TotalProduct -= orderedProduct.TotalProduct;
                    if(product.TotalProduct <= 0)
                    {
                        product.Status = (int)ProductEnum.Status.OUTOFSTOCK;
                    }
                    this._unitOfWork.ProductRepository.UpdateProductAsync(product);
                }
                foreach(var product in productOrders)
                {
                    List<DescribeProduct> describeProducts = await this._unitOfWork.DescribeProductRepository.GetDescribeProductsAsync(product.IdProduct.Value);
                    foreach (var describeProduct in describeProducts)
                    {
                        if (describeProduct.ProductId == product.IdProduct.Value && describeProduct.Color.Name.ToLower().Equals(product.Color.ToLower())
                            && describeProduct.Size.Name.ToLower().Equals(product.Size.ToLower()))
                        {
                            describeProduct.Amount -= product.Amount.Value;
                            this._unitOfWork.DescribeProductRepository.UpdateDescribeProduct(describeProduct);
                        }
                    }
                }
                Cart cart = await this._unitOfWork.CartRepository.GetUserCart(user.Id);
                List<CartProduct> existedCartProducts = await this._unitOfWork.CartProductRepository.GetCartProducts(cart.Id);
                foreach (var cartProduct in existedCartProducts)
                {
                    cartProduct.Status = (int)CartProductEnum.Status.INACTIVE;
                    this._unitOfWork.CartProductRepository.UpdateProductFromCart(cartProduct);
                }

                await this._unitOfWork.SaveChangesAsync();
            } catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<CartProductDetailsViewModel>> GetCartProducts(string username)
        {
            try
            {
                User user = await this._unitOfWork.UserRepository.GetUserAsync(username);

                Cart cart = await this._unitOfWork.CartRepository.GetUserCart(user.Id);

                if(cart == null)
                {
                    return new List<CartProductDetailsViewModel>();
                }

                List<CartProduct> cartProducts = await this._unitOfWork.CartProductRepository.GetCartProducts(cart.Id);
                List<CartProductDetailsViewModel> cartProductsDetails = new List<CartProductDetailsViewModel>();
                foreach (CartProduct product in cartProducts)
                {
                    CartProductDetailsViewModel cartProductDetailsViewModel = new CartProductDetailsViewModel
                    {

                        ProductId = product.ProductId,
                        ProductName = product.Product.Name,
                        Price = product.Product.DescribeProducts.FirstOrDefault(dp => dp.ProductId.Value == product.ProductId
                                    && dp.Color.Name.ToLower().Equals(product.Color.ToLower())
                                    && dp.Size.Name.ToLower().Equals(product.Size.ToLower())).Price.Value,
                        Amount = product.Amount,
                        ColorName = product.Color,
                        SizeName = product.Size,
                        Discount = product.Product.Discount.Value,
                    };
                    if(product.Product.ProductImages != null)
                    {
                        List<ProductImageViewModel> productImageViewModels = new List<ProductImageViewModel>();
                        foreach (var productImage in product.Product.ProductImages)
                        {
                            if(productImage.ProductId == product.ProductId)
                            {
                                productImageViewModels.Add(new ProductImageViewModel
                                {
                                    Source = productImage.Source
                                });
                                cartProductDetailsViewModel.ProductImageViewModels = productImageViewModels;
                            }
                        }
                    }
                    cartProductsDetails.Add(cartProductDetailsViewModel);
                }
                return cartProductsDetails;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task RemoveFromCart(CartProductViewModel cartProduct, string username)
        {
            try
            {
                Product existedProduct = await this._unitOfWork.ProductRepository.GetProductDetail(cartProduct.ProductId);
                if (existedProduct == null)
                {
                    throw new Exception("Product does not exsit!");
                }
                User existedUser = await this._unitOfWork.UserRepository.GetUserAsync(username);
                Cart userCart = await this._unitOfWork.CartRepository.GetUserCart(existedUser.Id);
                if (userCart != null)
                {
                    CartProduct existedCartProduct = await this._unitOfWork.CartProductRepository.GetProductFromCart(userCart.Id, cartProduct.ProductId,
                                                                                cartProduct.Color, cartProduct.Size);
                    if (existedCartProduct == null)
                    {
                        throw new Exception("Product does not exist in Cart!");
                    }
                    existedCartProduct.Status = (int)CartProductEnum.Status.INACTIVE;
                    existedCartProduct.Amount = 0;
                    this._unitOfWork.CartProductRepository.UpdateProductFromCart(existedCartProduct);
                    await this._unitOfWork.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
