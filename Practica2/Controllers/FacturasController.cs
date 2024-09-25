using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;
using Practica2.Models;
using Practica2.Repositories.Implementations;
using System.Collections.Generic;

namespace Practica2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class FacturasController : ControllerBase
    {

        private AplicacionRepository aplicacionRepository ;


        public FacturasController()
        {
            aplicacionRepository = new AplicacionRepository();
        }


        [HttpGet("FacturasGET")]

        public IActionResult Get()
        {
            return Ok(aplicacionRepository.GetAll());
        }

        [HttpPost("FacturaPOST")]
        public IActionResult Save([FromBody] Factura article)
        {
            try {
                if (article == null)
                {
                    return BadRequest("Se esperaba una factura completa");
                }
           
            if (aplicacionRepository.Add(article))

                return Ok("Se añadió existosamente!");
            else
                return StatusCode(500, "No se pudo registrar la factura!");
            }
            catch (Exception )
            {
                return StatusCode(500, "Error interno, intente nuevamente!");
            }

        }

        [HttpPut("FacturaPUT")]

        public IActionResult Put([FromBody] Factura article)
        {
            if (aplicacionRepository.Edit(article))
            {
                return Ok("Se actualizó correctamente");
            }
            else 
            {
                return StatusCode(500, "No se pudo actualizar la factura!");
            }
        }


        [HttpDelete("FacturaDELETE")]

        public IActionResult Delete(int id) 
        {

            if (aplicacionRepository.Delete(id))
            {
                return Ok("Se eliminó correctamente");
            }
            else
            {
                return StatusCode(500, "No se pudo eliminar la factura!");
            }
        }

    }
}
