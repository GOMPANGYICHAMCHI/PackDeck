using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Scriptable Object/GameData")]
public class GameDataSobject : ScriptableObject
{
    [SerializeField]
    public GameData gameData;
}
