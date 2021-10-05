﻿using System.Threading.Tasks;
using LagoVista.Core.Models;
using LagoVista.Core.Models.UIMetaData;
using LagoVista.UserAdmin.Models;
using LagoVista.UserAdmin.Models.Orgs;

namespace LagoVista.UserAdmin.Interfaces.Repos.Orgs
{
    public interface IOrganizationRepo
    {
        Task AddOrganizationAsync(Organization org);
        Task<Organization> GetOrganizationAsync(string orgId);
        Task UpdateOrganizationAsync(Organization org);
        Task<bool> QueryOrganizationExistAsync(string orgId);
        Task<bool> QueryNamespaceInUseAsync(string namespaceText);
        Task DeleteOrgAsync(string orgId);
        Task<bool> HasBillingRecords(string orgId);
        Task<ListResponse<Organization>> GetAllOrgsAsync(ListRequest listRequest);
    }
}