using ETD.TowerControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageModifier : MonoBehaviour
{
    [SerializeField] AbilitiesAndStatusEffects[] activeAbilities;

    float half = 0.5f;
    float quarter = 0.25f;
    float processedDamage;

    public AbilitiesAndStatusEffects[] GetActiveAbilities() { return activeAbilities; }

    public float CalculateDamageForProjectile(int rawDamage, evoTypes attackerType, evoTypes targetType)
    {
        ProcessChangeForType(rawDamage, attackerType, targetType);
        ProcessChangeForAbilities();
        return processedDamage;
    }

    private void ProcessChangeForType(int rawDamage, evoTypes attackerType, evoTypes targetType)
    {
        if(attackerType == evoTypes.Human) { processedDamage = ProcessHumanAttack(rawDamage, targetType); }
        else if (attackerType == evoTypes.Beast) { processedDamage = ProcessBeastAttack(rawDamage, targetType); }
        else if (attackerType == evoTypes.Undead) { processedDamage = ProcessUndeadAttack(rawDamage, targetType); }
        else if (attackerType == evoTypes.Goblinkin) { processedDamage = ProcessGoblinkinAttack(rawDamage, targetType); }
        else { processedDamage = rawDamage; }
    }

    private void ProcessChangeForAbilities()
    {
        ProcessChangeForRange();
        ProcessChangeForCrit();
    }

    private void ProcessChangeForRange()
    {
        foreach(AbilitiesAndStatusEffects ability in activeAbilities)
        {
            if(ability.GetAbility() == abilities.Sniper)
            {
                float distanceToTarget = Vector3.Distance
                    (transform.position, GetComponent<Attacker>().GetCurrentTarget().transform.position);
                processedDamage *= Mathf.Max
                    (1, distanceToTarget / (GetComponent<Attacker>().GetRange() * half));
                //any distance <= half the range = 1x damage. Any distance > half the range = up to 2x damage at full range.
            }
        }
    }

    private void ProcessChangeForCrit()
    {
        foreach(AbilitiesAndStatusEffects ability in activeAbilities)
        {
            if (ability.GetAbility() == abilities.Crit)
            {
                //0.5 is used to start and added to the end to give 1 and 4 an equal chance to roll as 2 and 3.
                //mathf.epsilon is used to prevent 5 from being rolled.
                if(Mathf.RoundToInt(Random.Range(0.5f, ability.GetEffectChance() + (0.5f - Mathf.Epsilon))) == 1)
                {
                    processedDamage *= ability.GetDamageMultiplier();
                }
            }
        }
    }

    private float ProcessHumanAttack(float rawDamage, evoTypes targetType)
    {
        return rawDamage - (rawDamage * quarter);
    }

    private float ProcessBeastAttack(float rawDamage, evoTypes targetType)
    {
        if (targetType == evoTypes.Undead) { return rawDamage + (rawDamage * half); }
        else { return rawDamage; }
    }

    private float ProcessUndeadAttack(float rawDamage, evoTypes targetType)
    {
        if (targetType == evoTypes.Human) { return rawDamage + (rawDamage * quarter); }
        if (targetType == evoTypes.Goblinkin) { return rawDamage + (rawDamage * quarter); }
        else { return rawDamage; }
    }

    private float ProcessGoblinkinAttack(float rawDamage, evoTypes targetType)
    {
        return rawDamage; 
    }

}
