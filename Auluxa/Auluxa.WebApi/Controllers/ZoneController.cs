using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Auluxa.Models;
using Auluxa.Repositories;
using Auluxa.WebApi.Helpers;

namespace Auluxa.WebApi.Controllers
{
	/// <summary>
	/// Zones Api Controller
	/// </summary>
	[RoutePrefix("api/zones")]
	public class ZoneController : ApiController
	{
		private readonly IApplicationRepository _repository;

		/// <summary>
		/// Constructor of the ZoneController
		/// </summary>
		/// <param name="repository">Injected by DI</param>
		public ZoneController(IApplicationRepository repository)
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
			List<int> idList = null;
			if (ids != null)
			{
				idList = ids.SplitAndTrim(',').Select(int.Parse).ToList(); 
			}

			List<Zone> zones = (await _repository.GetZonesAsync(idList)).ToList();
			return Ok(zones);
		}

		/// <summary>
		/// Upsert a new zone.
		/// </summary>
		/// <param name="zone"></param>
		/// <returns></returns>
		[HttpPost]
		[Route()]
		public async Task<IHttpActionResult> Post([FromBody]Zone zone)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			Zone createdZone = await _repository.UpsertZoneAsync(zone);
			return Created(Request.RequestUri, createdZone);
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
