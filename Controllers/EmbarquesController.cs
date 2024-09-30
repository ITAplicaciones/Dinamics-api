using Microsoft.AspNetCore.Mvc;
using DataverseAPI.Models.ProductoEmbarque;
using DataverseAPI.Helpers;



namespace DataverseAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class EmbarquesController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<EmbarquesController> _logger;
        private readonly EmbarquesHelper _myHelper;

        public EmbarquesController(IHttpClientFactory httpClientFactory, ILogger<EmbarquesController> logger, EmbarquesHelper myHelper)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _myHelper = myHelper;
        }

        [HttpPost(Name = "savelistaembarque")]
        public async Task<IActionResult> Post(List<ProductoDeEmbarque> shipmentList)
        {
            if (shipmentList == null || !ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest($"La data enviada no fue la esperada: {string.Join(", ", errors)}");
            }

            try
            {
                var tasks = shipmentList.Select(shipment => _myHelper.PostDataverseDataAsync("dc_embarques", shipment));
                await Task.WhenAll(tasks);
                return Ok(new { message = "La lista de Embarque fue guardada con exito!." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}", stackTrace = ex.StackTrace });
            }
        }
    }
}