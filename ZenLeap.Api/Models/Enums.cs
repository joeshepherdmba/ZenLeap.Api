﻿using System;
namespace ZenLeap.Api.Models
{

    //testing branch commit
    //test commit command
    public class Enums
    {
        public enum Priority
        {
            High,
            Medium,
            Low
        };

        public enum TaskStatus
        {
            New,
            Inprogress,
            OnHold,
            Done
        };

		public enum TimeStatus
		{
			Submitted, 
            Approved, 
            Denied
		};
    }
}
