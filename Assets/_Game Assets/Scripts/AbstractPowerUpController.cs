using System.Collections;
using UnityEngine;

namespace SpyCombat.Gameplay
{
    public abstract class AbstractPowerUpController : MonoBehaviour // ipickupable?
    {
        [SerializeField]
        protected Collider m_collider;

        [SerializeField]
        protected Transform m_meshParent;

        #region Unity's Callback
        public override int GetHashCode()
        {
            return GetType().GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            return true;
        }
        #endregion

        public abstract void PickUp(EntityController_Player player);

        public IEnumerator TimedRemove(ActivePowerUpHashSet<AbstractPowerUpController> activePowerUps, float time)
        {
            yield return new WaitForSeconds(time);

            activePowerUps.Remove(this);
            Destroy(gameObject);
        }

        protected void _AddTimedPowerUp(AbstractPowerUpController newPow, EntityController_Player player, float time)
        {
            var activePowerup = player.CurrentActivePowerup;
            activePowerup.Add(newPow);
            newPow.StartCoroutine(TimedRemove(activePowerup, time));
            _DisableVisibility();
        }

        private void _DisableVisibility()
        {
            m_collider.enabled = false;
            m_meshParent.gameObject.SetActive(false);
        }
    }
}