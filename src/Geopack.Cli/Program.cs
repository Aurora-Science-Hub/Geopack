using System.CommandLine;
using AuroraScienceHub.Geopack;

// More info about the System.CommandLine library:
// https://learn.microsoft.com/en-us/dotnet/standard/commandline/get-started-tutorial

var rootCommand = new RootCommand("Welcome to the Geopack!");

var calculateCommand = new Command("calculate", "Perform some useless calculations");
rootCommand.AddCommand(calculateCommand);

calculateCommand.SetHandler(() =>
{
    var result = new SomeCalculator().Calculate();
    Console.WriteLine($"Result: {result}");
});

return await rootCommand.InvokeAsync(args);

