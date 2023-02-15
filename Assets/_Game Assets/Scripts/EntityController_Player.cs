using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SpyCombat.Gameplay
{
    public class ActivePowerUpHashSet<T> : HashSet<T> where T : AbstractPowerUpController { }
    public class EntityController_Player : AbstractEntityDynamicController
    {
        public static EntityController_Player Instance { get; private set; }

        public ActivePowerUpHashSet<AbstractPowerUpController> CurrentActivePowerup { get; private set; } = new ActivePowerUpHashSet<AbstractPowerUpController>();

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
            if(powerUp)
            {
                powerUp.PickUp(this);
            }

            var waveTrigger = other.GetComponent<WaveController>();
            if (waveTrigger)
            {
                waveTrigger.StartWave();
            }
        }
        #endregion

        public override void HitByBullet(BulletController bullet)
        {
            CurrentHealth = Mathf.Max(0, CurrentHealth - 1);
            if (CurrentHealth <= 0)
            {
                SetIsAlive(false);
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
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            bool sprint = Input.GetButton("Sprint") && ((CurrentWeapon) ? !CurrentWeapon.isFiring : true);

            Vector3 dir = new Vector3(horizontal, 0f, vertical).normalized;
            float currentSprintMultiplier = (sprint) ? sprintMultiplier : 1.0f;

            if (dir.magnitude >= 0.1f)
            {
                float targetAngleMove = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
                Vector3 movedir = Quaternion.Euler(0f, targetAngleMove, 0f) * Vector3.forward;
                CharacterController.Move(movedir.normalized * speed * currentSprintMultiplier * Time.deltaTime);
            }
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
            if(interact)
            {
                Collider[] colliders = Physics.OverlapSphere(transform.position, 1.0f, Physics.AllLayers, QueryTriggerInteraction.Collide);
                for(int i=0; i<colliders.Length; i++)
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

        private float _CalcSpeed(float currentSprintMultiplier)
        {
            AbstractPowerUpController powerUp;
            float speedMultiplier = 1;
            if(CurrentActivePowerup.TryGetValue(new PowerUpController_SpeedMultiplier(), out powerUp))
            {
                print("oy");
                speedMultiplier = (powerUp as PowerUpController_SpeedMultiplier).SpeedMultiplier;
            }

            return speed * currentSprintMultiplier * speedMultiplier;
        }
    }
}
