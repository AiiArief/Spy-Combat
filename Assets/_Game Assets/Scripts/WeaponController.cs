using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpyCombat.Gameplay
{
    public class WeaponController : MonoBehaviour
    {
        [SerializeField] BulletController m_bulletPrefab; // database

        [Tooltip("A click down contain how much bullet")]
        public int bulletBurstPerShot = 3; // databse

        [Tooltip("Randomize angle of shooting")]
        public float shotRecoil = 1.5f; // database

        [Tooltip("Wait for another bullet when burst shot")]
        public float bulletFiringRate = 0.15f; // database

        [Tooltip("How many clicks before empty")]
        public int maxShot = 7; // database

        [Tooltip("How many second to fill one shot")]
        public float shotCooldownTime = 1.0f; // database

        [SerializeField] Collider m_collider;

        public float currentShot { get; private set; } = 0.0f;
        public bool isFiring { get; private set; } = false;

        private void LateUpdate()
        {
            currentShot = Mathf.Min(maxShot, currentShot + Time.deltaTime / shotCooldownTime);
        }

        public void PickUp(Transform gunParent)
        {
            m_collider.enabled = false;
            transform.SetParent(gunParent, false);
            transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.Euler(Vector3.zero));
        }

        public void Drop(Vector3 dropPos)
        {
            m_collider.enabled = true;
            transform.SetParent(null, false);
            transform.SetLocalPositionAndRotation(dropPos, Quaternion.Euler(Vector3.zero));
        }

        public void Fire(Transform bulletSpawn)
        {
            if (!isFiring && currentShot >= 1.0f)
            {
                StartCoroutine(_IsFiringCoroutine(bulletSpawn));
            }
        }

        private IEnumerator _IsFiringCoroutine(Transform bulletSpawn)
        {
            isFiring = true;
            currentShot = Mathf.Max(0, currentShot - 1);
            for (int i = 0; i < bulletBurstPerShot; i++)
            {
                Quaternion randRot = Quaternion.Euler(0.0f, bulletSpawn.rotation.eulerAngles.y + Random.Range(-shotRecoil, shotRecoil) * i, 0.0f);

                BulletController bullet = Instantiate(m_bulletPrefab, bulletSpawn.position, Quaternion.identity);
                bullet.InitializeBullet(randRot * Vector3.forward);

                yield return new WaitForSeconds(bulletFiringRate);
            }
            isFiring = false;
        }
    }
}
