/// ----------------------------------------------------------------------
/// <summary>
///     Base class for weapon
/// </summary>
/// <author>Narendra Arief Nugraha</author>
/// ----------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpyCombat.Gameplay
{
    public class WeaponController : MonoBehaviour
    {
        [SerializeField] WeaponScriptable m_weaponScriptable;
        public WeaponScriptable weaponScriptable { get => m_weaponScriptable; }

        [SerializeField] Collider m_collider;

        public float currentShot { get; private set; } = 0.0f;
        public bool isFiring { get; private set; } = false;

        public int CurrentDamageMultiplier { get; set; } = 1;

        #region Unity's Callback
        private void LateUpdate()
        {
            _HandleWeaponRegen();
        }
        #endregion

        public void PickUp(Transform gunParent, bool full = false)
        {
            currentShot = (full) ? weaponScriptable.maxShot : currentShot;

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

        private void _HandleWeaponRegen()
        {
            currentShot = Mathf.Min(m_weaponScriptable.maxShot, currentShot + Time.deltaTime / m_weaponScriptable.shotCooldownTime);
        }

        private IEnumerator _IsFiringCoroutine(Transform bulletSpawn)
        {
            isFiring = true;
            currentShot = Mathf.Max(0, currentShot - 1);
            for (int i = 0; i < m_weaponScriptable.bulletBurstPerShot; i++)
            {
                float rand = Mathf.Clamp(Random.Range(-m_weaponScriptable.shotRecoil, m_weaponScriptable.shotRecoil) * i, ShotRecoilClamp.RECOIL_MIN, ShotRecoilClamp.RECOIL_MAX);
                Quaternion randRot = Quaternion.Euler(0.0f, bulletSpawn.rotation.eulerAngles.y + rand, 0.0f);

                BulletController bullet = BulletPooler.Instance.GetPooledObject(m_weaponScriptable.bulletDatabaseID);
                bullet.transform.SetPositionAndRotation(bulletSpawn.position, Quaternion.identity);
                bullet.gameObject.SetActive(true);
                bullet.InitializeBullet(randRot * Vector3.forward, CurrentDamageMultiplier);

                yield return new WaitForSeconds(m_weaponScriptable.bulletFiringRate);
            }
            isFiring = false;
        }
    }
}
