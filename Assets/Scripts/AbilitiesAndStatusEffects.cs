using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability or Status Effect", menuName = "Ability/make new", order = 0)]
public class AbilitiesAndStatusEffects : ScriptableObject
{
    [Header("Status Effect")]
    [SerializeField] abilities ability;
    [SerializeField] statusEffects statusEffectToInflict;
    [Header("Customizable Effects")]
    [SerializeField] int multiHitCount;
    [SerializeField] float damageMultiplier;
    [SerializeField] float EffectChanceOneIn;
    [SerializeField] int statusEffectDamagePerSecond;
    [SerializeField] int statusEffectLifeTimeInSeconds;
    [SerializeField] float statusEffectRange;
    [SerializeField] bool isPassive;

    public abilities GetAbility() { return ability; }
    public statusEffects GetStatusEffect() { return statusEffectToInflict; }
    public int GetMultiHitCount() { return multiHitCount; }
    public float GetDamageMultiplier() { return damageMultiplier; }
    public float GetEffectChance() { return EffectChanceOneIn; }
    public int GetStatusEffectDamage() { return statusEffectDamagePerSecond; }
    public int GetStatusEffectLifeTime() { return statusEffectLifeTimeInSeconds; }
    public float GetStatusEffectRange() { return statusEffectRange; }
    public bool IsStatusEffectPassive() { return isPassive; }





}
