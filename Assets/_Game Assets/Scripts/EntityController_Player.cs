using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SpyCombat.Gameplay
{
    public class EntityController_Player : AbstractEntityDynamicController
    {
        public static EntityController_Player Instance { get; private set; }

        [Header("Movement : Player")]
        [Tooltip("Speed multiplier when sprinting")]
        public float sprintMultiplier = 2.0f;

        #region Unity's Callback
        private void Awake()
        {
            Instance = this;
        }

        protected override void Update()
        {
            base.Update();
            _HandleInteractInput();
        }

        private void OnTriggerEnter(Collider other)
        {
            var powerUp = other.GetComponent<AbstractPowerUpController>();
            if (powerUp)
            {
                powerUp.PickUp(this);
            }

            var waveTrigger = other.GetComponent<WaveController>();
            if (waveTrigger)
            {
                waveTrigger.StartWave();
            }

            var winTrigger = other.tag == "Win";
            if (winTrigger)
            {
                GameUI.Instance.WinScene();
            }
        }
        #endregion

        public override void HitByBullet(BulletController bullet)
        {
            CurrentHealth = Mathf.Max(0, CurrentHealth - bullet.damage);
            if (CurrentHealth <= 0)
            {
                SetIsAlive(false);
            }
        }

        public override void SetIsAlive(bool isAlive)
        {
            base.SetIsAlive(isAlive);

            if (!isAlive)
            {
                GameUI.Instance.ResetScene(3.0f);
            }
        }

        protected override void _HandleAttackInput()
        {
            bool fire = Input.GetButtonDown("Fire1");
            if (fire && CurrentWeapon)
            {
                CurrentWeapon.Fire(m_attackSpawn);
            }
        }

        protected override void _HandleMoveInput()
        {
            base._HandleMoveInput();

            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            bool sprint = Input.GetButton("Sprint") && ((CurrentWeapon) ? !CurrentWeapon.isFiring : true);

            Vector3 dir = new Vector3(horizontal, 0f, vertical).normalized;
            float currentSprintMultiplier = (sprint) ? sprintMultiplier : 1.0f;

            if (dir.magnitude >= 0.1f)
            {
                float targetAngleMove = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
                Vector3 movedir = Quaternion.Euler(0f, targetAngleMove, 0f) * Vector3.forward;
                Vector3 gravity = (CharacterController.isGrounded) ? Vector3.zero : ENTITY_GRAVITY * Time.deltaTime;
                Vector3 move = movedir.normalized * speed * currentSprintMultiplier * Time.deltaTime + gravity;
                CharacterController.Move(move);
            }
            else
            {
                Vector3 gravity = (CharacterController.isGrounded) ? Vector3.zero : ENTITY_GRAVITY * Time.deltaTime;
                CharacterController.Move(gravity);
            }

            Animator.SetFloat("moveRange", Mathf.RoundToInt(dir.magnitude) * currentSprintMultiplier);
        }

        protected override void _HandleLookInput()
        {
            Vector2 playerPos = Camera.main.WorldToViewportPoint(transform.position);
            Vector2 mousePosOnScreen = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            Vector2 rotDir = mousePosOnScreen - playerPos;
            float targetAngleRot = Mathf.Atan2(rotDir.x, rotDir.y) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            transform.rotation = Quaternion.Euler(0.0f, targetAngleRot, 0.0f);
        }

        private void _HandleInteractInput()
        {
            bool interact = Input.GetButtonDown("Interact");
            if (interact)
            {
                Collider[] colliders = Physics.OverlapSphere(transform.position, 1.0f, Physics.AllLayers, QueryTriggerInteraction.Collide);
                for (int i = 0; i < colliders.Length; i++)
                {
                    var weapon = colliders[i].GetComponent<WeaponController>();
                    if (weapon != null)
                    {
                        if (CurrentWeapon == null)
                        {
                            CurrentWeapon = weapon;
                            weapon.PickUp(m_weaponParent);
                        }
                        else
                        {
                            CurrentWeapon.Drop(new Vector3(transform.position.x, 0.0f, transform.position.z));

                            CurrentWeapon = weapon;
                            weapon.PickUp(m_weaponParent);
                        }
                        return;
                    }
                }
            }
        }
    }
}
