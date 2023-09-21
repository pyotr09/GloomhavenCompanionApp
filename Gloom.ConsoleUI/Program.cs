// See https://aka.ms/new-console-template for more information

using Gloom.Models.Monsters;
using Gloom.Models.Scenario;

Scenario? scenario = null;

var addScenario = () =>
{
    Console.Write("Level: ");
    var levelText = Console.ReadLine();
    int level;
    while (!int.TryParse(levelText, out level))
    {
        Console.Write("Not an integer, try again: ");
        levelText = Console.ReadLine();
    }
    
    Console.Write("Number (-1 for custom): ");
    var numText = Console.ReadLine();
    int num;
    while (!int.TryParse(numText, out num))
    {
        Console.Write("Not an integer, try again: ");
        numText = Console.ReadLine();
    }

    if (num == -1)
    {
        Console.Write("Name: ");
        var name = Console.ReadLine();
        scenario = new Scenario(level, name);
    }
    else
    {
        scenario = new Scenario(level, num, "Gloomhaven");
    }
    
    return scenario.ToString();
};

var addMonsterGroup = () =>
{
    if (scenario == null)
        return "Add a scenario first! Use \"add scenario\" command";
    
    Console.Write("Name: ");
    var name = Console.ReadLine();
    
    Console.Write("Name of Ability Deck: ");
    var deckName = Console.ReadLine();
    
    scenario.AddMonsterGroup(name, deckName);
    return scenario.ToString();
};

var addMonster = () =>
{
    if (scenario == null)
        return "Add a scenario first! Use \"add scenario\" command";
    if (scenario.MonsterGroups.Count == 0)
        return "Add a monster group first! Use \"add group\" command";

    string groupText;
    if (scenario.MonsterGroups.Count == 1)
    {
        groupText = scenario.MonsterGroups.First().Name;
    }
    else
    {
        var groupNames = scenario.MonsterGroups
            .Select(g => g.Name).ToList();
        var namesString = string.Join(",", groupNames);
        do
        {
            Console.Write(
                $"Which group [{namesString}]? ");
            groupText = Console.ReadLine() ?? string.Empty;
        } while (!groupNames.Contains(groupText));
    }

    string ne;
    do
    {
        Console.Write("Normal/Elite? (Enter n or e): ");
        ne = Console.ReadLine() ?? string.Empty;
    } while (ne != "n" && ne != "e");

    var tier = ne == "n" ? MonsterTier.Normal : MonsterTier.Elite;
    scenario.AddMonster(groupText, tier);

    return scenario.ToString();
};

var draw = () =>
{
    if (scenario == null)
        return "Add a scenario first! Use \"add scenario\" command";
    if (scenario.MonsterGroups.Count == 0)
        return "Add a monster group first! Use \"add group\" command";
    if (scenario.MonsterGroups.Where(g => g is MonsterGrouping)
        .All(g => (g as MonsterGrouping).Monsters.Count == 0))
    {
        return "Add a monster first! Use \"add monster\" command";
    }
    
    scenario.Draw();
    
    return scenario.ToString();
};

Console.WriteLine("Hello, World!");
string? commandText;
do
{
    Console.Write("Enter Command: ");
    commandText = Console.ReadLine();
    string output = "";

    try
    {
        if (commandText == "add scenario")
        {
            output = addScenario();
        }

        if (commandText == "add group")
        {
            output = addMonsterGroup();
        }

        if (commandText == "add monster")
        {
            output = addMonster();
        }

        if (commandText == "draw")
        {
            output = draw();
        }

        if (commandText == "help")
        {
            output = "You are beyond help!";
        }
    }
    catch (Exception e)
    {
        output = e.Message + "\n" + e.StackTrace;
    }

    Console.WriteLine(output);
} while(commandText != "exit");

Console.WriteLine("Goodbye, World!");