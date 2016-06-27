using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Auluxa.WebApp.Tools;
using Auluxa.WebApp.Zones.Models;
using Auluxa.WebApp.Zones.Repositories;

namespace Auluxa.WebApp.Zones.Controllers
{
    /// <summary>
    /// Zones Api Controller
    /// </summary>
    [Authorize]
    [RoutePrefix("api/zones")]
    public class ZoneController : ApiController
    {
        private readonly IZoneRepository _repository;

        /// <summary>
        /// Constructor of the ZoneController
        /// </summary>
        /// <param name="repository">Injected by DI</param>
        public ZoneController(IZoneRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Get zones (all or only requested)
        /// </summary>
        /// <param name="ids">comma-separated ids.</param>
        /// <returns></returns>
        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> Get(string ids = null)
        {
            List<int> idList = ids?.SplitAndTrim(',').Select(int.Parse).ToList();
            IEnumerable<Zone> zones;
            if (idList != null)
                zones = await _repository.GetZonesAsync(
                    User.Identity.Name,
                    ids.SplitAndTrim(',').Select(int.Parse));
            else
                zones = await _repository.GetZonesAsync(User.Identity.Name);

            return Ok(zones);
        }

        /// <summary>
        /// Create a new zone.
        /// </summary>
        /// <param name="zone"></param>
        /// <returns></returns>
        [HttpPost]
        [Route]
        public async Task<IHttpActionResult> Post([FromBody]CreateZoneViewModel zone)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Zone createdZone = await _repository.CreateZoneAsync(User.Identity.Name, zone.Name);

            return Created(Request.RequestUri, createdZone);
        }

        /// <summary>
        /// Update an exisiting zone.
        /// </summary>
        /// <param name="zone"></param>
        /// <returns></returns>
        [HttpPatch]
        [Route()]
        public async Task<IHttpActionResult> Patch([FromBody]UpdateZoneViewModel zone)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Zone updatedZone = await _repository.UpdateZoneAsync(User.Identity.Name, zone.zoneId, zone.Name);

            return Created(Request.RequestUri, updatedZone);
        }

        /// <summary>
        /// Delete the zone of the inputted id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            Zone deletedZone = await _repository.DeleteZoneAsync(User.Identity.Name, id);
            return Ok(deletedZone);
        }
    }
}
