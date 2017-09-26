using System;
namespace ZenLeap.Api.Authorization
{
    /// <summary>
    /// Sets up all constants for permissions. 
    /// Used in conjuntion with Operations Handlers.
    /// </summary>
	public static class Constants
	{
		public static readonly string CreateOperationName = "Create";
		public static readonly string ReadOperationName = "Read";
		public static readonly string UpdateOperationName = "Update";
		public static readonly string DeleteOperationName = "Delete";
		public static readonly string ApproveOperationName = "Approve";
		public static readonly string RejectOperationName = "Reject";

		public static readonly string TeamOwnersRole = "TeamOwner";
		public static readonly string TeamOwnersRoleNormalizedName = "Team Owner";
        public static readonly string TeamAdministratorsRole = "TeamAdministrator";
		public static readonly string TeamAdministratorsRoleNormalizedName = "Team Administrator";
		public static readonly string TeamMembersRole = "TeamMember";
		public static readonly string TeamMembersRoleNormalizedName = "Team Member";

		public static readonly string ActivityAdministratorsRole = "ActivityAdministrator";
		public static readonly string ActivityAdministratorsRoleNormalizedName = "Activity Administrator";
		public static readonly string ActivityMembersRole = "ActivityMember";
		public static readonly string ActivityMembersRoleNormalizedName = "Activity Member";

		public static readonly string GlobalAdministratorsRole = "GlobalAdministrators";
		public static readonly string GlobalAdministratorsRoleNormalizedName = "Global Administrators";

		// TODO: Deprecate
		public static readonly string CompanyAdministratorsRole = "CompanyAdministrators";
		public static readonly string CompanyManagersRole = "CompanyManagers";

		// TODO: Deprecate
		public static readonly string ProjectAdministratorsRole = "ProjectAdministrators";
		public static readonly string ProjectManagersRole = "ProjectManagers";

	}
}
