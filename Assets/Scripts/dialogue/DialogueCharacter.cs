using UnityEngine;

namespace DialogueSystem
{
    [CreateAssetMenu(fileName = "New Character", menuName = "Dialogue System/New Dialogue Character")]

    public class DialogueCharacter : ScriptableObject
    {
        public Sprite characterPhoto;
        public string characterName;
    }
}