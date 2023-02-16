using System.Collections;
using UnityEngine;

namespace SpyCombat.Gameplay
{
    public abstract class AbstractEntityController: MonoBehaviour
    {
        [Header("Health")]

        [SerializeField] 
        protected int m_defaultMaxHealth = 5;
        public int DefaultMaxHealth { get => m_defaultMaxHealth; }

        public int CurrentHealth { get; set; }

        protected virtual void OnEnable()
        {
            SetIsAlive(true);
        }

        public bool GetIsAlive()
        {
            return CurrentHealth > 0;
        }

        public virtual void SetIsAlive(bool isAlive)
        {
            gameObject.SetActive(isAlive);
            CurrentHealth = (isAlive) ? DefaultMaxHealth : 0;
        }
    }

    public abstract class AbstractEntityDynamicController : AbstractEntityController, IHitable
    {
        public static Vector3 ENTITY_GRAVITY = new Vector3(0.0f, -9.8f, 0.0f);

        [Header("Movement")]

        [SerializeField]
        protected CharacterController m_characterController;
        public CharacterController CharacterController { get => m_characterController; }

        [SerializeField]
        protected Animator m_animator;
        public Animator Animator { get => m_animator; }

        [Tooltip("Move speed (walking)")]
        public float speed = 3f;


        [Header("Weapon")]

        [SerializeField]
        WeaponController m_defaultWeaponPrefab;

        [SerializeField]
        protected Transform m_weaponParent;

        [SerializeField]
        protected Transform m_attackSpawn;

        public WeaponController CurrentWeapon { get; protected set; }

        protected virtual void Update()
        {
            if(GetIsAlive())
            {
                _HandleAttackInput();
                _HandleMoveInput();
                _HandleLookInput();
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _SetupWeapon();
        }

        public abstract void HitByBullet(BulletController bullet);

        public override void SetIsAlive(bool isAlive)
        {
            CurrentHealth = (isAlive) ? DefaultMaxHealth : 0;
            CharacterController.enabled = isAlive;
            Animator.SetBool("isDead", !isAlive);
            
            if(!isAlive)
            {
                if(CurrentWeapon)
                    CurrentWeapon.StopAllCoroutines();

                StartCoroutine(_DelaySetActive(false, 10.0f));
            }
        }

        protected virtual void _HandleAttackInput() { }

        protected virtual void _HandleMoveInput() { }

        protected virtual void _HandleLookInput() { }

        protected virtual void _SetupWeapon()
        {
            if (!m_defaultWeaponPrefab || CurrentWeapon)
                return;

            CurrentWeapon = Instantiate(m_defaultWeaponPrefab);
            CurrentWeapon.PickUp(m_weaponParent, true);
        }

        private IEnumerator _DelaySetActive(bool setActive, float time)
        {
            yield return new WaitForSeconds(time);
            gameObject.SetActive(setActive);
        }
    }
}