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
	/// Appliances Api Controller
	/// </summary>
	[RoutePrefix("api/models")]
	public class ApplianceModelController : ApiController
	{
		private readonly IApplicationRepository _repository;

		/// <summary>
		/// Constructor of the ApplianceModelController
		/// </summary>
		/// <param name="repository">Injected by DI</param>
		public ApplianceModelController(IApplicationRepository repository)
		{
			_repository = repository;
		}

		/// <summary>
		/// Get appliance modelss (all or only requested)
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

			List<ApplianceModel> applianceModels = (await _repository.GetApplianceModelsAsync(idList)).ToList();
			return Ok(applianceModels);
		}

		/// <summary>
		/// Create a new appliance model.
		/// </summary>
		/// <param name="applianceModel"></param>
		/// <returns></returns>
		[HttpPost]
		[Route()]
		public async Task<IHttpActionResult> Post([FromBody]ApplianceModel applianceModel)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			ApplianceModel createdApplianceModel = await _repository.CreateApplianceModelAsync(applianceModel);
			return Created(Request.RequestUri, createdApplianceModel);
		}

		/// <summary>
		/// Update an existing appliance model.
		/// </summary>
		/// <param name="applianceModel"></param>
		/// <returns></returns>
		[HttpPatch]
		[Route()]
		public async Task<IHttpActionResult> Patch([FromBody]ApplianceModel applianceModel)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			ApplianceModel updatedApplianceModel = await _repository.UpdateApplianceModelAsync(applianceModel);
			return Created(Request.RequestUri, updatedApplianceModel);
		}

		/// <summary>
		/// Delete the appliance model of the inputted id.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpDelete]
		[Route("{id}")]
		public async Task<IHttpActionResult> Delete(int id)
		{
			ApplianceModel deleteApplianceModel = await _repository.DeleteApplianceModelAsync(id);
			return Ok(deleteApplianceModel);
		}
	}
}
