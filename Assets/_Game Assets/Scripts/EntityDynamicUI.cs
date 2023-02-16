/// ----------------------------------------------------------------------
/// <summary>
///     UI for player controller in world space above player
/// </summary>
/// <author>Narendra Arief Nugraha</author>
/// ----------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpyCombat.Gameplay
{
    public class EntityDynamicUI : MonoBehaviour
    {
        [SerializeField] AbstractEntityDynamicController m_entity;

        [SerializeField] Slider m_healthBar;
        [SerializeField] Slider m_cooldownBar;

        Quaternion m_originalCamRotation;

        #region Unity's Callback
        private void Start()
        {
            m_originalCamRotation = transform.rotation;
        }

        private void Update()
        {
            m_healthBar.gameObject.SetActive(m_entity.GetIsAlive());
            m_cooldownBar.gameObject.SetActive(m_entity.GetIsAlive() && m_entity.CurrentWeapon);


            if (!m_entity.GetIsAlive())
                return;

            m_healthBar.value = m_entity.CurrentHealth * 1.0f / m_entity.DefaultMaxHealth;


            if (!m_entity.CurrentWeapon)
                return;

            m_cooldownBar.value = m_entity.CurrentWeapon.currentShot /m_entity.CurrentWeapon.weaponScriptable.maxShot;
        }

        private void LateUpdate()
        {
            transform.rotation = Camera.main.transform.rotation * m_originalCamRotation;
        }
        #endregion
    }
}
