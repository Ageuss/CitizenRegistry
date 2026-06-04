using CitizenRegistry.API.Domain.API.DTOs;
using CitizenRegistry.API.Domain.API.Entities;
using CitizenRegistry.API.Domain.API.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CitizenRegistry.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CitizenController : ControllerBase
    {
        private readonly ICitizenService _citizenService;
        private readonly IValidator<CitizenRequestDTO> _validator;

        public CitizenController(ICitizenService citizenService, IValidator<CitizenRequestDTO> validator)
        {
            _citizenService = citizenService;
            _validator = validator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CitizenRequestDTO requestDTO)
        {
            var validationResult = await _validator.ValidateAsync(requestDTO);

            if (!validationResult.IsValid)
                return BadRequest(new { errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList()});

            var citizenEntity = new Citizen
            {
                Name = requestDTO.Name,
                Cpf = requestDTO.Cpf
            };

            try
            {
                var createdCitizen = await _citizenService.CreateCitizenAsync(citizenEntity);

                var response = new CitizenResponseDTO
                {
                    Name = createdCitizen.Name,
                    Cpf = createdCitizen.Cpf
                };

                return CreatedAtAction(nameof(GetById), new { citizenEntity.Id }, response);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var citizen = await _citizenService.GetCitizenByIdAsync(id);

                if (citizen == null)
                    NotFound("Cidadão não encontrado");

                var response = new CitizenResponseDTO
                {
                    Name = citizen?.Name,
                    Cpf = citizen?.Cpf
                };

                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetByName(string? name)
        {
            try
            {
                var citizens = await _citizenService.GetCitizensByNameAsync(name);

                if (!citizens.Any())
                    return NotFound("Nenhum cidadão encontrado");

                var responseList = citizens.Select(c => new CitizenResponseDTO
                {
                    Name = c.Name,
                    Cpf = c.Cpf
                });

                return Ok(responseList);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("cpf/{cpf}")]
        public async Task<IActionResult> GetByCpf(string? cpf)
        {
            try
            {
                var citizen = await _citizenService.GetCitizenByCpfAsync(cpf);

                if (citizen == null)
                    return NotFound("Cidadão não encontrado");

                var response = new CitizenResponseDTO
                {
                    Name = citizen?.Name,
                    Cpf = citizen?.Cpf
                };

                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var citizens = await _citizenService.GetAllCitizensAsync();

                var responseList = citizens.Select(c => new CitizenResponseDTO
                {
                    Name = c.Name,
                    Cpf = c.Cpf
                });

                return Ok(responseList);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
