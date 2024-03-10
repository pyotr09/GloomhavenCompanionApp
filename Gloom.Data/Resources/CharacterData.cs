using System.Collections.Generic;

namespace Gloom.Data;

public static class CharacterData
{
    public static readonly Dictionary<string, int[]> HPbyLevel = new()
    {
        {"Brute", new []{0,10,12,14,16,18,20,22,24,26} },
        {"Cragheart", new []{0,10,12,14,16,18,20,22,24,26} },
        {"Spellweaver", new []{0,6,7,8,9,10,11,12,13,14} },
        {"Scoundrel", new []{0,8,9,11,12,14,15,17,18,20} },
        {"Tinkerer", new []{0,8,9,11,12,14,15,17,18,20} },
        {"Mindthief", new []{0,6,7,8,9,10,11,12,13,14} },
        {"Doomstalker", new []{0,8,9,11,12,14,15,17,18,20} },
        {"Summoner", new []{0,6,7,8,9,10,11,12,13,14} },
        {"Nightshroud", new []{0,8,9,11,12,14,15,17,18,20} },
        {"Berserker", new []{0,10,12,14,16,18,20,22,24,26} },
        {"Sooothsinger", new []{0,6,7,8,9,10,11,12,13,14} },
        {"Sawbones", new []{0,8,9,11,12,14,15,17,18,20} },
        {"Quartermaster", new []{0,10,12,14,16,18,20,22,24,26} },
        {"Plagueherald", new []{0,6,7,8,9,10,11,12,13,14} },
        {"Sunkeeper", new []{0,10,12,14,16,18,20,22,24,26} },
        {"Elementalist", new []{0,6,7,8,9,10,11,12,13,14} },
        {"Beast Tyrant", new []{0,6,7,8,9,10,11,12,13,14} }
    };
}