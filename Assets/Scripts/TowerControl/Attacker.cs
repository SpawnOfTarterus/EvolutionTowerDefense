using ETD.EnemyControl;
using ETD.UIControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETD.TowerControl
{
    public class Attacker : MonoBehaviour
    {
        [SerializeField] int damage;
        [SerializeField] float range;
        [SerializeField] float attacksPerSecond;
        [SerializeField] Projectile projectile;
        [SerializeField] int numberCanAttack = 1;

        Transform projectileParent;
        List<Enemy> potentialTargets = new List<Enemy>();
        List<Enemy> currentTargets = new List<Enemy>();
        float attackTimer = Mathf.Infinity;

        public void SetProjectileParent(Transform parent)
        {
            projectileParent = parent;
        }

        public Enemy GetCurrentTarget()
        {
            return currentTargets[0];
        }

        public int GetDamage()
        {
            return damage;
        }

        public float GetRange()
        {
            return range;
        }

        public float GetAttackSpeed()
        {
            return attacksPerSecond;
        }

        public void SetAttackSpeed(float newAttackSpeed)
        {
            attacksPerSecond = newAttackSpeed;
        }

        private void Start()
        {
            AbilitiesAndStatusEffects[] myAbilities = GetComponent<DamageModifier>().GetActiveAbilities();
            foreach(AbilitiesAndStatusEffects ability in myAbilities)
            {
                if(ability.GetAbility() == abilities.MultiShot)
                {
                    numberCanAttack = ability.GetMultiHitCount();
                }
            }
        }

        private void Update()
        {
            AttackMechanic();
        }

        private void AttackMechanic()
        {
            GetTargets();
            AttackTargets();
        }

        private void GetTargets()
        {
            FindTargets();
            SelectTargets();
        }

        private void SelectTargets()
        {
            if(potentialTargets.Count == 0) { return; }
            Enemy newTarget = GetClosestEnemy();
            if(newTarget == null) { return; }
            if(currentTargets.Count < numberCanAttack) { currentTargets.Add(newTarget); }
        }

        private void FindTargets()
        {
            Enemy[] enemies = FindObjectsOfType<Enemy>();
            foreach (Enemy enemy in enemies)
            {
                float distanceToEnemy = Vector3.Distance(enemy.transform.position, transform.position);
                if (distanceToEnemy > range)
                {
                    if (potentialTargets.Contains(enemy)) { potentialTargets.Remove(enemy); }
                    if (currentTargets.Contains(enemy)) { currentTargets.Remove(enemy); }
                }
                else if (distanceToEnemy <= range)
                {
                    if (!potentialTargets.Contains(enemy)) { potentialTargets.Add(enemy); }
                }
            }
        }

        public void RemoveFromTargetLists(Enemy enemy)
        {
            if (potentialTargets.Contains(enemy)) { potentialTargets.Remove(enemy); }
            if (currentTargets.Contains(enemy)) { currentTargets.Remove(enemy); }
        }

        private Enemy GetClosestEnemy()
        {
            Enemy closestEnemy = null;
            foreach(Enemy enemy in potentialTargets)
            {
                if(currentTargets.Contains(enemy)) { continue; }
                if(closestEnemy == null) { closestEnemy = enemy; }
                if(Vector3.Distance(transform.position, enemy.transform.position) < Vector3.Distance(transform.position, closestEnemy.transform.position))
                {
                    closestEnemy = enemy;
                }
            }
            return closestEnemy;
        }

        private void AttackTargets()
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= 1f / attacksPerSecond)
            {
                if(currentTargets.Count == 0) { return; }
                foreach(Enemy target in currentTargets)
                {
                    Projectile projectileInstance = Instantiate(projectile, transform.position, Quaternion.identity, projectileParent);
                    float processedDamage = GetComponent<DamageModifier>().CalculateDamageForProjectile
                        (damage, GetComponent<UISelectionDescription>().GetMyType(), target.GetComponent<UISelectionDescription>().GetMyType());
                    projectileInstance.SetTarget(target);
                    projectileInstance.SetDamage(processedDamage);
                    projectileInstance.SetParentType(GetComponent<UISelectionDescription>().GetMyType());
                    projectileInstance.SetAttacker(GetComponent<DamageModifier>());
                    attackTimer = 0f;
                }
            }
        }

        [ExecuteAlways]
        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, range);
        }
    }
}
