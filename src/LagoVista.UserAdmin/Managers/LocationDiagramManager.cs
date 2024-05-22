﻿using LagoVista.Core.Interfaces;
using LagoVista.Core.Managers;
using LagoVista.Core.Models;
using LagoVista.Core.Models.UIMetaData;
using LagoVista.Core.PlatformSupport;
using LagoVista.Core.Validation;
using LagoVista.UserAdmin.Interfaces.Managers;
using LagoVista.UserAdmin.Interfaces.Repos.Orgs;
using LagoVista.UserAdmin.Models.Orgs;
using System;
using System.Threading.Tasks;

namespace LagoVista.UserAdmin.Managers
{
    public class LocationDiagramManager : ManagerBase, ILocationDiagramManager
    {
        private readonly ILocationDiagramRepo _locationDiagramRepo;

        public LocationDiagramManager(ILocationDiagramRepo locationDiagramRepo, ILogger logger, IAppConfig appConfig, IDependencyManager dependencyManager, ISecurity security) : base(logger, appConfig, dependencyManager, security)
        {
            _locationDiagramRepo = locationDiagramRepo ?? throw new ArgumentNullException(nameof(locationDiagramRepo));
        }

        public async Task<InvokeResult> AddLocationDiagramAsync(LocationDiagram diagramLocation, EntityHeader org, EntityHeader user)
        {
            ValidationCheck(diagramLocation, Actions.Create);
            await AuthorizeAsync(diagramLocation, AuthorizeResult.AuthorizeActions.Create, user, org);

            await _locationDiagramRepo.AddLocationDiagramAsync(diagramLocation);

            return InvokeResult.Success;
        }

        public async Task<LocationDiagram> GetLocationDiagramAsync(string id, EntityHeader org, EntityHeader user)
        {
            var diagram = await _locationDiagramRepo.GetLocationDiagramAsync(id);
            await AuthorizeAsync(diagram, AuthorizeResult.AuthorizeActions.Read, user, org);

            return diagram;
        }

        public async Task<ListResponse<LocationDiagramSummary>> GetLocationDiagramsAsync(ListRequest listRequest, EntityHeader org, EntityHeader user )
        {
            await AuthorizeOrgAccessAsync(user, org, typeof(LocationDiagram));
            return await _locationDiagramRepo.GetLocationDiagramsAsync(org.Id, listRequest);
        }

        public async Task<InvokeResult> UpdateLocationDiagramAsync(LocationDiagram diagramLocation, EntityHeader org, EntityHeader user)
        {
            ValidationCheck(diagramLocation, Actions.Create);
            await AuthorizeAsync(diagramLocation, AuthorizeResult.AuthorizeActions.Update, user, org);

            await _locationDiagramRepo.UpdateLocationDiagramAsync(diagramLocation);

            return InvokeResult.Success;
        }
    }
}
