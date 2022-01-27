using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AttackScheme : ScriptableObject
{
    public AttackConfig DefaultAttackConfig;
    public List<AttackSequence> _attackSequences;
}