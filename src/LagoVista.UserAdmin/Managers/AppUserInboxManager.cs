﻿using LagoVista.Core;
using LagoVista.Core.Models;
using LagoVista.Core.Models.UIMetaData;
using LagoVista.Core.Validation;
using LagoVista.UserAdmin.Interfaces;
using LagoVista.UserAdmin.Models.Users;
using LagoVista.UserAdmin.Repos.Repos.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LagoVista.UserAdmin.Models.Apps
{
    public class AppUserInboxManager : IAppUserInboxManager 
    {
        private readonly IAppUserInboxRepo _inboxRepo;
        private readonly ISystemNotificationsRepo _systemNotificationRepo;

        public AppUserInboxManager(IAppUserInboxRepo inboxRepo, ISystemNotificationsRepo systemNotificationRepo)
        {
            this._inboxRepo = inboxRepo ?? throw new ArgumentNullException(nameof(inboxRepo));
            this._systemNotificationRepo = systemNotificationRepo ?? throw new ArgumentNullException(nameof(systemNotificationRepo));   
        }

        public async Task<InvokeResult> CreateInboxItemAsync(NewAppUserInboxItemItem item)
        {
            await _inboxRepo.AddInboxItemAsync(item.CreateInboxItem());
            return InvokeResult.Success;
        }

        public async Task<InvokeResult> MarkAsReadAsync(string partitionKey, string rowKey, EntityHeader org, EntityHeader user)
        {
            var item = await this._inboxRepo.GetItemInboxItemAsync(partitionKey, rowKey);
            if (!item.Viewed)
            {
                item.Viewed = true;
                item.ViewedTimeStamp = DateTime.UtcNow.ToJSONString();
                await this._inboxRepo.UpdateInboxItemAsync(item);
            }

            return InvokeResult.Success;
        }

        public async Task<InvokeResult> DeleteItemAsync(string partitionKey, string rowKey, EntityHeader org, EntityHeader user)
        {
            await this._inboxRepo.DeleteItemAsync(partitionKey, rowKey);
            return InvokeResult.Success;
        }


        public async Task<InvokeResult<int>> GetAllInboxItemCountAsync(EntityHeader org, EntityHeader user)
        {
            var bldr = TimingBuilder.StartNew();

            var myItems = await _inboxRepo.GetInboxItemsAsync(org.Id, user.Id, ListRequest.Create());
            bldr.CompleteAndRestart("[AppUserInboxManager__GetAllInboxItemCountAsync__GetInboxItemsAsync]");
            var allItems = new List<InboxItem>();
            allItems.AddRange(myItems.Model.Where(mod=>mod.Viewed == false).Select(mod => mod.ToInboxItem()));

            var sysItems = await _systemNotificationRepo.GetSystemAndPublicNotifications(org.Id);
            bldr.CompleteAndRestart("[AppUserInboxManager__GetAllInboxItemCountAsync__GetSystemAndPublicNotifications]");
            allItems.AddRange(sysItems.Select(sys => sys.ToInboxItem()));
            return InvokeResult<int>.Create(allItems.Count, bldr);
        }


        public async Task<ListResponse<InboxItem>> GetAllInboxItemsAsync(EntityHeader org, EntityHeader user, ListRequest listRequest)
        {
            var bldr = TimingBuilder.StartNew();

            var myItems = await _inboxRepo.GetInboxItemsAsync(org.Id, user.Id, listRequest);
            bldr.CompleteAndRestart("[AppUserInboxManager__GetAllInboxItemsAsync__GetAllInboxItemsAsync]");

            var allItems = new List<InboxItem>();
            allItems.AddRange(myItems.Model.Select(mod => mod.ToInboxItem()));
            foreach (var item in myItems.Model)
            {
                if (!item.Viewed)
                {
                    item.Viewed = true;
                    item.ViewedTimeStamp = DateTime.UtcNow.ToJSONString();
                    await this._inboxRepo.UpdateInboxItemAsync(item);
                }
            }

            var sysItems = await _systemNotificationRepo.GetSystemAndPublicNotifications(org.Id);
            bldr.CompleteAndRestart("[AppUserInboxManager__GetAllInboxItemsAsync__GetSystemAndPublicNotifications]");
            allItems.AddRange(sysItems.Select(sys => sys.ToInboxItem()));
            return ListResponse<InboxItem>.Create(allItems.OrderByDescending(itm=>itm.CreationDate), myItems, bldr);
        }

        public async Task<ListResponse<AppUserInboxItem>> GetUnreadInboxItemsAsync(EntityHeader org, EntityHeader user, ListRequest listRequest)
        {
            return await _inboxRepo.GetInboxItemsAsync(org.Id, user.Id, listRequest, true);
        }
    }
}
