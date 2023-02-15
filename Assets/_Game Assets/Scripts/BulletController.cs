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
        [SerializeField] Rigidbody m_rigidbody;

        [Tooltip("Bullet's move speed in meter / second")]
        [SerializeField] float m_bulletSpeed = 50.0f;

        #region Unity's Callback
        private void OnCollisionEnter(Collision collision)
        {
            var hitable = collision.gameObject.GetComponent<IHitable>();
            if (hitable != null)
            {
                hitable.HitByBullet(this);
            }

            Destroy(gameObject); // ganti pooling
        }
        #endregion

        /// <summary>
        ///     Call this after instantiate to make this bullet works!
        /// </summary>
        /// <param name="originalDirection">Where are you going?</param>
        public void InitializeBullet(Vector3 originalDirection)
        {
            Destroy(gameObject, 3.0f);
            transform.forward = originalDirection;

            m_rigidbody.velocity = originalDirection * m_bulletSpeed;
        }
    }
}
