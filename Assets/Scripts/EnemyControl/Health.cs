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

        private void ApplyStatusEffect(DamageModifier attacker)
        {
            if(attacker.GetStatusEffect() == statusEffects.Poisoned) 
            {
                if (poisonCo != null) { StopCoroutine(poisonCo); }
                poisonCo = StartCoroutine(ProcessPoison(attacker)); 
            }
        }

        IEnumerator ProcessPoison(DamageModifier attacker)
        {
            for(int i = 0; i < attacker.GetStatusEffectLifeTime(); i++)
            {
                yield return new WaitForSeconds(1);
                LoseHealth(attacker.GetStatusEffectDamage());
                Debug.Log("Taking Poison Damage");
            }
        }

        public void TakeDamage(int damage, DamageModifier attacker)
        {
            myAttacker = attacker.GetComponent<Attacker>();
            if (attacker.GetStatusEffect() != statusEffects.None) { ApplyStatusEffect(attacker); }
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
