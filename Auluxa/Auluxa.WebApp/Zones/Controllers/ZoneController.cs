using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Auluxa.WebApp.Appliances.Models;
using Auluxa.WebApp.Appliances.Repositories;
using Auluxa.WebApp.Helpers;
using Auluxa.WebApp.Zones.Models;
using Auluxa.WebApp.Zones.Repositories;

namespace Auluxa.WebApp.Zones.Controllers
{
    /// <summary>
    /// Zones Api Controller
    /// </summary>
    [RoutePrefix("api/zones")]
    public class ZoneController : ApiController
    {
        private readonly IZoneRepository _repository;
        private readonly IApplianceRepository _applianceRepository;

        /// <summary>
        /// Constructor of the ZoneController
        /// </summary>
        /// <param name="repository">Injected by DI</param>
        public ZoneController(IZoneRepository repository, IApplianceRepository applianceRepository)
        {
            _repository = repository;
            _applianceRepository = applianceRepository;
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
            List<int> idList = null;
            if (ids != null)
                idList = ids.SplitAndTrim(',').Select(int.Parse).ToList();

            List<Zone> zones = (await _repository.GetZonesAsync(idList)).ToList();
            return Ok(zones);
        }

        /// <summary>
        /// Create a new zone.
        /// </summary>
        /// <param name="zone"></param>
        /// <returns></returns>
        [HttpPost]
        [Route()]
        public async Task<IHttpActionResult> Post([FromBody]Zone zone)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            List<Appliance> usedAppliances =
                (await _applianceRepository.GetAppliancesAsync(zone.Appliances.Select(z => z.Id))).ToList();
            zone.Appliances = usedAppliances;

            Zone createdZone = await _repository.CreateZoneAsync(zone);
            return Created(Request.RequestUri, createdZone);
        }

        /// <summary>
        /// Update an exisiting zone.
        /// </summary>
        /// <param name="zone"></param>
        /// <returns></returns>
        [HttpPatch]
        [Route()]
        public async Task<IHttpActionResult> Patch([FromBody]Zone zone)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (zone.Appliances != null)
            {
                List<Appliance> usedAppliances =
                    (await _applianceRepository.GetAppliancesAsync(zone.Appliances.Select(a => a.Id))).ToList();
                zone.Appliances = usedAppliances;
            }

            Zone updatedZone = await _repository.UpdateZoneAsync(zone);
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
            Zone deletedZone = await _repository.DeleteZoneAsync(id);
            return Ok(deletedZone);
        }
    }
}
