using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpyCombat.Gameplay
{
    public class EntityController_Boss : AbstractEntityDynamicController
    {
        [SerializeField] int m_currentPattern = 0;

        Vector3 m_targetRandomPos;
        [SerializeField] float m_changeRandomEveryTime = 2.0f;
        float m_randomWayTime = 0.0f;

        public override void HitByBullet(BulletController bullet)
        {
            CurrentHealth = Mathf.Max(0, CurrentHealth - bullet.damage);
            if (CurrentHealth <= 0)
            {
                SetIsAlive(false);
            }
        }

        protected override void _HandleMoveInput()
        {
            EntityController_Player player = EntityController_Player.Instance;
            Vector3 dir = Vector3.zero;
            Vector3 gravity = (CharacterController.isGrounded) ? Vector3.zero : ENTITY_GRAVITY * Time.deltaTime;
            switch (m_currentPattern)
            {
                case 0:
                    break;
                case 1:
                    if(m_randomWayTime <= 0.0f)
                    {
                        m_randomWayTime = m_changeRandomEveryTime;
                        m_targetRandomPos = new Vector3(transform.position.x + Random.Range(-5.0f, 5.0f), 0.0f, transform.position.z + Random.Range(-5.0f, 5.0f));
                    }

                    dir = m_targetRandomPos - transform.position;
                    m_randomWayTime -= Time.deltaTime;

                    CharacterController.Move(dir.normalized * speed * Time.deltaTime + gravity);
                    Animator.SetFloat("moveRange", Mathf.RoundToInt(dir.magnitude));
                    break;
                case 2:
                    dir = player.transform.position - transform.position;

                    CharacterController.Move(dir.normalized * speed * Time.deltaTime + gravity);
                    Animator.SetFloat("moveRange", Mathf.RoundToInt(dir.magnitude));
                    break;
            }
        }

        protected override void _HandleAttackInput()
        {
            if(m_currentPattern>0)
                CurrentWeapon.Fire(m_attackSpawn);
        }

        protected override void _HandleLookInput()
        {
            EntityController_Player player = EntityController_Player.Instance;
            switch (m_currentPattern)
            {
                case 0:
                    break;
                case 1:
                    transform.rotation = Quaternion.Euler(0.0f, transform.rotation.eulerAngles.y + 360.0f * Time.deltaTime, 0.0f);
                    break;
                case 2:
                    Vector3 dir = player.transform.position - transform.position;
                    Quaternion rotation = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));

                    transform.rotation = Quaternion.Euler(0.0f, rotation.eulerAngles.y, 0.0f);
                    break;
            }
        }
    }
}
