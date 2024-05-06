using System;
using System.Collections.Generic;
using System.Linq;
using Cocona;
using MoreLinq;
namespace TaskManager
{
	internal class Program
	{
		public static Tasks Data;

		public static void Main(string[] args)
		{
			LoadData();

			CoconaApp.Run<Program>(args);
		}
		private static void LoadData()
		{

			Data = DataSaver.Load("Tasks", new Tasks()
			{
				AllTasks = new List<Task>()
			});
			Data.Init();
		}

		[Command(Aliases = new[]
		{
			"l"
		})]
		public void List([Option] string date = "empty")
		{
			if (Data.AllTasks.Count > 0)
			{
				if (string.IsNullOrEmpty(date) || date == "empty")
				{
					Console.WriteLine($"Tasks: {Data.AllTasks.Count}");
					IEnumerable<Task> tasks = Data.AllTasks.DistinctBy(FormatDate);

					foreach (Task task in tasks)
					{
						string formatDate = FormatDate(task);
						Console.WriteLine($"{Data.AllTasks.CountBy(t => FormatDate(t) == formatDate).Count()}: {formatDate}");
					}
				}
				else
				{
					if (date.ToLower() is ("current" or "today" or "now"))
					{
						date = FormatDate(DateTime.Now);
					}

					IEnumerable<Task> filtered = Data.AllTasks.Where(d => FormatDate(d) == date);

					if (filtered.Any())
					{
						Console.WriteLine($"Tasks: {filtered.Count()}");
						foreach (Task task in filtered)
						{
							Console.WriteLine($"\t{task.Date}\t{task.Message}");
						}
					}
					else
					{
						Console.WriteLine($"No tasks found for {date}");
					}
				}
			}
			else
			{
				Console.WriteLine($"No tasks added");
			}
		}
		private static string FormatDate(Task t)
		{

			return FormatDate(t.GetDate());
		}
		private static string FormatDate(DateTime t)
		{

			return $"{t.Day:00}.{t.Month:00}.{t.Year:0000}";
		}

		[Command]
		public void Add([Argument] string message)
		{
			Data.AllTasks.Add(new Task(message));
			Data.Save();
			DataSaver.Save("Tasks", Data);
		}

	}
}