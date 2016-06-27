using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Auluxa.WebApp.Devices.Models;
using Auluxa.WebApp.Devices.Repositories;
using Auluxa.WebApp.Scenes.Models;
using Auluxa.WebApp.Scenes.Repositories;
using Auluxa.WebApp.Tools;

namespace Auluxa.WebApp.Scenes.Controllers
{
    /// <summary>
    /// Scenes Api Controller
    /// </summary>
    [Authorize]
    [RoutePrefix("api/scenes")]
    public class SceneController : ApiController
    {
        private readonly ISceneRepository _sceneRepository;
        private readonly IDeviceRepository _deviceRepository;

        /// <summary>
        /// Constructor of the SceneController
        /// </summary>
        /// <param name="sceneRepository">Injected by DI</param>
        /// <param name="deviceRepository"></param>
        public SceneController(ISceneRepository sceneRepository, IDeviceRepository deviceRepository)
        {
            _sceneRepository = sceneRepository;
            _deviceRepository = deviceRepository;
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
            List<int> idList = ids?.SplitAndTrim(',').Select(int.Parse).ToList();
            IEnumerable<Scene> scenes;
            if (idList != null)
                scenes = await _sceneRepository.GetScenesAsync(
                    User.Identity.Name,
                    ids.SplitAndTrim(',').Select(int.Parse));
            else
                scenes = await _sceneRepository.GetScenesAsync(User.Identity.Name);

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


            Scene createdScene = await _sceneRepository.CreateSceneAsync(scene);
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

            if (scene.DeviceSettings != null)
            {
                //await BindSceneSettingsToDevices(scene);
                scene.DeviceSettings = scene.DeviceSettings;
            }

            Scene updatedScene = await _sceneRepository.UpdateSceneAsync(scene);
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
            Scene deletedScene = await _sceneRepository.DeleteSceneAsync(User.Identity.Name, id);
            return Ok(deletedScene);
        }

        private async Task BindSceneSettingsToDevices(Scene scene)
        {
            List<Device> usedDevices =
                (await _deviceRepository.GetDevicesAsync(User.Identity.Name, scene.DeviceSettings.Select(s => s.Device.Id))).ToList();

            foreach (DeviceSetting setting in scene.DeviceSettings)
            {
                setting.Device = usedDevices.SingleOrDefault(a => a.Id == setting.Device.Id);
                if (!setting.IsValid())
                {
                    throw new Exception($"Invalid setting for device {setting.Device.Id}");
                }
            }
        }
    }
}
