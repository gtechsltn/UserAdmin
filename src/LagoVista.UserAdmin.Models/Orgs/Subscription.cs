﻿using LagoVista.Core.Attributes;
using LagoVista.Core.Interfaces;
using LagoVista.Core.Models;
using LagoVista.Core.Validation;
using LagoVista.UserAdmin.Models.Resources;
using System;
using System.Collections.Generic;

namespace LagoVista.UserAdmin.Models.Orgs
{
    [EntityDescription(Domains.OrganizationDomain, UserAdminResources.Names.Subscription_Title, UserAdminResources.Names.Subscription_Help, 
        UserAdminResources.Names.Subscription_Description, EntityDescriptionAttribute.EntityTypes.SimpleModel, typeof(UserAdminResources),
        GetListUrl: "/api/subscriptions", GetUrl: "/api/subscription/{id}", SaveUrl: "/api/subscription", FactoryUrl: "/api/subscription/factory")]
    public class Subscription : IValidateable, IKeyedEntity, INamedEntity, ISummaryFactory, IFormDescriptor
    {
        public const string Status_OK = "ok";
        public const string Status_FreeAccount = "freeaccount";
        public const string Status_TrialAccount = "trialaccount";
        public const string Status_NoPaymentDetails = "nopaymentdetails";

        public const string PaymentTokenStatus_OK = "ok";
        public const string PaymentTokenStatus_Waived = "waived";
        public const string PaymentTokenStatus_Empty = "empty";
        public const string PaymentTokenStatus_Invalid = "invalid";

        public const string SubscriptionKey_Trial = "trial";

        public Guid Id { get; set; }

        public String OrgId { get; set; }

        public string CreatedById { get; set; }

        public string LastUpdatedById { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastUpdatedDate { get; set; }
        public string CustomerId { get; set; }

        [FormField(LabelResource: UserAdminResources.Names.Subscription_PaymentMethod, HelpResource:UserAdminResources.Names.Subscription_PaymentMethod_Help, FieldType: FieldTypes.PaymentMethod, ResourceType: typeof(UserAdminResources), IsRequired: true)]
        public string PaymentToken { get; set; }

        [FormField(LabelResource: UserAdminResources.Names.Subscription_PaymentMethod_Date, FieldType: FieldTypes.Date, IsUserEditable:false, ResourceType: typeof(UserAdminResources))]
        public DateTime? PaymentTokenDate { get; set; }

        [FormField(LabelResource: UserAdminResources.Names.Subscription_PaymentMethod_Expires, FieldType: FieldTypes.Date, ResourceType: typeof(UserAdminResources), IsUserEditable: false)]
        public DateTime? PaymentTokenExpires { get; set; }

        [FormField(LabelResource: UserAdminResources.Names.Subscription_PaymentMethod_Status, FieldType: FieldTypes.ReadonlyLabel, ResourceType: typeof(UserAdminResources), IsRequired: false)]
        public string PaymentTokenStatus { get; set; }

        [FormField(LabelResource: UserAdminResources.Names.Subscription_Status, FieldType: FieldTypes.ReadonlyLabel, ResourceType: typeof(UserAdminResources), IsRequired: false)]
        public String Status { get; set; }

        [FormField(LabelResource: UserAdminResources.Names.Common_Key, HelpResource: UserAdminResources.Names.Common_Key_Help, FieldType: FieldTypes.Key, RegExValidationMessageResource: UserAdminResources.Names.Common_Key_Validation, ResourceType: typeof(UserAdminResources), IsRequired: true)]
        public string Key { get; set; }

        [FormField(LabelResource: UserAdminResources.Names.Common_Name, FieldType: FieldTypes.Text, ResourceType: typeof(UserAdminResources), IsRequired: true)]
        public string Name { get; set; }

        [FormField(LabelResource: UserAdminResources.Names.Common_Description, FieldType: FieldTypes.MultiLineText, ResourceType: typeof(UserAdminResources), IsRequired: false)]
        public string Description { get; set; }

        [CustomValidator]
        public void Validate(ValidationResult result, Actions action)
        {
            if (action == Actions.Update && Key == SubscriptionKey_Trial)
            {
                result.AddUserError("Can not update trial subscription.");
            }
        }

        public SubscriptionSummary CreateSummary()
        {
            return new SubscriptionSummary()
            {
                Id = Id,
                Name = Name,
                PaymentTokenStatus = PaymentTokenStatus,
                Key = Key,
                Description = Description,
                IsPublic = false
                
            };
        }

        ISummaryData ISummaryFactory.CreateSummary()
        {
            return CreateSummary();
        }

        public List<string> GetFormFields()
        {
            return new List<string>()
            {
                nameof(Name),
                nameof(Key),
                nameof(Status),
                nameof(PaymentToken),
                nameof(PaymentTokenStatus),
                nameof(PaymentTokenDate),
                nameof(PaymentTokenExpires),
                nameof(Description),
            };
        }
    }

    [EntityDescription(Domains.OrganizationDomain, UserAdminResources.Names.Subscription_Title, UserAdminResources.Names.Subscription_Help,
        UserAdminResources.Names.Subscription_Description, EntityDescriptionAttribute.EntityTypes.Summary, typeof(UserAdminResources),
        GetListUrl: "/api/subscriptions", GetUrl: "/api/subscription/{id}", SaveUrl: "/api/subscription", FactoryUrl: "/api/subscription/factory")]
    public class SubscriptionSummary : SummaryData
    {
        public new Guid Id { get; set; }
        public string Status { get; set; }
        public string PaymentTokenStatus { get; set; }
    }
}
