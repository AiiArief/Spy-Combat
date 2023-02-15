using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpyCombat.Gameplay
{
    [System.Serializable]
    public class Wave
    {
        [SerializeField] Wave_Spawner[] m_waveSpawners;
        public Wave_Spawner[] waveSpawners { get => m_waveSpawners; }
    }

    [System.Serializable]
    public class Wave_Spawner
    {
        [SerializeField] AbstractEntityDynamicController m_enemyPrefab;
        public AbstractEntityDynamicController EnemyPrefab { get => m_enemyPrefab; }

        [SerializeField] Transform m_spawner;
        public Transform Spawner { get => m_spawner; }
    }

    public class WaveController : MonoBehaviour
    {
        [SerializeField] Collider m_collider;

        private bool m_isWaveStarted = false;

        [SerializeField] GameObject[] m_accessDoors;

        [SerializeField] Wave[] m_waves;
        int m_currentWaveIndex = 0;

        public void StartWave()
        {
            if (m_isWaveStarted)
                return;

            _SetDoorClosed(true);

            m_currentWaveIndex = 0;
            StartCoroutine(_HandleWave());
        }

        private void _SetDoorClosed(bool closed)
        {
            m_isWaveStarted = closed;

            foreach (GameObject door in m_accessDoors)
                door.SetActive(closed);
        }

        private IEnumerator _HandleWave()
        {
            yield return new WaitForSeconds(3.0f);

            Wave currentWave = m_waves[m_currentWaveIndex];
            AbstractEntityDynamicController[] checkedEnemies = new AbstractEntityDynamicController[currentWave.waveSpawners.Length];

            for(int i=0; i<currentWave.waveSpawners.Length; i++)
            {
                var enemy = Instantiate(currentWave.waveSpawners[i].EnemyPrefab, currentWave.waveSpawners[i].Spawner.position, currentWave.waveSpawners[i].Spawner.rotation);
                checkedEnemies[i] = enemy;
            }

            while(_CheckAllEnemiesAreAlive(checkedEnemies))
            {
                yield return null;
            }

            m_currentWaveIndex = Mathf.Min(m_waves.Length, m_currentWaveIndex + 1);
            if (m_currentWaveIndex < m_waves.Length)
            {
                StartCoroutine(_HandleWave());
                yield break;
            }

            m_collider.enabled = false;
            _SetDoorClosed(false);
        }

        private bool _CheckAllEnemiesAreAlive(AbstractEntityController[] enemies)
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                if (enemies[i].GetIsAlive())
                    return true;
            }

            return false;
        }
    }
}
