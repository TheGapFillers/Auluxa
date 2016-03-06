using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Auluxa.WebApp.Appliances.Models;
using Auluxa.WebApp.Appliances.Repositories;
using Auluxa.WebApp.Helpers;
using Auluxa.WebApp.Scenes.Models;
using Auluxa.WebApp.Scenes.Repositories;

namespace Auluxa.WebApp.Scenes.Controllers
{
    /// <summary>
    /// Scenes Api Controller
    /// </summary>
    [RoutePrefix("api/scenes")]
    public class SceneController : ApiController
    {
        private readonly ISceneRepository _repository;
        private readonly IApplianceRepository _applianceRepository;

        /// <summary>
        /// Constructor of the SceneController
        /// </summary>
        /// <param name="respository">Injected by DI</param>
        public SceneController(ISceneRepository respository, IApplianceRepository applianceRepository)
        {
            _repository = respository;
            _applianceRepository = applianceRepository;
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

            await BindSceneSettingsToAppliances(scene);

            Scene createdScene = await _repository.CreateSceneAsync(scene);
            return Created(Request.RequestUri, createdScene);
        }

        /// <summary>
        /// Update an existing scene.
        /// </summary>
        /// <param name="scene"></param>
        /// <returns></returns>
        [HttpPatch]
        [Route()]
        public async Task<IHttpActionResult> Patch([FromBody]Scene scene)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (scene.ApplianceSettings != null)
            {
                await BindSceneSettingsToAppliances(scene);
                scene.ApplianceSettings = scene.ApplianceSettings;
            }

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

        private async Task BindSceneSettingsToAppliances(Scene scene)
        {
            List<Appliance> usedAppliances =
                (await _applianceRepository.GetAppliancesAsync(scene.ApplianceSettings.Select(s => s.Appliance.Id))).ToList();

            foreach (ApplianceSetting setting in scene.ApplianceSettings)
            {
                setting.Appliance = usedAppliances.SingleOrDefault(a => a.Id == setting.Appliance.Id);
                if (!setting.IsValid())
                {
                    throw new Exception($"Invalid setting for appliance {setting.Appliance.Id}");
                }
            }
        }
    }
}
