using System.Threading.Tasks;
using System.Web.Http;
using Auluxa.WebApp.Models;
using Auluxa.WebApp.Repositories;

namespace Auluxa.WebApp.Controllers
{
	/// <summary>
	/// Settings Api Controller
	/// </summary>
	[RoutePrefix("api/settings")]
	public class SettingController : ApiController
	{
		private readonly IApplicationRepository _repository;

		/// <summary>
		/// Constructor of the SettingController
		/// </summary>
		/// <param name="repository">Injected by DI</param>
		public SettingController(IApplicationRepository repository)
		{
			_repository = repository;
		}

		/// <summary>
		/// Get settings (all or only requested)
		/// </summary>
		/// <returns></returns>
		[Route("")]
		[HttpGet]
		public async Task<IHttpActionResult> Get()
		{
			Setting setting = (await _repository.GetSettingAsync());
			return Ok(setting);
		}

		/// <summary>
		/// Create a new setting.
		/// </summary>
		/// <param name="setting"></param>
		/// <returns></returns>
		[HttpPost]
		[Route()]
		public async Task<IHttpActionResult> Post([FromBody]Setting setting)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			Setting createdSetting = await _repository.CreateSettingAsync(setting);
			return Created(Request.RequestUri, createdSetting);
		}

		/// <summary>
		/// Update an exisiting setting.
		/// </summary>
		/// <param name="setting"></param>
		/// <returns></returns>
		[HttpPatch]
		[Route()]
		public async Task<IHttpActionResult> Patch([FromBody]Setting setting)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			Setting updatedSetting = await _repository.UpdateSettingAsync(setting);
			return Created(Request.RequestUri, updatedSetting);
		}

	}
}
