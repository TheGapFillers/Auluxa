using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Auluxa.WebApp.Helpers;
using Auluxa.WebApp.Models;
using Auluxa.WebApp.Repositories;

namespace Auluxa.WebApp.Controllers
{
	/// <summary>
	/// Kraniums Api Controller
	/// </summary>
	[RoutePrefix("api/kranium")]
	public class KraniumController : ApiController
	{
		private readonly IApplicationRepository _repository;

		/// <summary>
		/// Constructor of the KraniumController
		/// </summary>
		/// <param name="repository">Injected by DI</param>
		public KraniumController(IApplicationRepository repository)
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
			Kranium kranium = (await _repository.GetKraniumAsync());
			return Ok(kranium);
		}

		/// <summary>
		/// Create a new kranium.
		/// </summary>
		/// <param name="kranium"></param>
		/// <returns></returns>
		[HttpPost]
		[Route()]
		public async Task<IHttpActionResult> Post([FromBody]Kranium kranium)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			Kranium createdKranium = await _repository.CreateKraniumAsync(kranium);
			return Created(Request.RequestUri, createdKranium);
		}

		/// <summary>
		/// Update an exisiting kranium.
		/// </summary>
		/// <param name="kranium"></param>
		/// <returns></returns>
		[HttpPatch]
		[Route()]
		public async Task<IHttpActionResult> Patch([FromBody]Kranium kranium)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			Kranium updatedKranium = await _repository.UpdateKraniumAsync(kranium);
			return Created(Request.RequestUri, updatedKranium);
		}

	}
}
