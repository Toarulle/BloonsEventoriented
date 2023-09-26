using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "DeathPort", menuName = "Bloons TD/Death Port", order = 1)]
public class DeathPortObject : ScriptableObject
{
    public UnityAction<DeathPortObject, GameObject> onPop = delegate{};
    
    public void Pop(GameObject poppedBalloon)
    {
        onPop(this, poppedBalloon);
        Destroy(poppedBalloon, 16/60f);
    }
}
