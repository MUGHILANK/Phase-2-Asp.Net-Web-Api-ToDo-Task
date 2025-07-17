using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using todoTask.Models.Domain;
using todoTask.Models.DTO;
using todoTask.Repositories;

namespace todoTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // Jwt Authorize 
    // Error 401: Unauthorized user
    //[Authorize]
    public class TodotaskController : ControllerBase
    {
        private IMapper _mapper;
        private readonly ITodotaskRepository todotaskRepository;

        public TodotaskController(IMapper mapper, ITodotaskRepository todotaskRepository)
        {
            this._mapper = mapper;
            this.todotaskRepository = todotaskRepository;
        }
        //POST /api/Todotask/Create
        [HttpPost]
        [Route("Create")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromBody] AddTodotaskDto addTodotaskDto)
        {
            //Map DTO to Domain Model
            var todotaskDomain = _mapper.Map<Todotask>(addTodotaskDto);
            var createdTodoTask = await todotaskRepository.CreateAsync(todotaskDomain);
            var responseDto = _mapper.Map<AddTodotaskDto>(todotaskDomain);
            //Map domain model to DTO
            return Ok(responseDto);
        }

        [HttpGet]
        [Authorize(Roles = "Reader,Writer")]
        public async Task<IActionResult> GetAllData()
        {
            var todoTaskDomain = await todotaskRepository.GetAllDataAsync();
            var result = _mapper.Map<List<Todotask>>(todoTaskDomain);
            return Ok(result);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Reader,Writer")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var getData = await todotaskRepository.GetById(id);

            return Ok(getData);
        }

        [HttpPut("{id:Guid}")]
        [Authorize(Roles = "Writer,Reader")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateTodotaskDto updateTodotaskDto)
        {
            var todotaskDomain = _mapper.Map<Todotask>(updateTodotaskDto);
            var updatetask = await todotaskRepository.UpdateAsync(id, todotaskDomain);
            if (updatetask == null)
            {
                return NotFound(new {message =  "Message: Data is not Found!" });
            }

            var responseDto = _mapper.Map<UpdateTodotaskDto>(updatetask);
            return Ok(new { message = "Task Updated successfully" });
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var todotask = await todotaskRepository.GetById(id);
            if (todotask == null)
            {
                return NotFound();
            }
            
            await todotaskRepository.DeleteAsync(id);
            return Ok(new { message = "Task deleted successfully" });

        }
    }
}
