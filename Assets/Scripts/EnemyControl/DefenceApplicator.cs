using ETD.EnemyControl;
using ETD.TowerControl;
using ETD.UIControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenceApplicator : MonoBehaviour
{
    float half = 0.5f;
    float quarter = 0.25f;
    float processedDamage;
    [SerializeField] statusEffects currentStatusEffect;

    public void ApplyStatusEffect(statusEffects newStatusEffect)
    {
        if(currentStatusEffect == newStatusEffect) { return; }
        currentStatusEffect = newStatusEffect;
    }

    private void Update()
    {
        RemoveStatusEffect();
    }

    public void RemoveStatusEffect()
    {
        if(currentStatusEffect == statusEffects.None) { return; }
        bool stillInRange = false;
        Tower[] allTowers = FindObjectsOfType<Tower>();
        foreach(Tower tower in allTowers)
        {
            DamageModifier damageModifier = tower.GetComponent<DamageModifier>();
            if (damageModifier.GetStatusEffect() == statusEffects.None) { continue; }
            if(damageModifier.GetStatusEffect() == currentStatusEffect)
            {
                if(Vector3.Distance(transform.position, tower.transform.position) <= damageModifier.GetStatusEffectRange())
                {
                    stillInRange = true;
                }
            }
        }
        if(!stillInRange) { currentStatusEffect = statusEffects.None; }
    }

    public void ApplyDamageReduction(float rawDamage, evoTypes attackerType, DamageModifier attacker)
    {
        processedDamage = rawDamage;
        evoTypes myType = GetComponent<UISelectionDescription>().GetMyType();
        ProcessChangeForStatusEffect(processedDamage);
        ProcessChangeForType(processedDamage, attackerType, myType);
        GetComponent<Health>().TakeDamage(Mathf.RoundToInt(processedDamage), attacker);
    }

    private void ProcessChangeForStatusEffect(float rawDamage)
    {
        if(currentStatusEffect == statusEffects.Intimidated)
        {
            processedDamage = rawDamage + (rawDamage * quarter);
        }
    }

    private void ProcessChangeForType(float rawDamage, evoTypes attackerType, evoTypes myType)
    {
        if(myType == evoTypes.Human) { processedDamage = ProcessHumanDef(rawDamage, attackerType); }
        else if (myType == evoTypes.Beast) { processedDamage = ProcessBeastDef(rawDamage, attackerType); }
        else if (myType == evoTypes.Undead) { processedDamage = ProcessUndeadDef(rawDamage, attackerType); }
        else if (myType == evoTypes.Goblinkin) { processedDamage = ProcessGoblinkinDef(rawDamage, attackerType); }
        else { processedDamage = rawDamage; }
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
