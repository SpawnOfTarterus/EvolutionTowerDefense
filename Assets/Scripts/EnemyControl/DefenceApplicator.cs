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
    [SerializeField] List <statusEffects> currentStatusEffects = new List<statusEffects>();
    List<statusEffects> passives = new List<statusEffects>();

    public void ApplyStatusEffect(DamageModifier attacker)
    {
        AbilitiesAndStatusEffects[] abilitiesAndEffects = attacker.GetActiveAbilities();
        foreach(AbilitiesAndStatusEffects effects in abilitiesAndEffects)
        {
            if(effects.GetStatusEffect() == statusEffects.None) { continue; }
            if(effects.IsStatusEffectEnemyPassive()) { continue; }
            if(!currentStatusEffects.Contains(effects.GetStatusEffect()))
            {
                currentStatusEffects.Add(effects.GetStatusEffect());
                TriggerStatusEffect(effects);
            }
        }
    }

    public void RemoveStatusEffect(statusEffects statusEffectToBeRemoved)
    {
        if(!currentStatusEffects.Contains(statusEffectToBeRemoved)) { return; }
        currentStatusEffects.Remove(statusEffectToBeRemoved);
    }

    private void TriggerStatusEffect(AbilitiesAndStatusEffects effect)
    {
        if(effect.GetStatusEffect() == statusEffects.Poisoned) { GetComponent<Health>().ApplyStatusEffect(effect); }
        if(effect.GetStatusEffect() == statusEffects.Stunned) { GetComponent<Mover>().ApplyStatusEffect(effect); }
    }

    private void Update()
    {
        PassiveStatusEffectControl();
    }

    private void PassiveStatusEffectControl()
    {
        List <statusEffects> statusEffectsInRange = new List<statusEffects>();
        Tower[] allTowers = FindObjectsOfType<Tower>();
        foreach(Tower tower in allTowers)
        {
            AbilitiesAndStatusEffects[] abilities = tower.GetComponent<DamageModifier>().GetActiveAbilities();
            foreach(AbilitiesAndStatusEffects ability in abilities)
            {
                if(ability.IsStatusEffectEnemyPassive())
                {
                    if(!passives.Contains(ability.GetStatusEffect())) { passives.Add(ability.GetStatusEffect()); }
                    if(Vector3.Distance(transform.position, tower.transform.position) <= ability.GetStatusEffectRange())
                    {
                        if(!currentStatusEffects.Contains(ability.GetStatusEffect()))
                        {
                            currentStatusEffects.Add(ability.GetStatusEffect());
                        }
                        statusEffectsInRange.Add(ability.GetStatusEffect());
                    }
                }
            }
        }
        List<statusEffects> tempCurrentStatusEffects = MakeCopy(currentStatusEffects);
        foreach(statusEffects effect in tempCurrentStatusEffects)
        {
            if(!passives.Contains(effect)) { continue; }
            if(!statusEffectsInRange.Contains(effect))
            {
                currentStatusEffects.Remove(effect);
            }
        }
    }

    private List<statusEffects> MakeCopy(List<statusEffects> listToCopy)
    {
        List<statusEffects> copy = new List<statusEffects>();
        foreach(statusEffects effect in listToCopy)
        {
            copy.Add(effect);
        }
        return copy;
    }

    public void ApplyDamageReduction(float rawDamage, evoTypes attackerType, DamageModifier attacker)
    {
        processedDamage = rawDamage;
        evoTypes myType = GetComponent<UISelectionDescription>().GetMyType();
        ApplyStatusEffect(attacker);
        ProcessChangeForStatusEffect(processedDamage);
        ProcessChangeForType(processedDamage, attackerType, myType);
        GetComponent<Health>().TakeDamage(Mathf.RoundToInt(processedDamage));
    }

    private void ProcessChangeForStatusEffect(float rawDamage)
    {
        if(currentStatusEffects.Contains(statusEffects.Intimidated))
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
