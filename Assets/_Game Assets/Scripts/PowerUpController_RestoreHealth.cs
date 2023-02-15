using System.Collections;
using UnityEngine;

namespace SpyCombat.Gameplay
{
    public class PowerUpController_RestoreHealth : AbstractPowerUpController
    {
        public override void PickUp(EntityController_Player player)
        {
            player.CurrentHealth = player.DefaultMaxHealth;
            Destroy(gameObject);
        }
    }
}