using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;
using RestaurantAPI.Services;

namespace RestaurantAPI.Controllers
{
    [Route("api/restaurant")]
    [ApiController]
    [Authorize]

    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;
        public RestaurantController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [HttpPut("{id}")]
        public ActionResult Modify([FromRoute] int id, [FromBody] ModifyRestaurantDto dto)
        {
            //Zadanie praktyczne: Modyfikacja encji

            //if (!ModelState.IsValid) //o tym zapomniałam, a w dalszej części kursu ten fragment kodu zostaje usunięty po dopisaniu atrybutu ApiController
            //{
            //    return BadRequest(ModelState);
            //}
            
            if (_restaurantService.GetById(id) is null) //powinnam to sprawdzić wewnątrz metody
            {
                return NotFound();
            }
            else
            {
                _restaurantService.Modify(id, dto);
                return Ok();
            }
            
        }

        /// <summary>
        /// Delete by Id action
        /// </summary>
        /// <param name="id">Restaurant Id</param>
        /// <returns>Action result</returns>
        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            _restaurantService.Delete(id);

            return NoContent();
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        //[Authorize(Roles = "Manager")] //dwa parametry to BŁĄD
        public  ActionResult CreateRestaurant([FromBody] CreateRestaurantDto dto)
        {
            var userId = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var id =_restaurantService.Create(dto);

            return Created($"api/restaurant/{id}", null);
        }

        [HttpGet]
        //[Authorize(Policy = "AtLeast20")]
        //[Authorize(Policy = "AtLeastTwoRestaurantCreated")]
        [AllowAnonymous] // od 45 lekcji

        public ActionResult<IEnumerable<RestaurantDto>> GetAll([FromQuery]RestaurantQuery query)
        {

            var restaurantsDtos = _restaurantService.GetAll(query);

            return Ok(restaurantsDtos);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public ActionResult<RestaurantDto> Get([FromRoute] int id)
        {

            var restaurant = _restaurantService.GetById(id);

            return Ok(restaurant);
        }
    }
}
