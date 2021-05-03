using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Character Info")]
public class CharacterInfo : ScriptableObject
{
    public string characterName;
    public Sprite sprite;
    public bool isLeftAligned;
}
