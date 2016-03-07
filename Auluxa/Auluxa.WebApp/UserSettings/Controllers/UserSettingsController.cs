using System.Threading.Tasks;
using System.Web.Http;
using Auluxa.WebApp.UserSettings.Models;
using Auluxa.WebApp.UserSettings.Repositories;

namespace Auluxa.WebApp.UserSettings.Controllers
{
	/// <summary>
	/// Settings Api Controller
	/// </summary>
	[RoutePrefix("api/settings")]
	public class UserSettingsController : ApiController
	{
		private readonly IUserSettingsRepository _repository;

		/// <summary>
		/// Constructor of the SettingController
		/// </summary>
		/// <param name="repository">Injected by DI</param>
		public UserSettingsController(IUserSettingsRepository repository)
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
			UserSetting setting = (await _repository.GetSettingAsync());
			return Ok(setting);
		}

		/// <summary>
		/// Create a new setting.
		/// </summary>
		/// <param name="setting"></param>
		/// <returns></returns>
		[HttpPost]
		[Route()]
		public async Task<IHttpActionResult> Post([FromBody]UserSetting setting)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			UserSetting createdSetting = await _repository.CreateSettingAsync(setting);
			return Created(Request.RequestUri, createdSetting);
		}

		/// <summary>
		/// Update an existing setting.
		/// </summary>
		/// <param name="setting"></param>
		/// <returns></returns>
		[HttpPatch]
		[Route()]
		public async Task<IHttpActionResult> Patch([FromBody]UserSetting setting)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			UserSetting updatedSetting = await _repository.UpdateSettingAsync(setting);
			return Created(Request.RequestUri, updatedSetting);
		}

	}
}
