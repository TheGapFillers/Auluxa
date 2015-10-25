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
    /// Scenes Api Controller
    /// </summary>
    [RoutePrefix("api/scenes")]
    public class SceneController : ApiController
    {
        private readonly IApplicationRepository _repository;

        /// <summary>
        /// Constructor of the SceneController
        /// </summary>
        /// <param name="respository">Injected by DI</param>
        public SceneController(IApplicationRepository respository)
        {
            _repository = respository;
        }

        /// <summary>
        /// Get scenes (all or only requested)
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

            List<Scene> scenes = (await _repository.GetScenesAsync(idList)).ToList();
            return Ok(scenes);
        }

        /// <summary>
        /// Create a new scene.
        /// </summary>
        /// <param name="scene"></param>
        /// <returns></returns>
        [HttpPost]
        [Route()]
        public async Task<IHttpActionResult> Post([FromBody]Scene scene)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Scene createdScene = await _repository.CreateSceneAsync(scene);
            return Created(Request.RequestUri, createdScene);
        }

        /// <summary>
        /// Update an exisiting scene.
        /// </summary>
        /// <param name="scene"></param>
        /// <returns></returns>
        [HttpPatch]
        [Route()]
        public async Task<IHttpActionResult> Patch([FromBody]Scene scene)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Scene updatedScene = await _repository.UpdateSceneAsync(scene);
            return Created(Request.RequestUri, updatedScene);
        }

        /// <summary>
        /// Delete the scene of the inputted id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            Scene deletedScene = await _repository.DeleteSceneAsync(id);
            return Ok(deletedScene);
        }
    }
}
