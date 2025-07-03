using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using todoTask.Models.Domain;
using todoTask.Models.DTO;
using todoTask.Repositories;

namespace todoTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodotaskController : ControllerBase
    {
        private IMapper _mapper;
        private readonly ITodotaskRepository todotaskRepository;

        public TodotaskController(IMapper mapper, ITodotaskRepository todotaskRepository)
        {
            this._mapper = mapper;
            this.todotaskRepository = todotaskRepository;
        }

        [HttpPost]
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
        public async Task<IActionResult> GetAllData()
        {
            var todoTaskDomain = await todotaskRepository.GetAllDataAsync();
            var result = _mapper.Map<List<Todotask>>(todoTaskDomain);
            return Ok(result);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var getData = await todotaskRepository.GetById(id);

            return Ok(getData);
        }

        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateTodotaskDto updateTodotaskDto)
        {
            var todotaskDomain = _mapper.Map<Todotask>(updateTodotaskDto);
            var updatetask = await todotaskRepository.UpdateAsync(id, todotaskDomain);
            if (updatetask == null)
                return NotFound();

            var responseDto = _mapper.Map<UpdateTodotaskDto>(updatetask);
            return Ok(responseDto);

        }

        [HttpDelete]
        [Route("{id:Guid}")]
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
