using ETD.EnemyControl;
using ETD.UIControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenceApplicator : MonoBehaviour
{
    float half = 0.5f;
    float quarter = 0.25f;
    float processedDamage;

    public void ApplyDamageReduction(float rawDamage, evoTypes attackerType, DamageModifier attacker)
    {
        evoTypes myType = GetComponent<UISelectionDescription>().GetMyType();
        processedDamage = ProcessChangeForType(rawDamage, attackerType, myType);
        GetComponent<Health>().TakeDamage(Mathf.RoundToInt(processedDamage), attacker);
    }

    private float ProcessChangeForType(float rawDamage, evoTypes attackerType, evoTypes myType)
    {
        if(myType == evoTypes.Human) { return ProcessHumanDef(rawDamage, attackerType); }
        else if (myType == evoTypes.Beast) { return ProcessBeastDef(rawDamage, attackerType); }
        else if (myType == evoTypes.Undead) { return ProcessUndeadDef(rawDamage, attackerType); }
        else if (myType == evoTypes.Goblinkin) { return ProcessGoblinkinDef(rawDamage, attackerType); }
        else { return rawDamage; }
    }

    private float ProcessHumanDef(float rawDamage, evoTypes attackerType)
    {
        return rawDamage + (rawDamage * quarter);
    }

    private float ProcessBeastDef(float rawDamage, evoTypes attackerType)
    {
        return rawDamage - (rawDamage * quarter);
    }

    private float ProcessUndeadDef(float rawDamage, evoTypes attackerType)
    {
        return rawDamage;
    }

    private float ProcessGoblinkinDef(float rawDamage, evoTypes attackerType)
    {
        return  rawDamage;
    }

}
