using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "SelectTowerPort", menuName = "Bloons TD/Select Tower Port", order = 4)]
public class SelectTowerPortObject : ScriptableObject
{
    public UnityAction<SelectTowerPortObject, TowerBehaviour> onSelect = delegate{};
    public UnityAction<SelectTowerPortObject, TowerBehaviour> onDeSelect = delegate{};

    public void Select(TowerBehaviour towerBehaviour)
    {
        onSelect(this, towerBehaviour);
    }
    
    public void DeSelect(TowerBehaviour towerBehaviour)
    {
        onDeSelect(this, towerBehaviour);
    }
}
