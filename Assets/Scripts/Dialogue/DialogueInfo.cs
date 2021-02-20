using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue")]
public class DialogueInfo : ScriptableObject
{
    [System.Serializable]
    public class Sentence
    {
        public CharacterInfo character;
        [TextArea(3, 10)]
        public string text;
        public bool isChoice;
        [TextArea(3, 10)]
        public string[] choices;
    }

    public Sentence[] sentences;
}
