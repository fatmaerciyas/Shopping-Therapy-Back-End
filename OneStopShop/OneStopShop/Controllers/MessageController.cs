using AutoMapper;
using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using DataAccessLayer.Concrete;
using DTOLayer.DTOs.MessageDTOs;
using DTOLayer.DTOs.ProductDTOs;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OneStopShop.Constants;

namespace OneStopShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : BaseApiController
    {

        private readonly IMessageService _messageService;
        Context _context;
        UnitOfWork unitOfWork = new UnitOfWork();


        public MessageController(IMessageService messageService, Context context)
        {
            _context = context;
            _messageService = messageService;
        }

        // Route -> Create a new message to send to another user
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateNewMessage([FromBody] CreateMessageDTO createMessageDto)
        {
            var result = await _messageService.CreateNewMessageAsync(User, createMessageDto);
            if (result.IsSucceed)
                return Ok(result.Message);

            return StatusCode(result.StatusCode, result.Message);
        }

        [HttpPost]
        [Route("deleteMessage")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            Message entity = new Message();

            try
            {

                entity = unitOfWork.messageManager.GetById(id);
                unitOfWork.messageManager.Delete(entity);
                unitOfWork.Complete();
                unitOfWork.Dispose();
                return Ok();
            }
            catch (Exception e)
            {
                return NoContent();
            }

        }

        // Route -> Get All Messages for current user, Either as Sender or as Receiver
        [HttpGet]
        [Route("inbox")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<SelectMessageDTO>>> GetMyMessages()
        {
            // var messages = await _messageService.GetMyMessagesAsync(User);

            var loggedInUser = User.Identity.Name;

            var inboxMessages = _context.Messages
                .Where(q => q.Receiver == loggedInUser);




            if (inboxMessages != null)
            {
                var inbox = await _messageService.GetMyInboxAsync(User);
                return Ok(inbox);
            }
            else
            {
                return BadRequest();
            }

        }

        [HttpGet]
        [Route("sendbox")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<SelectMessageDTO>>> GetMySendBoxMessages()
        {
            // var messages = await _messageService.GetMyMessagesAsync(User);

            var loggedInUser = User.Identity.Name;


            var sendboxMessages = _context.Messages
                .Where(q => q.Sender == loggedInUser);

            if (sendboxMessages != null)
            {
                var sendbox = await _messageService.GetMySendboxAsync(User);
                return Ok(sendbox);
            }
            else
            {
                return BadRequest();
            }

        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            Message entity = new Message();
            try
            {

                entity = unitOfWork.messageManager.GetById(id);
                if (entity == null) return NotFound();

                return Ok(entity);
            }
            catch (Exception e)
            {
                return NoContent();
            }

        }

        // Route -> Get all messages With Owner access and Admin access
        [HttpGet]
        [Authorize(Roles = StaticUserRoles.OwnerAdmin)]
        public async Task<ActionResult<IEnumerable<SelectMessageDTO>>> GetMessages()
        {
            var messages = await _messageService.GetMessagesAsync();
            return Ok(messages);
        }



        //UnitOfWork unitOfWork = new UnitOfWork();
        //private readonly IMapper _mapper;

        //public MessageController(IMapper mapper)
        //{
        //    _mapper = mapper;
        //}



        //[HttpGet]
        //public async Task<IActionResult> GetAll()
        //{
        //    var values = unitOfWork.messageManager.GetList();
        //    return Ok(values);

        //}

        //[Route("{id}")]
        //[HttpGet]
        //public async Task<IActionResult> GetById(int id)
        //{
        //    Message entity = new Message();
        //    try
        //    {

        //        entity = unitOfWork.messageManager.GetById(id);
        //        var model = _mapper.Map<SelectMessageDTO>(entity);
        //        return Ok(model);
        //    }
        //    catch (Exception e)
        //    {
        //        return NoContent();
        //    }

        //}


        //[HttpPost]
        //public async Task<IActionResult> Add(CreateMessageDTO model)
        //{
        //    try
        //    {
        //        var entity = _mapper.Map<Message>(model);

        //        unitOfWork.messageManager.Insert(entity);
        //        unitOfWork.Complete();
        //        unitOfWork.Dispose();
        //        return Ok(model);
        //    }
        //    catch (Exception e)
        //    {
        //        return NoContent();
        //    }

        //}

        //[HttpDelete]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    Message entity = new Message();
        //    try
        //    {

        //        entity = unitOfWork.messageManager.GetById(id);
        //        unitOfWork.messageManager.Delete(entity);
        //        return Ok();
        //    }
        //    catch (Exception e)
        //    {
        //        return NoContent();
        //    }
        //}
    }
}
