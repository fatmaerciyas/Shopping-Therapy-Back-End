//using AutoMapper;
//using BusinessLayer.Concrete;
//using DataAccessLayer.Concrete;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;

//namespace OneStopShop.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class CargoController : BaseApiController
//    {
//        UnitOfWork unitOfWork = new UnitOfWork();
//        private readonly IMapper _mapper;
//        private readonly Context _contex;


//        public CargoController(IMapper mapper, Context context)
//        {
//            _mapper = mapper;
//            _contex = context;

//        }

//        [HttpGet]
//        public async Task<ActionResult> GetAllOrder()
//        {
//            var cargos = unitOfWork.cartManager.Get
//            if (carts != null)
//            {
//                return Ok(carts);
//            }
//            return BadRequest();

//        }

//    }
//}
