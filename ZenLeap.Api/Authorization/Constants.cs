using System;
namespace ZenLeap.Api.Authorization
{
	public static class Constants
	{
		public static readonly string CreateOperationName = "Create";
		public static readonly string ReadOperationName = "Read";
		public static readonly string UpdateOperationName = "Update";
		public static readonly string DeleteOperationName = "Delete";
		public static readonly string ApproveOperationName = "Approve";
		public static readonly string RejectOperationName = "Reject";

		public static readonly string CompanyAdministratorsRole = "CompanyAdministrators";
		public static readonly string CompanyManagersRole = "CompanyManagers";

		public static readonly string ProjectAdministratorsRole = "ProjectAdministrators";
		public static readonly string ProjectManagersRole = "ProjectManagers";
	}
}
