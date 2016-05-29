using System.Threading.Tasks;
using System.Web.Http;
using Auluxa.WebApp.Kranium.Models;
using Auluxa.WebApp.Kranium.Repositories;

namespace Auluxa.WebApp.Kranium.Controllers
{
	/// <summary>
	/// Kraniums Api Controller
	/// </summary>
	[RoutePrefix("api/kranium")]
	public class KraniumController : ApiController
	{
		private readonly IKraniumRepository _repository;

		/// <summary>
		/// Constructor of the KraniumController
		/// </summary>
		/// <param name="repository">Injected by DI</param>
		public KraniumController(IKraniumRepository repository)
		{
			_repository = repository;
		}

		/// <summary>
		/// Get kraniums (all or only requested)
		/// </summary>
		/// <returns></returns>
		[Route("")]
		[HttpGet]
		public async Task<IHttpActionResult> Get()
		{
			KraniumEntity kranium = (await _repository.GetKraniumAsync());
			return Ok(kranium);
		}

		/// <summary>
		/// Create a new kranium.
		/// </summary>
		/// <param name="kranium"></param>
		/// <returns></returns>
		[HttpPost]
		[Route()]
		public async Task<IHttpActionResult> Post([FromBody]KraniumEntity kranium)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			KraniumEntity createdKranium = await _repository.CreateKraniumAsync(kranium);
			return Created(Request.RequestUri, createdKranium);
		}

		/// <summary>
		/// Update an exisiting kranium.
		/// </summary>
		/// <param name="kranium"></param>
		/// <returns></returns>
		[HttpPatch]
		[Route()]
		public async Task<IHttpActionResult> Patch([FromBody]KraniumEntity kranium)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			KraniumEntity updatedKranium = await _repository.UpdateKraniumAsync(kranium);
			return Created(Request.RequestUri, updatedKranium);
		}

	}
}
