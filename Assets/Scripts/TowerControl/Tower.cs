using ETD.EnemyControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETD.TowerControl
{
    public class Tower : MonoBehaviour
    {
        [SerializeField] int damage;
        [SerializeField] float range;
        [SerializeField] int cost;
        [SerializeField] float timeBetweenAttacks;
        [SerializeField] bool isBuilt = false;
        [SerializeField] Projectile projectile;

        Enemy target;
        float attackTimer = Mathf.Infinity;
        TowerSpawner mySpawner = null;

        public int GetCost()
        {
            return cost;
        }

        public void SetSpawner(TowerSpawner newSpawner)
        {
            mySpawner = newSpawner;
        }

        public void SetAsBuilt()
        {
            isBuilt = true;
        }

        private void Update()
        {
            AttackMechanic();
        }


        private void OnTriggerStay(Collider other)
        {
            if(isBuilt) { return; }
            Debug.Log("Collision in progress.");
            if (other.GetComponent<Tower>() || other.GetComponent<Enemy>())
            {
                mySpawner.ToggleColorBuildIndicator(false);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if(isBuilt) { return; }
            Debug.Log("exit collision.");
            if (other.GetComponent<Tower>() || other.GetComponent<Enemy>())
            {
                mySpawner.ToggleColorBuildIndicator(true);
            }
        }

        private void AttackMechanic()
        {
            GetTarget();
            if (!IsTargetInRange()) { return; }
            AttackTarget();
        }

        private void GetTarget()
        {
            Enemy[] enemies = FindObjectsOfType<Enemy>();
            foreach (Enemy enemy in enemies)
            {
                if(Vector3.Distance(enemy.transform.position, transform.position) > range) { continue; }
                if(target != null)
                {
                    if(Vector3.Distance(enemy.transform.position, transform.position) < 
                        Vector3.Distance(enemy.transform.position, target.transform.position))
                    {
                        target = enemy;
                    }
                }
                else
                {
                    target = enemy;
                }
            }
        }

        private bool IsTargetInRange()
        {
            if (target == null) { return false; }
            if (Vector3.Distance(target.transform.position, transform.position) > range)
            {
                target = null;
                return false;
            }
            return true;
        }

        private void AttackTarget()
        {
            attackTimer += Time.deltaTime;
            if(attackTimer >= timeBetweenAttacks)
            {
                Projectile projectileInstance = Instantiate(projectile, transform.position, Quaternion.identity);
                projectileInstance.SetTarget(target);
                projectileInstance.SetDamage(damage);
                attackTimer = 0f;
            }
        }

        [ExecuteAlways]
        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, range);
        }


    }
}
