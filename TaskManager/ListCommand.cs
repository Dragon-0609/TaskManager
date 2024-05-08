using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
namespace TaskManager;

internal class ListCommand
{
	internal void OutputTaskCountAndDates()
	{

		Console.WriteLine($"Tasks: {Program.Data.AllTasks.Count}");
		IEnumerable<Task> tasks = Program.Data.AllTasks.DistinctBy<Task, string>(FormatDate);

		foreach (Task task in tasks)
		{
			string formatDate = FormatDate(task);
			Console.WriteLine($"{Program.Data.AllTasks.CountBy(t => FormatDate(t) == formatDate).Count()}: {formatDate}");
		}
	}
	internal void OutputSpecifiedDate(string date)
	{

		date = GetNow(date);

		IEnumerable<Task> filtered = Program.Data.AllTasks.Where(d => FormatDate(d) == date);

		if (filtered.Any())
		{
			Console.WriteLine($"Tasks: {filtered.Count()}");
			foreach (Task task in filtered)
			{
				Console.WriteLine($"\t{FormatTime(task.GetDate())}\t{task.Message}");
			}
		}
		else
		{
			Console.WriteLine($"No tasks found for {date}");
		}
	}
	internal string GetNow(string date)
	{

		if (date.ToLower() is ("current" or "today" or "now"))
		{
			date = FormatDate(DateTime.Now);
		}
		return date;
	}
	internal bool ShouldDisplayAllTaskCount(string date)
	{

		return string.IsNullOrEmpty(date) || date == "empty";
	}
	internal string FormatDate(Task t)
	{

		return FormatDate(t.GetDate());
	}
	internal string FormatDate(DateTime t)
	{

		return $"{t.Day:00}.{t.Month:00}.{t.Year:0000}";
	}
	internal static string FormatTime(DateTime t)
	{

		return $"{t.Hour:00}:{t.Minute:00}";
	}

	internal void OutputAllTasks()
	{
		List<Task> tasks = Program.Data.AllTasks;

		Console.WriteLine($"{tasks.Count} task{Plural(tasks)}");

		IEnumerable<IGrouping<string, Task>> groups = tasks.GroupBy(g => FormatDate(g.GetDate()));

		foreach (IGrouping<string, Task> group in groups)
		{
			Task[] tsks = group.ToArray();
			Console.WriteLine($"{group.Key}, {group.Count()} task{Plural(tsks)}");


			foreach (Task task in tsks)
			{
				Console.WriteLine($"\t{FormatTime(task.GetDate())}\t{task.Message}");
			}

		}


	}
	private static string Plural<T>(IEnumerable<T> tasks)
	{

		return (tasks.Count() > 1 ? "s" : "");
	}
}