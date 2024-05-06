using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;
namespace TaskManager
{
	[Serializable]
	public class Tasks
	{

		public List<Task> AllTasks;
		public Task[] CurrentTasks;

		public void Init()
		{
			AllTasks = new List<Task>();
			if (CurrentTasks != null)
			{
				AllTasks.AddRange(CurrentTasks);
			}
		}

		public void Save()
		{
			CurrentTasks = AllTasks.ToArray();
		}

	}

	[Serializable]
	public class Task
	{
		public string Date;
		public string Message;

		public Task(string message)
		{
			Message = message;
			Date = DateTime.Now.ToString(CultureInfo.InvariantCulture);
		}

		public DateTime GetDate() => DateTime.Parse(Date, CultureInfo.InvariantCulture);
	}
}