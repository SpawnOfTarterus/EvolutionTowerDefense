using ETD.EnemyControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETD.TowerControl
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float moveSpeed = 1f;

        int damage;
        Enemy target;

        public void SetDamage(int newDamage)
        {
            damage = newDamage;
        }

        public void SetTarget(Enemy newTarget)
        {
            target = newTarget;
        }

        private void Update()
        {
            SelfDestruct();
            MoveAtTarget();
        }

        private void MoveAtTarget()
        {
            if(target == null) { return; }
            Vector3 targetPos = target.GetHitTransform().position;
            transform.LookAt(targetPos);
            transform.position += transform.forward * Time.deltaTime * moveSpeed;
        }

        private void SelfDestruct()
        {
            if(target == null)
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            other.GetComponent<Health>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
