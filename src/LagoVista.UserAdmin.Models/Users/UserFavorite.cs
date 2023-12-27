﻿using LagoVista.Core;
using LagoVista.Core.Interfaces;
using LagoVista.Core.Models;
using LagoVista.Core.Validation;
using System;
using System.Collections.Generic;

namespace LagoVista.UserAdmin.Models.Users
{
    public class UserFavorites : UserAdminModelBase, IKeyedEntity, INamedEntity, IValidateable, IOwnedEntity
    {
        public UserFavorites()
        {
            Id = Guid.NewGuid().ToId();
            Favorites = new List<UserFavorite>();
        }

        public string Key { get; set; }
        public EntityHeader OwnerOrganization { get; set; }
        public EntityHeader OwnerUser { get; set; }
        public bool IsPublic { get; set; }
        public string Name { get; set; }
        public List<UserFavorite> Favorites { get; set; }
        public static string GenerateId(EntityHeader org, EntityHeader user)
        {
            return $"{org.Id}{user.Id}";
        }

        public static string GenerateId(string orgId, string userId)
        {
            return $"{orgId}{userId}";
        }
    }

    public class UserFavorite
    {
        public UserFavorite()
        {
            Id = Guid.NewGuid().ToId();
            DateAdded = DateTime.UtcNow.ToJSONString();
        }

        public string Id { get; set; }

        public string DateAdded { get; set; }
        public List<string> Route { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public string Icon { get; set; }
        public string Link { get; set; }
    }
}
