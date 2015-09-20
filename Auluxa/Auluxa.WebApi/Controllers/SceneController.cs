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
        private readonly IApplicationRepository _sceneRepository;

        /// <summary>
        /// Constructor of the SceneController
        /// </summary>
        /// <param name="sceneRespository">Injected by DI</param>
        public SceneController(IApplicationRepository sceneRespository)
        {
            _sceneRepository = sceneRespository;
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
            {
                idList = ids.SplitAndTrim(',').Select(int.Parse).ToList(); 
            }

            List<Scene> scenes = (await _sceneRepository.GetScenesAsync(idList)).ToList();
            return Ok(scenes);
        }

        /// <summary>
        /// Upsert a new scene.
        /// </summary>
        /// <param name="scene"></param>
        /// <returns></returns>
        [HttpPost]
        [Route()]
        public async Task<IHttpActionResult> Post([FromBody]Scene scene)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Scene createdScene = await _sceneRepository.UpsertSceneAsync(scene);
            return Created(Request.RequestUri, createdScene);
        }

        /// <summary>
        /// Delete the scene of the inputted id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route()]
        public async Task<IHttpActionResult> Delete(int id)
        {
            Scene deletedScene = await _sceneRepository.DeleteSceneAsync(id);
            return Ok(deletedScene);
        }
    }
}
