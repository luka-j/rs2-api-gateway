﻿using CCO.CCOConfigs;
using CCO.Repositories;
using Grpc.Core;

namespace CCO.Services
{
    public class DatasourceOperationsService : DatasourceOperations.DatasourceOperationsBase
    {
        private readonly CCORepository _repository;
        private readonly DatabaseRepository _databaseRepository;
        private readonly CacheRepository _cacheRepository;

        public DatasourceOperationsService(CCORepository repository, DatabaseRepository databaseRepository, CacheRepository cacheRepository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _databaseRepository = databaseRepository ?? throw new ArgumentNullException(nameof(databaseRepository));
            _cacheRepository = cacheRepository ?? throw new ArgumentNullException(nameof(cacheRepository));
        }

        public override async Task<DatabaseReadResponse> DatabaseRead (DatabaseReadRequest request, ServerCallContext context)
        {
            var config = getConfig(request.Identifier);
            var database = config.Data.Databases.FirstOrDefault() ?? throw new Exception("Database not found");

            var entries = await _databaseRepository.ReadAll(database);
            var response = new DatabaseReadResponse();
            response.Items.AddRange(entries.Select((entry) => new DatabaseItem { Id = entry.Id, Amount = entry.Amount }));

            return response;
        }

        public override async Task<DatabaseWriteResponse> DatabaseWrite (DatabaseWriteRequest request, ServerCallContext context) 
        {
            var config = getConfig(request.Identifier);
            var database = config.Data.Databases.FirstOrDefault() ?? throw new Exception("Database not found");

            var success = await _databaseRepository.Create(database, request.Amount);

            return new DatabaseWriteResponse { Success = success };
        }

        public override async Task<DatabaseDeleteResponse> DatabaseDelete(DatabaseDeleteRequest request, ServerCallContext context)
        {
            var config = getConfig(request.Identifier);
            var database = config.Data.Databases.FirstOrDefault() ?? throw new Exception("Database not found");

            var success = await _databaseRepository.Delete(database, request.Id);

            return new DatabaseDeleteResponse { Success = success };
        }

        public override async Task<CacheReadResponse> CacheRead (CacheReadRequest request, ServerCallContext context)
        {
            var config = getConfig(request.Identifier);
            var cache = config.Data.Caches.FirstOrDefault() ?? throw new Exception("Cache not found");

            string value = await _cacheRepository.GetAsync(cache, request.Key);
            
            return new CacheReadResponse { Value = value };
        }

        public override async Task<CacheWriteResponse> CacheWrite (CacheWriteRequest request, ServerCallContext context)
        {
            var config = getConfig(request.Identifier);
            var cache = config.Data.Caches.FirstOrDefault() ?? throw new Exception("Cache not found");

            var success = await _cacheRepository.SetAsync(cache, request.Key, request.Value, TimeSpan.Parse(request.Ttl));

            return new CacheWriteResponse { Success = success };
        }

        public override Task<QueueReadResponse> QueueRead (QueueReadRequest request, ServerCallContext context) 
        {
            throw new NotImplementedException();
        }

        public override Task<QueueWriteResponse> QueueWrite (QueueWriteRequest request, ServerCallContext context)
        {
            throw new NotImplementedException();
        }

        private CCOConfig getConfig (ConfigIdentifier id)
        {
            CCOIdentifier identifier = new CCOIdentifier(id.CcoName, id.CcoVersion);
            var config = _repository.GetCurrentConfig(identifier, DateTime.Now);

            if (config == null)
            {
                throw new Exception("CCO " + id.CcoName + "/" + id.CcoVersion + " not found");
            }

            return config;
        }
    }
}
