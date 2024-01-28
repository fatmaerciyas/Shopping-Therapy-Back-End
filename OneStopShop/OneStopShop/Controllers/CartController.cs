using AutoMapper;
using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using DataAccessLayer.Concrete;
using DTOLayer.DTOs.AuthDTOs;
using DTOLayer.DTOs.CartDTOs;
using DTOLayer.DTOs.CartItemDTOs;
using DTOLayer.DTOs.CategoryDTOs;
using DTOLayer.DTOs.ProductDTOs;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace OneStopShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : BaseApiController
    {

        UnitOfWork unitOfWork = new UnitOfWork();
        private readonly IMapper _mapper;
        private readonly Context _contex;
        private readonly IAuthService _authService;


        public CartController(IMapper mapper, Context context, IAuthService authService)
        {
            _mapper = mapper;
            _contex = context;
            _authService = authService;
        }

        //Carts- if shopping is ot complete
        [HttpGet]
        [Route("carts")]
        public async Task<ActionResult> GetAll()
        {
           

            var cartItems = unitOfWork.cartManager.GetCartProductRelationship();
            List<Cart> cart = new List<Cart>();
            if (cartItems != null)
            {
                foreach (var item in cartItems)
                {
                    if (item.BasketId == null)
                    {
                        cart.Add(item);
                    }
                }
                return Ok(cart);

            }

            return BadRequest();
            //var cartItems = unitOfWork.cartManager.GetList();
            //List<Cart> cart = new List<Cart>();
            //if (cartItems != null)
            //{
            //    foreach (var item in cartItems)
            //    {
            //        if (item.BasketId == null)
            //        {
            //            cart.Add(item);
            //        }
            //    }
            //    return Ok(cart);

            //}

            //return BadRequest();

        }

        //Orders By User
        [HttpGet]
        [Route("orders")]
        public async Task<ActionResult> GetAllOrderByUsername(string username)
        {
            var carts = unitOfWork.cartManager.GetCartProductRelationship();

            var basket = unitOfWork.basketManager.GetByUserName(username);

            List<Cart> cart = new List<Cart>();
            if (carts != null)
            {
                foreach (var item in carts)
                {
                    foreach (var basketItem in basket)
                    {
                        if (item.BasketId == basketItem.BasketId)
                        {
                            cart.Add(item);
                        }
                    }
                  
                }
                return Ok(cart);

            }

            return BadRequest();

        }


        [HttpGet]
        [Route("allOrders")]
        public async Task<ActionResult> GetAllOrder()
        {
            var carts = unitOfWork.cartManager.GetCartProductRelationship();
            if (carts != null)
            { 
                return Ok(carts);
            }
            return BadRequest();

        }


        [HttpGet]
        [Route("getById")]
        public async Task<ActionResult> GetById(int id)
        {
            var carts = unitOfWork.cartManager.GetByIdCartProductRelationship(id);


            if (carts != null)
            {
                return Ok(carts);
            }
            return BadRequest();

        }

        [HttpGet]
        [Route("getCargoTypes")]
        public async Task<ActionResult> GetCargoTypes()
        {
            List<int> cargoTypeIds = Enum.GetValues(typeof(Cart.CargoTypes)).Cast<int>().ToList();

            List<Cart.CargoTypes> cargoTypes = cargoTypeIds.Select(id => (Cart.CargoTypes)id).ToList();

            return Ok(cargoTypes);

        }

        [HttpPost]
        [Route("carts")]
        public void Add(int productId, int quantity = 1)
        {
            using (var _context = new Context())
            {
                var items = _context.Carts.Where(cart => cart.ProductId == productId).ToList();
                var product = new Product();

                foreach (var item in items)
                {
                    //Urun varsa quantity'yi 1 artır
                    if ( item.BasketId == null )
                    {
                       
                        item.Quantity += quantity;

                        product = unitOfWork.productManager.GetById(item.ProductId);

                        if (product != null)
                        {
                            // Decrease the Stock property of the Product entity
                            product.Stock -= quantity;
                           
                        }
                        unitOfWork.cartManager.Update(item);
                        unitOfWork.Complete();
                        unitOfWork.Dispose();

                    }

                }
                //Urun yoksa ekle

               product = unitOfWork.productManager.GetById(productId);

                if (product != null)
                {
                    // Decrease the Stock property of the Product entity
                    product.Stock -= 1;

                }
                unitOfWork.cartManager.Insert(new Cart { ProductId = productId, Quantity = quantity, CargoTypeId = 0 });
                unitOfWork.Complete();
                unitOfWork.Dispose();
            }

        }

        [HttpPost]
        [Route("updateCargoType")]
        public void UpdateCargoType(int productId, int CargoTypeId, int basketId)
        {
            using (var _context = new Context())
            {
                var item = _context.Carts.Where(cart => cart.ProductId == productId && cart.BasketId == basketId).FirstOrDefault();

                item.CargoTypeId = CargoTypeId;
                unitOfWork.cartManager.Update(item);

                unitOfWork.Complete();
                unitOfWork.Dispose();
            }

        }

        [HttpDelete]
        public void RemoveItem(int productId, int quantity)
        {
            using (var _context = new Context())
            {
                var items = _context.Carts.Where(cart => cart.ProductId == productId).ToList();

                foreach (var item in items)
                {

                    if (item.BasketId == null)
                    {

                        if (item.Quantity == 0)
                        {
                            unitOfWork.cartManager.Delete(item);
                            unitOfWork.Complete();
                            unitOfWork.Dispose();
                        }

                        else
                        {
                            item.Quantity -= quantity;
                            unitOfWork.cartManager.Update(item);
                            unitOfWork.Complete();
                            unitOfWork.Dispose();
                        }

                    }
                }

            }
        }


        [HttpDelete]
        [AllowAnonymous]
        [Route("deleteCart")]
        public void DeleteCart(int cartId)
        {
            using (var _context = new Context())
            {
                var item = _context.Carts.Where(cart => cart.CartId == cartId).FirstOrDefault();

                if(item != null)
                {
                    unitOfWork.cartManager.Delete(item);
                }
                unitOfWork.Complete();
                unitOfWork.Dispose();
            }
        }
        //[HttpPost]
        //public async Task<IActionResult> Add(CreateCartDTO model)
        //{
        //    try
        //    {
        //        var entity = _mapper.Map<Cart>(model);

        //        unitOfWork.cartManager.Insert(entity);
        //        unitOfWork.Complete();
        //        unitOfWork.Dispose();
        //        return Ok(model);
        //    }
        //    catch (Exception e)
        //    {
        //        return NoContent();
        //    }

        //}

        //[Route("{id}")] //look
        //[HttpDelete]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    Cart entity = new Cart();
        //    try
        //    {

        //        entity = unitOfWork.cartManager.GetById(id);
        //        unitOfWork.cartManager.Delete(entity);
        //        return Ok();
        //    }
        //    catch (Exception e)
        //    {
        //        return NoContent();
        //    }
        //}
    }
}
