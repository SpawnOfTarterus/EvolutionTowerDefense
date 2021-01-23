using ETD.TowerControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageModifier : MonoBehaviour
{
    float half = 0.5f;
    float quarter = 0.25f;
    float processedDamage;

    public float CalculateDamageForProjectile(int rawDamage, EvoTypes attackerType, EvoTypes targetType)
    {
        return ProcessChangeForType(rawDamage, attackerType, targetType);
    }

    private float ProcessChangeForType(int rawDamage, EvoTypes attackerType, EvoTypes targetType)
    {
        if(attackerType == EvoTypes.Human) { processedDamage = ProcessHumanAttack(rawDamage, targetType); }
        else if (attackerType == EvoTypes.Beast) { processedDamage = ProcessBeastAttack(rawDamage, targetType); }
        else if (attackerType == EvoTypes.Undead) { processedDamage = ProcessUndeadAttack(rawDamage, targetType); }
        else if (attackerType == EvoTypes.Goblinkin) { processedDamage = ProcessGoblinkinAttack(rawDamage, targetType); }
        return processedDamage;
    }

    private float ProcessHumanAttack(float rawDamage, EvoTypes targetType)
    {
        return rawDamage - (rawDamage * quarter);
    }

    private float ProcessBeastAttack(float rawDamage, EvoTypes targetType)
    {
        if (targetType == EvoTypes.Undead) { return rawDamage + (rawDamage * half); }
        else { return rawDamage; }
    }

    private float ProcessUndeadAttack(float rawDamage, EvoTypes targetType)
    {
        if (targetType == EvoTypes.Human) { return rawDamage + (rawDamage * quarter); }
        if (targetType == EvoTypes.Goblinkin) { return rawDamage + (rawDamage * quarter); }
        else { return rawDamage; }
    }

    private float ProcessGoblinkinAttack(float rawDamage, EvoTypes targetType)
    {
        return rawDamage; 
    }

}
