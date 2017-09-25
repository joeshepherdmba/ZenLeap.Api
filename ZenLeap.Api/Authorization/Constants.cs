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
        public static readonly string TeamAdministratorsRole = "TeamAdministrators";
		public static readonly string TeamMembersRole = "TeamMembers";

		public static readonly string EventAdministratorsRole = "EventAdministrators";
		public static readonly string EventMembersRole = "EvetnMembers";

		public static readonly string GlobalAdministratorsRole = "GlobalAdministrators";

		// TODO: Deprecate
		public static readonly string CompanyAdministratorsRole = "CompanyAdministrators";
		public static readonly string CompanyManagersRole = "CompanyManagers";

		// TODO: Deprecate
		public static readonly string ProjectAdministratorsRole = "ProjectAdministrators";
		public static readonly string ProjectManagersRole = "ProjectManagers";

	}
}
