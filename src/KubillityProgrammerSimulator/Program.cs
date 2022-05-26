using KubillityProgrammerSimulator;
using KubillityProgrammerSimulator.Flows;
using KubillityProgrammerSimulator.Helpers;
using KubillityProgrammerSimulator.Interact;
using KubillityProgrammerSimulator.Interacts;
using KubillityProgrammerSimulator.Models;
using Spectre.Console;

AnsiConsole.Write(
    new FigletText("Kubillity Programmer Simulator")
        .LeftAligned()
        .Color(Color.Red));

var aside = Game.Instance.GetAside("Prologue");
if (aside != null)
{
    var flow = new MultilineTextFlow(100);
    foreach (var text in aside.Parse())
    {
        flow.AddText(text);
    }
    flow.Show();
}
AnsiConsole.Write(new Rule());

new GameStartInteract().Invoke();
