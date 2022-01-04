using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AttackSequence : ScriptableObject
{
    public List<Attack> _attacks;
    
    /*default -> toAttack hold
    waitHold
    toAttack ->fromAttack
    waitSequence
    overAttackSmall - from here to new attack (from default) , or sequence attack (toAttack with hold)
    overAttackBig*/
}