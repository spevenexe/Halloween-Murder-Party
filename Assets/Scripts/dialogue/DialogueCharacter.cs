using UnityEngine;

namespace DialogueSystem
{
    [CreateAssetMenu(fileName = "New Character", menuName = "Dialogue System/New DialogueCharacter")]

    public class DialogueCharacter : ScriptableObject
    {
        public Sprite characterPhoto;
        public string characterName;
    }
}