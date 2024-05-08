using System;
using System.Collections.Generic;
using Cocona;
namespace TaskManager
{
	internal class Program
	{
		public static Tasks Data;
		private static ListCommand _listCommand;

		public static ConsoleColor DefaultColor;

		public static void Main(string[] args)
		{
			DefaultColor = Console.ForegroundColor;
			LoadData();
			InitCommands();
			CoconaApp.Run<Program>(args);
		}
		private static void InitCommands()
		{

			_listCommand = new ListCommand();
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
		public void List([Argument] string date = "empty", [Option('a')] bool all = false)
		{
			if (all)
			{
				_listCommand.OutputAllTasks();
			}
			else if (Data.AllTasks.Count > 0)
			{
				if (_listCommand.ShouldDisplayAllTaskCount(date))
				{
					_listCommand.OutputTaskCountAndDates();
				}
				else
				{
					_listCommand.OutputSpecifiedDate(date);
				}
			}
			else
			{
				Console.WriteLine($"No tasks added");
			}
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