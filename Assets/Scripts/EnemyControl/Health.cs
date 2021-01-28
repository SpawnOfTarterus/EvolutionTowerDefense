using ETD.PlayerControl;
using ETD.TowerControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETD.EnemyControl
{
    public class Health : MonoBehaviour
    {
        [SerializeField] int maxHealth;
        [SerializeField] int currentHealth;

        bool isDead = false;
        Coroutine poisonCo;
        Attacker myAttacker;

        public int GetMaxHealth()
        {
            return maxHealth;
        }

        private void Start()
        {
            currentHealth = maxHealth;
        }

        public void ApplyStatusEffect(AbilitiesAndStatusEffects effect)
        {
            if (poisonCo != null) { StopCoroutine(poisonCo); }
            poisonCo = StartCoroutine(ProcessPoison(effect)); 
        }

        IEnumerator ProcessPoison(AbilitiesAndStatusEffects effect)
        {
            for(int i = 0; i < effect.GetStatusEffectLifeTime(); i++)
            {
                yield return new WaitForSeconds(1);
                LoseHealth(effect.GetStatusEffectDamage());
            }
            GetComponent<DefenceApplicator>().RemoveStatusEffect(effect.GetStatusEffect());
        }

        public void TakeDamage(int damage)
        {
            LoseHealth(damage);
        }

        private void LoseHealth(int damage)
        {
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                Die();
            }
        }

        public void Die()
        {
            if(isDead) { return; }
            isDead = true;
            myAttacker.RemoveFromTargetLists(GetComponent<Enemy>());
            Enemy enemyComponent = GetComponent<Enemy>();
            enemyComponent.GetMySpawner().RemoveFromEnemiesInPlay(enemyComponent);
            if(currentHealth == 0)
            {
                FindObjectOfType<GoldController>().GainGold(enemyComponent.GetGoldReward());
            }
            Destroy(gameObject);
        }

    }
}
