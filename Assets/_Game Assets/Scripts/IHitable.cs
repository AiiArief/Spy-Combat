using System.Collections;
using UnityEngine;

namespace SpyCombat.Gameplay
{
    public interface IHitable
    {
        public void HitByBullet(BulletController bullet);
    }
}