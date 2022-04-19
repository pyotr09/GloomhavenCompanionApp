using System.Linq;
using Gloom.Data;

namespace Gloom.Model;

public static class Utils
{
    public static string GetDeckName(string monsterName, string expansion)
    {
        if (expansion == "Jaws of the Lion")
        {
            if (monsterName.Contains("Imp"))
                return "Jaws of the Lion Imp";
            if (monsterName.Equals("Monstrosity"))
                return "Monstrosity";
            if (monsterName.Equals("Vermling Raider"))
                return "Vermling Raider";
            if (monsterName.Equals("Vermling Scout"))
                return "Vermling Scout";
            if (monsterName.Equals("Zealot"))
                return "Zealot";
            if (monsterName.Equals("Blood Ooze"))
                return "Blood Ooze";
            if (MonsterStatsDeserialized.Instance.Bosses.Any(b => b.Name == monsterName))
                return "Jaws of the Lion Boss";
            return "Jaws of the Lion " + monsterName;
        }
        if (expansion == "Crimson Scales")
        {
            if (monsterName.Equals("Toxic Imp"))
                return "Toxic Imp";
        }
        if (monsterName.Contains("Archer"))
            return "Archer";
        if (monsterName.Contains("Guard"))
            return "Guard";
        if (monsterName.Contains("Imp"))
            return "Imp";
        if (monsterName.Contains("Scout"))
            return "Scout";
        if (monsterName.Contains("Shaman"))
            return "Shaman";
        if (monsterName.Contains("Ashblade"))
            return "Ashblade";
        if (monsterName.Contains("Savage"))
            return "Savage";
        if (monsterName.Contains("Tracker"))
            return "Tracker";
        if (MonsterStatsDeserialized.Instance.Bosses.Any(b => b.Name == monsterName))
            return "Boss";

        return monsterName;
    }
}