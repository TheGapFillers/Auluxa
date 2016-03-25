using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Auluxa.WebApp.Appliances.Models;
using Auluxa.WebApp.Appliances.Repositories;
using Auluxa.WebApp.Helpers;
using Auluxa.WebApp.Zones.Repositories;
using Auluxa.WebApp.Zones.Models;

namespace Auluxa.WebApp.Appliances.Controllers
{
	/// <summary>
	/// Appliances Api Controller
	/// </summary>
	[RoutePrefix("api/appliances")]
	public class ApplianceController : ApiController
	{
		private readonly IApplianceRepository _repository;
		//private readonly IZoneRepository _zoneRepository;

		/// <summary>
		/// Constructor of the ApplianceController
		/// </summary>
		/// <param name="repository">Injected by DI</param>
		public ApplianceController(IApplianceRepository repository/*, IZoneRepository zoneRepository*/)
		{
			_repository = repository;
			//_zoneRepository = zoneRepository;
		}

		/// <summary>
		/// Get appliances (all or only requested)
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

			List<Appliance> appliances = (await _repository.GetAppliancesAsync(idList)).ToList();
			return Ok(appliances);
		}

		/// <summary>
		/// Create a new appliance.
		/// </summary>
		/// <param name="appliance"></param>
		/// <returns></returns>
		[HttpPost]
		[Route()]
		public async Task<IHttpActionResult> Post([FromBody]Appliance appliance)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			Appliance createdAppliance = await _repository.CreateApplianceAsync(appliance);
			return Created(Request.RequestUri, createdAppliance);
		}

		/// <summary>
		/// Update an existing appliance.
		/// </summary>
		/// <param name="appliance"></param>
		/// <returns></returns>
		[HttpPatch]
		[Route()]
		public async Task<IHttpActionResult> Patch([FromBody]Appliance appliance)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			//if (appliance.Zone != null)
			//{
			//	Zone usedZone = (await _zoneRepository.GetZonesAsync(new List<int> { appliance.Zone.Id })).First();
			//	appliance.Zone = usedZone;
			//}

			Appliance updatedAppliance = await _repository.UpdateApplianceAsync(appliance);

			return Created(Request.RequestUri, updatedAppliance);
		}

		/// <summary>
		/// Delete the appliance of the inputted id.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpDelete]
		[Route("{id}")]
		public async Task<IHttpActionResult> Delete(int id)
		{
			Appliance deletedAppliance = await _repository.DeleteApplianceAsync(id);
			return Ok(deletedAppliance);
		}
	}
}
