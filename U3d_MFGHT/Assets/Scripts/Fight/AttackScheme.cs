using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AttackScheme : ScriptableObject
{
    public AttackConfig DefaultAttackIdlePositionConfig;
    public AttackConfig DefaultAttackConfig;
    public List<AttackSequence> _attackSequences;
}