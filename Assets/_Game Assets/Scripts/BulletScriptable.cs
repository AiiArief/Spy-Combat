/// ----------------------------------------------------------------------
/// <summary>
///     Database for bullet
/// </summary>
/// <author>Narendra Arief Nugraha</author>
/// ----------------------------------------------------------------------
using System;
using System.Collections;
using UnityEngine;

namespace SpyCombat.Gameplay
{
    [CreateAssetMenu(menuName = "Database/Bullet")]
    public class BulletScriptable : ScriptableObject
    {
        [Tooltip("Bullet's life time before automatically disable")]
        public float lifeTime = 3.0f;

        [Tooltip("Bullet's move speed in meter / second")]
        public float bulletSpeed = 50.0f;

        [Tooltip("Bullet's damage")]
        public int damage = 1;
    }
}