using System.Collections;
using UnityEngine;

namespace SpyCombat.Gameplay
{
    public class PowerUpController_SpeedMultiplier : AbstractPowerUpController
    {
        [SerializeField]
        float m_speedMultiplier = 2.0f;
        public float SpeedMultiplier { get => m_speedMultiplier; }

        public override void PickUp(EntityController_Player player)
        {
            _AddTimedPowerUp(this, player, 5.0f);
        }
    }
}