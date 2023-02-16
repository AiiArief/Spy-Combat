/// ----------------------------------------------------------------------
/// <summary>
///     Bullet that instantiated by player on shoot
/// </summary>
/// <author>Narendra Arief Nugraha</author>
/// ----------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpyCombat.Gameplay
{
    public class BulletController : MonoBehaviour
    {
        [SerializeField] BulletScriptable m_bulletScriptable;
        public BulletScriptable bulletScriptable { get => m_bulletScriptable; }

        [SerializeField] Rigidbody m_rigidbody;

        public int damage { get; private set; }

        #region Unity's Callback
        private void OnTriggerEnter(Collider collision)
        {
            var hitable = collision.GetComponent<IHitable>();
            if (hitable != null)
            {
                hitable.HitByBullet(this);
                gameObject.SetActive(false);
                return;
            }

            var bullet = collision.GetComponent<BulletController>();
            if (bullet)
                return;

            gameObject.SetActive(false);
        }
        #endregion

        /// <summary>
        ///     Call this after instantiate to make this bullet works!
        /// </summary>
        /// <param name="originalDirection">Where are you going?</param>
        public void InitializeBullet(Vector3 originalDirection, int damageMultiplier = 1)
        {
            StartCoroutine(_Lifetime());
            transform.forward = originalDirection;

            m_rigidbody.velocity = originalDirection * m_bulletScriptable.bulletSpeed;

            damage = m_bulletScriptable.damage * damageMultiplier;
        }

        private IEnumerator _Lifetime()
        {
            yield return new WaitForSeconds(m_bulletScriptable.lifeTime);

            gameObject.SetActive(false);
        }
    }
}
