using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Auluxa.WebApp.Devices.Models;
using Auluxa.WebApp.Devices.Repositories;
using Auluxa.WebApp.Tools;
using Auluxa.WebApp.Zones.Models;
using Auluxa.WebApp.Zones.Repositories;

namespace Auluxa.WebApp.Devices.Controllers
{
    /// <summary>
    /// Devices Api Controller
    /// </summary>
    [Authorize]
    [RoutePrefix("api/devices")]
    public class DeviceController : ApiController
    {
        private readonly IDeviceRepository _repository;
        private readonly IZoneRepository _zoneRepository;

        /// <summary>
        /// Constructor of the DeviceController
        /// </summary>
        /// <param name="deviceRepository">Injected by DI</param>
        /// <param name="zoneRepository">Injected by DI</param>
        public DeviceController(IDeviceRepository deviceRepository, IZoneRepository zoneRepository)
        {
            _repository = deviceRepository;
            _zoneRepository = zoneRepository;
        }

        /// <summary>
        /// Get devices (all or only requested)
        /// </summary>
        /// <param name="ids">comma-separated ids. If not specified, will return all available devices.</param>
        /// <returns></returns>
        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> Get(string ids = null)
        {
            List<int> idList = ids?.SplitAndTrim(',').Select(int.Parse).ToList();
            IEnumerable<Device> devices;
            if (idList != null)
                devices = await _repository.GetDevicesAsync(
                    User.Identity.Name,
                    ids.SplitAndTrim(',').Select(int.Parse));
            else
                devices = await _repository.GetDevicesAsync(User.Identity.Name);

            return Ok(devices);
        }

        /// <summary>
        /// Create a new device.
        /// </summary>
        /// <param name="deviceVm">The device view model to create the device</param>
        /// <returns></returns>
        [HttpPost]
        [Route]
        public async Task<IHttpActionResult> Post([FromBody]CreateDeviceViewModel deviceVm)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Device createdDevice = await _repository.CreateDeviceAsync(User.Identity.Name, deviceVm.DeviceModelId);

            return Created(Request.RequestUri, createdDevice);
        }

        /// <summary>
        /// Update the current settings of a device
        /// </summary>
        /// <param name="deviceSettingsVm">Specify the device and the device settings dictionary</param>
        /// <returns></returns>
        [HttpPut]
        [Route("settings")]
        public async Task<IHttpActionResult> UpdateDeviceSettings([FromBody]UpdateDeviceSettingsViewModel deviceSettingsVm)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Device updatedDevice = await _repository.UpdateDeviceSettingsAsync(User.Identity.Name, deviceSettingsVm.DeviceId, deviceSettingsVm.Settings);

            return Ok(updatedDevice);
        }

        /// <summary>
        /// Update the zones of the device
        /// </summary>
        /// <param name="deviceZonesVm">Specify the device and the device zones ids (comma separated)</param>
        /// <returns></returns>
        [HttpPut]
        [Route("zones")]
        public async Task<IHttpActionResult> UpdateDeviceZones([FromBody]UpdateDeviceZonesViewModel deviceZonesVm)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            List<int> idList = deviceZonesVm.zoneIds?.SplitAndTrim(',').Select(int.Parse).ToList();
            IEnumerable<Zone> zones = await _zoneRepository.GetZonesAsync(User.Identity.Name, idList);

            Device updatedDevice = await _repository.UpdateDeviceZonesAsync(User.Identity.Name, deviceZonesVm.DeviceId, zones);

            return Ok(updatedDevice);
        }

        /// <summary>
        /// Delete the device of the inputted id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            Device deletedDevice = await _repository.DeleteDeviceAsync(User.Identity.Name, id);
            return Ok(deletedDevice);
        }
    }
}
