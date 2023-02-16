/// ----------------------------------------------------------------------
/// <summary>
///     Database for weapon
/// </summary>
/// <author>Narendra Arief Nugraha</author>
/// ----------------------------------------------------------------------
using System.Collections;
using UnityEngine;

namespace SpyCombat.Gameplay
{
    public struct ShotRecoilClamp
    {
        public static float RECOIL_MIN = -10.0f;
        public static float RECOIL_MAX = 10.0f;
    }

    [CreateAssetMenu(menuName = "Database/Weapon")]
    public class WeaponScriptable : ScriptableObject
    {
        [Tooltip("ID for bullet in object pooler")]
        public int bulletDatabaseID = 0;

        [Tooltip("A click down contain how much bullet")]
        public int bulletBurstPerShot = 3;

        [Tooltip("Randomize angle of shooting. Check ShootRecoilClamp to see min and max of this value")]
        public float shotRecoil = 1.5f;

        [Tooltip("Wait for another bullet when burst shot")]
        public float bulletFiringRate = 0.15f;

        [Tooltip("How many clicks before empty")]
        public int maxShot = 7;

        [Tooltip("How many second to fill one shot")]
        public float shotCooldownTime = 1.0f;
    }
}