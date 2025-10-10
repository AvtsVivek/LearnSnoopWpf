// See https://aka.ms/new-console-template for more information
using PresentationSourceInConsoleAppOne.ConsoleUI;

Console.WriteLine("Hello, World!");


var utilities = new Utilities();

utilities.GetProcessDetailsAndCopyToClipboard();

var currentProcess = System.Diagnostics.Process.GetCurrentProcess();
var processDetails = $"ProcessId{currentProcess.Id}-ProcessName{currentProcess.ProcessName}";
Console.WriteLine(processDetails);

Console.WriteLine($"Process id is: {currentProcess.Id}");

var presentationSourceCount = utilities.GetPresentationSourceCount();

Console.WriteLine($"PresentationSourceCount: {presentationSourceCount}");

Console.WriteLine("Press enter to exit the process");

Console.ReadLine();


