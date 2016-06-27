using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Auluxa.WebApp.Devices.Models;
using Auluxa.WebApp.Devices.Repositories;
using Auluxa.WebApp.Tools;

namespace Auluxa.WebApp.Devices.Controllers
{
    /// <summary>
    /// Devices Api Controller
    /// </summary>
    [RoutePrefix("api/models")]
    public class DeviceModelController : ApiController
    {
        private readonly IDeviceRepository _repository;

        /// <summary>
        /// Constructor of the DeviceModelController
        /// </summary>
        /// <param name="repository">Injected by DI</param>
        public DeviceModelController(IDeviceRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Get device modelss (all or only requested)
        /// </summary>
        /// <param name="ids">comma-separated ids.</param>
        /// <returns></returns>
        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> Get(string ids = null)
        {
            IEnumerable<DeviceModel> models;
            if (ids != null)
                models = await _repository.GetDeviceModelsAsync(ids.SplitAndTrim(',').Select(int.Parse).ToList());
            else
                models = await _repository.GetDeviceModelsAsync();

            return Ok(models);
        }

        /// <summary>
        /// Create a new device model.
        /// </summary>
        /// <param name="deviceModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route()]
        public async Task<IHttpActionResult> Post([FromBody]DeviceModel deviceModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            DeviceModel createdDeviceModel = await _repository.CreateDeviceModelAsync(deviceModel);
            return Created(Request.RequestUri, createdDeviceModel);
        }

        /// <summary>
        /// Update an existing device model.
        /// </summary>
        /// <param name="deviceModel"></param>
        /// <returns></returns>
        [HttpPatch]
        [Route()]
        public async Task<IHttpActionResult> Patch([FromBody]DeviceModel deviceModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            DeviceModel updatedDeviceModel = await _repository.UpdateDeviceModelAsync(deviceModel);
            return Created(Request.RequestUri, updatedDeviceModel);
        }

        /// <summary>
        /// Delete the device model of the inputted id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            DeviceModel deleteDeviceModel = await _repository.DeleteDeviceModelAsync(id);
            return Ok(deleteDeviceModel);
        }
    }
}
