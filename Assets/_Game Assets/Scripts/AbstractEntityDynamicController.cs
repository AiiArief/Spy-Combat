using System.Collections;
using UnityEngine;

namespace SpyCombat.Gameplay
{
    [RequireComponent(typeof(Collider))]
    public abstract class AbstractEntityDynamicController: AbstractEntityController, IHitable
    {
        [Header("Movement")]

        [SerializeField]
        protected CharacterController m_characterController;
        public CharacterController CharacterController { get => m_characterController; }

        [Tooltip("Move speed (walking)")]
        public float speed = 3f; // pindah ke database


        [Header("Weapon")]

        [SerializeField]
        WeaponController m_defaultWeaponPrefab; // ubah ke database

        [SerializeField]
        protected Transform m_weaponParent;

        [SerializeField]
        protected Transform m_attackSpawn;

        public WeaponController CurrentWeapon { get; protected set; }

        protected virtual void Update()
        {
            _HandleAttackInput();
            _HandleMoveInput();
            _HandleLookInput();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _SetupWeapon();
        }

        public abstract void HitByBullet(BulletController bullet);

        protected virtual void _HandleAttackInput() { }

        protected virtual void _HandleMoveInput() { }

        protected virtual void _HandleLookInput() { }

        protected virtual void _SetupWeapon()
        {
            if (!m_defaultWeaponPrefab)
                return;

            CurrentWeapon = Instantiate(m_defaultWeaponPrefab);
            CurrentWeapon.PickUp(m_weaponParent);
        }
    }
}