using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SpyCombat
{
    public class GameUI : MonoBehaviour
    {
        public static GameUI Instance { get; private set; }

        [SerializeField] Image m_transition;

        #region Unity's Callback
        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            } else
            {
                Destroy(gameObject);
            }
        }

        private void OnEnable()
        {
            StartCoroutine(_Transition(Color.black, new Color(0, 0, 0, 0), false));
        }
        #endregion

        public void LoadScene(string sceneName, float delay = 0.0f)
        {
            Action action = () => {
                SceneManager.LoadScene(sceneName);
                StartCoroutine(_Transition(Color.black, new Color(0, 0, 0, 0), false));
            };
            StartCoroutine(_Transition(new Color(0, 0, 0, 0), Color.black, true, delay, action));
        }

        public void ResetScene(float delay = 0.0f)
        {
            LoadScene(SceneManager.GetActiveScene().name, delay);
        }

        public void WinScene()
        {
            Time.timeScale = 0.0f;
            Action action = () => Application.Quit();
            StartCoroutine(_Transition(new Color(1, 1, 1, 0), Color.white, true, 0.0f, action));
        }

        private IEnumerator _Transition(Color fromColor, Color toColor, bool transitionActiveAfter, float delay = 0.0f, Action afterAction = null)
        {
            if (delay > 0.0f)
                yield return new WaitForSecondsRealtime(delay);

            m_transition.gameObject.SetActive(true);
            m_transition.color = fromColor;

            float t = 0.0f;
            while(m_transition.color != toColor)
            {
                m_transition.color = Color.Lerp(fromColor, toColor, t);
                t += Time.unscaledDeltaTime;
                yield return null;
            }

            m_transition.gameObject.SetActive(transitionActiveAfter);

            if (afterAction != null)
                afterAction.Invoke();
        }
    }
}