using AutoMapper;
using BusinessLayer.Concrete;
using DTOLayer.DTOs.CategoryDTOs;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OneStopShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : BaseApiController
    {

        UnitOfWork unitOfWork = new UnitOfWork();
        private readonly IMapper _mapper;

        public CategoryController(IMapper mapper)
        {
            _mapper = mapper;
        }



        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var values = unitOfWork.categoryManager.GetList();
            return Ok(values);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var values = unitOfWork.categoryManager.GetById(id);
            return Ok(values);

        }



        [HttpPost]
        public async Task<IActionResult> Add(CreateCategoryDTO model)
        {

            var entity = new Category
            {
                Name = model.Name,
                Description = model.Description,
                Image = model.Image,
          
            };

            if(model.Image == "")
            {
                entity.Image = "/public/assets/images/shop/categories/fashion.jpg";
            }

            try
            {

                unitOfWork.categoryManager.Insert(entity);
                unitOfWork.Complete();
                unitOfWork.Dispose();
                return Ok(model);
            }
            catch (Exception e)
            {
                return NoContent();
            }

        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateCategoryDTO model)
        {
            

            var entity = new Category
            {
                CategoryId = model.CategoryId,
                Name = model.Name,
                Description = model.Description,
                Image = model.Image,

            };

            if (model.Image == "")
            {
                entity.Image = "/public/assets/images/shop/categories/fashion.jpg";
            }

            try
            {

                unitOfWork.categoryManager.Update(entity);
                unitOfWork.Complete();
                unitOfWork.Dispose();
                return Ok(model);
            }
            catch (Exception e)
            {
                return NoContent();
            }

        }
        [HttpPost]
        [Route("deleteItem")]
        public async Task<IActionResult> Delete(int id)
        {
            Category entity = new Category();
            try
            {

                entity = unitOfWork.categoryManager.GetById(id);
                unitOfWork.categoryManager.Delete(entity);
                unitOfWork.Complete();
                unitOfWork.Dispose();
                return Ok();
            }
            catch (Exception e)
            {
                return NoContent();
            }
        }
    }
}

