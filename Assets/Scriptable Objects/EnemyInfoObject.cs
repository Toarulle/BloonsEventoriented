using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyInfo", menuName = "Bloons TD/Enemy Info", order = 5)]
public class EnemyInfoObject : ScriptableObject
{
    public List<float> enemySpeeds = new List<float>();
}
