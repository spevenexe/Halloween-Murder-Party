using System.Collections.Generic;
using DialogueSystem;
using UnityEngine;

public class NPCManager : Singleton<NPCManager>
{
    private List<NPC> npcs;

    protected override void Awake()
    {
        base.Awake();

        npcs = new();
        GameObject[] objects = GameObject.FindGameObjectsWithTag("NPC");
        foreach (GameObject o in objects)
        {
            npcs.Add(o.GetComponent<NPC>());
        }
    }

    public static void SetRiskyCheck(DialogueCharacter dialogueCharacter, bool _value)
    {
        List<NPC> npcs = instance.npcs;
        foreach (NPC _npc in npcs)
        {
            if (_npc.character.characterName == dialogueCharacter.characterName)
            {
                _npc.canRiskyCheck = _value;
            }
        }

    }
    public static void SetSafeCheck(DialogueCharacter dialogueCharacter, bool _value)
    {
        List<NPC> npcs = instance.npcs;
        foreach (NPC _npc in npcs)
        {
            if (_npc.character.characterName == dialogueCharacter.characterName)
            {
                _npc.canSafeCheck = _value;
            }
        }
    }
}