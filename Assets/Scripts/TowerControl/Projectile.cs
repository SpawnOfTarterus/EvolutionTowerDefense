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
            MoveAtTarget();
        }

        private void MoveAtTarget()
        {
            Vector3 targetPos = target.GetHitTransform().position;
            transform.LookAt(targetPos);
            transform.position += transform.forward * Time.deltaTime * moveSpeed;
        }

        private void OnTriggerEnter(Collider other)
        {
            other.GetComponent<Health>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
