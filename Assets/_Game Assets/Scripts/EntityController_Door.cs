using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpyCombat.Gameplay
{
    public class EntityController_Door : AbstractEntityController, IHitable
    {
        public void HitByBullet(BulletController bullet)
        {
            CurrentHealth = Mathf.Max(0, CurrentHealth - bullet.damage);
            if (CurrentHealth <= 0)
            {
                SetIsAlive(false);
            }
        }
    }
}
