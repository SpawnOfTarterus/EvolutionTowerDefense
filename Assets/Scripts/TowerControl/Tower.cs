using ETD.BuildingControl;
using ETD.EnemyControl;
using ETD.PlayerControl;
using ETD.UIControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETD.TowerControl
{
    public class Tower : MonoBehaviour
    {
        int costToEvolve;
        [SerializeField] List<statusEffects> currentBuffsFromOtherTowers = new List<statusEffects>();
        List<AbilitiesAndStatusEffects> passivesToApply = new List<AbilitiesAndStatusEffects>();
        Tower myBuffGiver;

        public List<statusEffects> GetActiveStatusEffects() { return currentBuffsFromOtherTowers; }
        public void SetMyBuffGiver(Tower tower) { myBuffGiver = tower; }
        public Tower GetMyBuffGiver() { return myBuffGiver; }

        public void AddStatusEffect(statusEffects effect)
        { if (!currentBuffsFromOtherTowers.Contains(effect)) { currentBuffsFromOtherTowers.Add(effect); } }
        public void RemoveStatusEffect(statusEffects effect) //call when a tower with a passive sells;
        { if (currentBuffsFromOtherTowers.Contains(effect)) { currentBuffsFromOtherTowers.Remove(effect); } }

        private void Start()
        {
            costToEvolve = GetComponent<UISelectionDescription>().GetBuildCost();
            foreach (AbilitiesAndStatusEffects ability in GetComponent<DamageModifier>().GetActiveAbilities())
            {
                if (ability.IsStatusEffectTowerPassive())
                {
                    passivesToApply.Add(ability);
                }
            }
        }

        private void Update()
        {
            ApplyAbilityBuffToTowers();
        }

        private void ApplyAbilityBuffToTowers()
        {
            if (passivesToApply.Count == 0) { return; }
            Tower[] towers = FindObjectsOfType<Tower>();
            foreach (Tower tower in towers)
            {
                foreach (AbilitiesAndStatusEffects ability in passivesToApply)
                {
                    if (Vector3.Distance(tower.transform.position, transform.position) <= ability.GetStatusEffectRange())
                    {
                        tower.AddStatusEffect(ability.GetStatusEffect());
                        tower.SetMyBuffGiver(this);
                    }
                }
            }
        }

        public void Evolve(Tower newTower)
        {
            if(newTower.costToEvolve <= FindObjectOfType<GoldController>().GetCurrentGold())
            {
                FindObjectOfType<BuildingSpawner>().EvolveSelectedTower(this, newTower);
                FindObjectOfType<GoldController>().SpendGold(costToEvolve);
            }
            else { Debug.Log("Not enough gold."); }
        }









    }
}
