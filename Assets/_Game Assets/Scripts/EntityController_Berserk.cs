using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpyCombat.Gameplay
{
    public class EntityController_Berserk : AbstractEntityDynamicController
    {
        public override void HitByBullet(BulletController bullet)
        {
            CurrentHealth = Mathf.Max(0, CurrentHealth - 1);
            if (CurrentHealth <= 0)
            {
                SetIsAlive(false);
            }
        }

        protected override void _HandleMoveInput()
        {
            EntityController_Player player = EntityController_Player.Instance;
            Vector3 dir = player.transform.position - transform.position;

            m_characterController.Move(dir.normalized * speed * Time.deltaTime);
        }

        protected override void _HandleAttackInput()
        {
            CurrentWeapon.Fire(m_attackSpawn);
        }

        protected override void _HandleLookInput()
        {
            EntityController_Player player = EntityController_Player.Instance;
            Vector3 dir = player.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));

            transform.rotation = Quaternion.Euler(0.0f, rotation.eulerAngles.y, 0.0f);
        }
    }
}
