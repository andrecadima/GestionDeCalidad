using System.Runtime.InteropServices.JavaScript;
using MicroServicioUser.App.Services;
using MicroServicioUser.Dom.Dto;
using MicroServicioUser.Dom.Entities;
using MicroServicioUser.Dom.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Org.BouncyCastle.Bcpg.OpenPgp;
using System.ComponentModel.DataAnnotations;

namespace MicroServicioUsuario.API.Controllers
{
    public class ChangePasswordDto
    {
        [Required]
        public int ? Id { get; set; }
        [Required]
        public string ? CurrentPassword { get; set;  }
        [Required]
        public string ? NewPassword{ get; set; }
    }
    public class LoginDto{
        [Required]
        public string ? Username { get; set; }
        [Required]
        public string ? Password { get; set; }
    }
    
    public class RegisterDto{
        [Required]
        public string ? FirstName { get; set; }
        [Required]
        public string ? LastName { get; set; }
        public string ? Email { get; set; }
        [Required]
        public string ? Role { get; set; }
        [Required]
        public int? CreatedBy { get; set; }
    }
    
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Require authentication for all endpoints by default
    public class UserController : ControllerBase
    {
        private readonly IRepositoryService<User> service;
        private readonly LoginService loginService;
        private readonly RegistrationService registrationService;
        private readonly IJwtService jwtService;
        
        public UserController(IRepositoryService<User> service, LoginService loginService, RegistrationService registrationService,
            IJwtService jwtService)
        {
            this.service = service;
            this.loginService = loginService;
            this.registrationService = registrationService;
            this.jwtService = jwtService;
        }
        
            
        [HttpPost("change-password")]
        [AllowAnonymous]
        public async Task<ActionResult<bool>> ChangePasswordFirstLogin([FromBody] ChangePasswordDto cpDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!cpDTO.Id.HasValue)
                return BadRequest(new { Ok = false, Error = "Id is required." });

            var res = await this.loginService.ChangePasswordFirstLogin(
                cpDTO.Id.Value,
                cpDTO.CurrentPassword!,
                cpDTO.NewPassword!
            );

            return Ok(new { Ok = res.ok, Error = res.error });
        }
        [HttpPost("login")]
        [AllowAnonymous] // Allow anonymous access for login
        public async Task<ActionResult<bool>> Login([FromBody] LoginDto loginDTO)
        {

            var obj = await this.loginService.ValidateLogin(loginDTO.Username!, loginDTO.Password!);

            if (obj.ok && obj.role != null && obj.userId != null)
            {
                var result = await jwtService.GenerateToken(obj.userId.Value, obj.role);

                return Ok(new LoginResponse()
                {
                    Error = "",
                    Ok = true,
                    Token = result.Value,
                    TokenType = "Bearer"
                });
            }

            return Ok(new LoginResponse() { Error = obj.error, Ok = false });
        }
        
        [HttpPost("register")]
        [AllowAnonymous] 
      public async Task<ActionResult<bool>> Register([FromBody] RegisterDto registerDto)
        {
            if (!registerDto.CreatedBy.HasValue)
            {
                return BadRequest(new { Ok = false, Error = "CreatedBy is required." });
            }
            var obj = await this.registrationService.RegisterUser(
                registerDto.FirstName!,
                registerDto.LastName!,
                registerDto.Email!,
                registerDto.Role!,
                registerDto.CreatedBy.Value
            );
            return Ok(new { Ok = obj.ok, Error = obj.error });
        }
            
        [HttpGet("select")]
        public async Task<ActionResult<List<User>>> Select()
        {
            var categories = await service.GetAll();
            if (categories.IsSuccess)
            {
                return Ok(categories.Value);
            }
            else
            {
                return StatusCode(500, new
                {
                    message = "Error al obtener los usuarios",
                    error = categories.Errors
                });
            }
        }

        [HttpGet("getById/{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<User>> GetById(int id)
        {
            var category = await service.GetById(id);
            if (category.IsSuccess)
            {
                return Ok(category.Value);
            }
            else
            {
                return StatusCode(500, new
                {
                    message = "Error al obtener el usuario",
                    error = category.Errors
                });
            }
        }

        // GET: api/User/search/comida

        [HttpGet("search/{property}")]
        public async Task<ActionResult<List<User>>> Search(string property)
        {
            var categories = await service.Search(property);
            if (categories.IsSuccess)
            {
                return Ok(categories.Value);
            }
            else
            {
                return StatusCode(500, new
                {
                    message = "Error al buscar usuario",
                    error = categories.Errors
                });
            }
        }

        // POST: api/User

        [HttpPost("create")]
        public async Task<ActionResult> Create([FromBody] User User)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await service.Insert(User);

            if (result.IsSuccess)
            {
                return CreatedAtAction(
                    nameof(Select),
                    new { id = result.Value },
                    new
                    {
                        id = result.Value,
                        message = "Usuario creado exitosamente",
                        data = User
                    }
                );
            }
            else
            {
                return StatusCode(500, new
                {
                    message = "Error al crear el usuario",
                    error = result.Errors
                });
            }
        }

        // PUT: api/User/5

        [HttpPut("update")]
        public async Task<ActionResult> Update([FromBody] User User)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await service.Update(User);

            if (result.IsSuccess)
            {
                return Ok(new
                {
                    message = "Usuario actualizado exitosamente",
                    data = User
                });
            }
            else
            {
                if (result.Errors.Any(e => e.Contains("No se encontró")))
                {
                    return NotFound(new
                    {
                        message = $"Usuario con ID {User.Id} no encontrada"
                    });
                }
                else
                {
                    return StatusCode(500, new
                    {
                        message = "Error al actualizar el usuario",
                        error = result.Errors
                    });
                }
            }
        }

        // DELETE: api/User/5

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await service.Delete(id);

            if (result.IsSuccess)
            {
                return Ok(new
                {
                    message = $"Usuario con ID {id} eliminada exitosamente"
                });
            }
            else
            {
                if (result.Errors.Any(e => e.Contains("No se encontró")))
                {
                    return NotFound(new
                    {
                        message = $"Usuario con ID {id} no encontrada"
                    });
                }
                else
                {
                    return StatusCode(500, new
                    {
                        message = "Error al eliminar el usario",
                        error = result.Errors
                    });
                }
            }
        }
    }
}
