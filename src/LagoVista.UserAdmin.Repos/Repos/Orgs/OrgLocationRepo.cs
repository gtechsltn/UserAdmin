﻿using LagoVista.CloudStorage.DocumentDB;

using System;
using System.Threading.Tasks;
using LagoVista.Core.PlatformSupport;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.Documents;
using LagoVista.UserAdmin.Models.Orgs;
using LagoVista.UserAdmin.Interfaces.Repos.Orgs;
using LagoVista.IoT.Logging.Loggers;
using Microsoft.Azure.Cosmos;

namespace LagoVista.UserAdmin.Repos.Orgs
{
    public class OrgLocationRepo : DocumentDBRepoBase<OrgLocation>, IOrgLocationRepo
    {
        bool _shouldConsolidateCollections;

        public OrgLocationRepo(IUserAdminSettings userAdminSettings, IAdminLogger logger) :
            base(userAdminSettings.UserStorage.Uri, userAdminSettings.UserStorage.AccessKey, userAdminSettings.UserStorage.ResourceName, logger)
        {
            _shouldConsolidateCollections = userAdminSettings.ShouldConsolidateCollections;
        }

        protected override bool ShouldConsolidateCollections
        {
            get { return _shouldConsolidateCollections; }
        }

        public Task AddLocationAsync(OrgLocation orgLocation)
        {
            return CreateDocumentAsync(orgLocation);
        }

        public Task UpdateLocationAsync(OrgLocation org)
        {
            return UpsertDocumentAsync(org);
        }

        public Task<OrgLocation> GetLocationAsync(String id)
        {
            return GetDocumentAsync(id);
        }

        public Task<IEnumerable<OrgLocation>> GetOrganizationLocationAsync(String orgId)
        {
            return QueryAsync(act => act.Organization.Id == orgId);
        }

        public async Task<bool> QueryNamespaceInUseAsync(string orgId, string namespaceText)
        {
            try
            {
                var organization = (await QueryAsync(loc => loc.Namespace == namespaceText && loc.Organization.Id == orgId));
                return organization.ToList().Any();
            }
            catch(CosmosException)
            {
                /* If the collection doesn't exist, it will throw this exception */
                return false;
            }
        }
    }
}
