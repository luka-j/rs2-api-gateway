﻿using Configurator.CCOEntities;
using Configurator.DTOs;
using Configurator.Entities;
using Configurator.Services;
using Microsoft.AspNetCore.Mvc;

namespace Configurator.Controllers
{
    [ApiController]
    [Route("cco")]
    public class CCOConfigController : ControllerBase
    {
        private readonly CCOService _ccoService;
        private readonly ConfiguratorService _configuratorService;

        public CCOConfigController(CCOService ccoService, ConfiguratorService configuratorService)
        {
            _ccoService = ccoService ?? throw new ArgumentNullException(nameof(ccoService));
            _configuratorService = configuratorService ?? throw new ArgumentNullException(nameof(configuratorService));
        }

        [HttpGet]
        [ProducesResponseType(typeof(CCOConfigListDTO), StatusCodes.Status200OK)]
        public async Task<ActionResult<CCOConfigListDTO>> GetConfigs()
        {
            var databases = await _ccoService.GetAllDatabases();
            var caches = await _ccoService.GetAllCaches();
            var queues = await _ccoService.GetAllQueues();

            return Ok(new CCOConfigListDTO(databases, caches, queues));
        }

        [HttpGet("databases")]
        [ProducesResponseType(typeof(IEnumerable<DatabaseSpec>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<DatabaseSpec>>> GetDatabaseConfigs()
        {
            return Ok(await _ccoService.GetAllDatabases());
        }

        [HttpGet("databases/{apiName}")]
        [ProducesResponseType(typeof(DatabaseSpec), StatusCodes.Status200OK)]
        public async Task<ActionResult<DatabaseSpec>> GetDatabaseConfig(string apiName)
        {
            try
            {
                return Ok(await _ccoService.GetDatabase(apiName));
            }
            catch { return Ok(); }
        }

        [HttpPost("databases")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> AddDatabase([FromBody] DatabaseSpec data)
        {
            try
            {
                string configData = CCOService.GetDatabaseString(data);
                Config config = new("datasources", data.Title, "", "databases", configData);

                await _configuratorService.ModifyAndUpdate(new[] { config });
                return Ok(true);
            }
            catch { return Ok(false); }
        }

        [HttpDelete("databases/{apiName}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> DeleteDatabaseConfig(string apiName)
        {
            try
            {
                var config = new Config("datasources", apiName, "", "databases", "");
                await _configuratorService.DeleteConfigs(new[] { config });
                return Ok(true);
            }
            catch { return Ok(false); }
        }
        [HttpGet("caches")]
        [ProducesResponseType(typeof(IEnumerable<CacheSpec>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CacheSpec>>> GetCacheConfigs()
        {
            return Ok(await _ccoService.GetAllCaches());
        }

        [HttpGet("caches/{apiName}")]
        [ProducesResponseType(typeof(CacheSpec), StatusCodes.Status200OK)]
        public async Task<ActionResult<CacheSpec>> GetCacheConfig(string apiName)
        {
            try
            {
                return Ok(await _ccoService.GetCache(apiName));
            }
            catch { return Ok(); }
        }

        [HttpPost("caches")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> AddCache([FromBody] CacheSpec data)
        {
            try
            {
                string configData = CCOService.GetCacheSpec(data);
                Config config = new("datasources", data.Title, "", "caches", configData);

                await _configuratorService.ModifyAndUpdate(new[] { config });
                return Ok(true);
            }
            catch { return Ok(false); }
        }

        [HttpDelete("caches/{apiName}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> DeleteCacheConfig(string apiName)
        {
            try
            {
                var config = new Config("datasources", apiName, "", "caches", "");
                await _configuratorService.DeleteConfigs(new[] { config });
                return Ok(true);
            }
            catch { return Ok(false); }
        }
        [HttpGet("queues")]
        [ProducesResponseType(typeof(IEnumerable<QueueSpec>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<QueueSpec>>> GetQueueConfigs()
        {
            return Ok(await _ccoService.GetAllQueues());
        }

        [HttpGet("queues/{apiName}")]
        [ProducesResponseType(typeof(QueueSpec), StatusCodes.Status200OK)]
        public async Task<ActionResult<QueueSpec>> GetQueueConfig(string apiName)
        {
            try
            {
                return Ok(await _ccoService.GetQueue(apiName));
            }
            catch { return Ok(); }
        }

        [HttpPost("queues")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> AddQueue([FromBody] QueueSpec data)
        {
            try
            {
                string configData = CCOService.GetQueueSpec(data);
                Config config = new("datasources", data.Title, "", "queues", configData);

                await _configuratorService.ModifyAndUpdate(new[] { config });
                return Ok(true);
            }
            catch { return Ok(false); }
        }

        [HttpDelete("queues/{apiName}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> DeleteQueueConfig(string apiName)
        {
            try
            {
                var config = new Config("datasources", apiName, "", "queues", "");
                await _configuratorService.DeleteConfigs(new[] { config });
                return Ok(true);
            }
            catch { return Ok(false); }
        }
    }
}
