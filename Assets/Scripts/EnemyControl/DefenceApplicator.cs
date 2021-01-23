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

    public void ApplyDamageReduction(float rawDamage, EvoTypes attackerType)
    {
        EvoTypes myType = GetComponent<UISelectionDescription>().GetMyType();
        processedDamage = ProcessChangeForType(rawDamage, attackerType, myType);
        Debug.Log($"Raw damage after defence= {processedDamage}");
        GetComponent<Health>().TakeDamage(Mathf.RoundToInt(processedDamage));
    }

    private float ProcessChangeForType(float rawDamage, EvoTypes attackerType, EvoTypes myType)
    {
        if(myType == EvoTypes.Human) { return ProcessHumanDef(rawDamage, attackerType); }
        else if (myType == EvoTypes.Beast) { return ProcessBeastDef(rawDamage, attackerType); }
        else if (myType == EvoTypes.Undead) { return ProcessUndeadDef(rawDamage, attackerType); }
        else if (myType == EvoTypes.Goblinkin) { return ProcessGoblinkinDef(rawDamage, attackerType); }
        else { return rawDamage; }
    }

    private float ProcessHumanDef(float rawDamage, EvoTypes attackerType)
    {
        return rawDamage + (rawDamage * quarter);
    }

    private float ProcessBeastDef(float rawDamage, EvoTypes attackerType)
    {
        return rawDamage - (rawDamage * quarter);
    }

    private float ProcessUndeadDef(float rawDamage, EvoTypes attackerType)
    {
        return rawDamage;
    }

    private float ProcessGoblinkinDef(float rawDamage, EvoTypes attackerType)
    {
        return  rawDamage;
    }

}
