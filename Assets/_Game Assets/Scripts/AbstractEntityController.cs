using System.Collections;
using UnityEngine;

namespace SpyCombat.Gameplay
{
    public abstract class AbstractEntityController: MonoBehaviour
    {
        [Header("Health")]

        [SerializeField] 
        protected int m_defaultMaxHealth = 5; // ubah ke database
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

        public void SetIsAlive(bool isAlive)
        {
            gameObject.SetActive(isAlive);
            CurrentHealth = (isAlive) ? DefaultMaxHealth : 0;
        }
    }
}