using System.Collections;
using UnityEngine;

namespace SpyCombat.Gameplay
{
    public class PowerUpController_IncreaseDamage : AbstractPowerUpController
    {
        public static int MAX_DAMAGE_MULTIPLIER = 4;

        public override void PickUp(EntityController_Player player)
        {
            if(player.CurrentWeapon)
                player.CurrentWeapon.CurrentDamageMultiplier = Mathf.Min(MAX_DAMAGE_MULTIPLIER, player.CurrentWeapon.CurrentDamageMultiplier + 1);

            Destroy(gameObject);
        }
    }
}