using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Avatar", menuName = "GameData/Avatar", order = 1)]
public class Avatar : ScriptableObject
{
    public byte ID = 0;
    public Sprite AvatarSprite;
}
