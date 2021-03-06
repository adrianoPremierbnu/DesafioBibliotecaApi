using DesafioBibliotecaApi.DTOs;
using DesafioBibliotecaApi.Entities;
using DesafioBibliotecaApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;

namespace DesafioBibliotecaApi.Controllers
{
    [ApiController, Route("[controller]")]
    public class AuthorController : ControllerBase
    {
        private readonly AuthorService _authorService;

        public AuthorController(AuthorService authorService)
        {
            _authorService = authorService;
        }

        //[HttpPost, Authorize(Roles = "Admin, Functionary"), Route("authors")]
        [HttpPost, Route("authors")]
        public IActionResult Create([FromBody] NewAuthorDTO authorDTO)
        {
            authorDTO.Validar();

            if (!authorDTO.Success)
                return BadRequest(authorDTO.Errors);

            try
            {
                var author = new Author(authorDTO.Name, authorDTO.Lastname, authorDTO.Nacionality, authorDTO.Document, authorDTO.Age);

                var result = _authorService.Create(author);

                if (!result.Success)
                    return BadRequest(result);
                else
                    return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest("Error creating author : " + ex.Message);
            }      
                        
        }

        //[HttpGet, Authorize, Route("authors")]
        [HttpGet, Route("authors")]
        public IActionResult Get([FromQuery] string? name, [FromQuery] string? nationality, [FromQuery] int? age, [FromQuery] int page = 1, [FromQuery] int itens = 50)
        {
            try
            {
                var userId = User.Claims.First(c => c.Type == ClaimTypes.Sid).Value;

            }
            catch (Exception ex)
            {
               return BadRequest("User not authenticated");
            }

            return Ok(_authorService.GetFilter(name, nationality, age, page, itens));
            
        }

        //[HttpGet, Authorize, Route("{id}/authors")]
        [HttpGet, Route("{id}/authors")]
        public IActionResult Get(Guid id)
        {
            /*try
            {
                var userId = User.Claims.First(c => c.Type == ClaimTypes.Sid).Value;

            }
            catch (Exception ex)
            {
                return BadRequest("User not authenticated");
            } */           

            return Ok(_authorService.Get(id));

        }

        //[HttpDelete, Authorize(Roles = "Admin, Functionary"), Route("{id}/authors")]
        [HttpDelete, Route("{id}/authors")]
        public IActionResult Delete(Guid id)
        {
            var result = _authorService.Delete(id);

            if (!result.Success)
                return BadRequest(result);
            else
                return Ok(result);

        }

        //[HttpPut, Authorize(Roles = "Admin, Functionary"), Route("authors")]
        [HttpPut, Route("authors")]
        public IActionResult UpdateAuthor(Guid id, NewAuthorDTO authorDTO)
        {
            authorDTO.Validar();

            if (!authorDTO.Success)
                return BadRequest(authorDTO.Errors);

            try
            {
                var author = new Author(authorDTO.Name, authorDTO.Lastname, authorDTO.Nacionality, authorDTO.Document, authorDTO.Age, id);

                var result = _authorService.UpdateAuthor(author);

                if (!result.Success)
                    return BadRequest(result);
                else
                    return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest("Error updating author : " + ex.Message);
            }

        }


    }
}
