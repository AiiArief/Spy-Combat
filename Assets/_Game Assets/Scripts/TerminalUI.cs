using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

namespace SpyCombat.Menuscreen
{
    [System.Serializable]
    struct ClampParsedCommand
    {
        public int min;
        public int max;
    }

    enum Programs
    {
        None,
        PerfectNumberChecking,
        Hacking
    }

    public class TerminalUI : MonoBehaviour
    {
        [SerializeField] TMP_InputField m_inputField;
        [SerializeField] TMP_Text m_consoleText;

        Programs m_currentPrograms = Programs.None;

        #region Unity's Callback
        private void Update()
        {
            if (Input.GetKey(KeyCode.Return))
                SendCommand();
        }
        #endregion

        public void SendCommand()
        {
            if (string.IsNullOrWhiteSpace(m_inputField.text))
                return;

            string command = m_inputField.text;
            _Log(command);
            _ResetInputField();

            switch (m_currentPrograms)
            {
                case Programs.None:
                    int q = 0;
                    if (int.TryParse(command, out q))
                    {
                        _ProcessPerfectNumberChecking(q);
                        return;
                    }

                    if (command.Equals("oop"))
                    {
                        _ProcessReadingOOPEssay();
                        return;
                    }

                    if (command.Equals("hack"))
                    {
                        StartCoroutine(_Hacking());
                        return;
                    }

                    _Log("Command is not recognizable", true);
                    break;
                case Programs.PerfectNumberChecking:
                    int n = 0;
                    if (int.TryParse(command, out n))
                        _ProcessPerfectNumberChecking(n);
                    else
                        _ExitPerfectNumberCheck("Unable to parse, quitting perfect number checking...");
                    break;
            }
        }

        private void _ResetInputField()
        {
            m_inputField.text = "";
            m_inputField.Select();
            m_inputField.ActivateInputField();
        }

        private void _Log(string log, bool isQuitting = false)
        {
            m_consoleText.text += log + "\n" + (isQuitting ? ">" : "");
        }

        #region Perfect Number Checking
        [SerializeField] ClampParsedCommand m_clampInputCount;
        [SerializeField] ClampParsedCommand m_clampCheckNumber;
        int[] m_checkedNumbers;
        int m_checkedNumber_index = 0;
        private void _ProcessPerfectNumberChecking(int parsedCommand)
        {
            switch (m_currentPrograms == Programs.PerfectNumberChecking)
            {
                case false:
                    if (!(parsedCommand >= m_clampInputCount.min && parsedCommand <= m_clampInputCount.max))
                    {
                        _ExitPerfectNumberCheck("Q value should be between " + m_clampInputCount.min + " & " + m_clampInputCount.max);
                        return;
                    }

                    m_checkedNumbers = new int[parsedCommand];
                    m_checkedNumber_index = 0;

                    m_currentPrograms = Programs.PerfectNumberChecking;
                    break;
                case true:
                    if (!(parsedCommand >= m_clampCheckNumber.min && parsedCommand <= m_clampCheckNumber.max))
                    {
                        _ExitPerfectNumberCheck("Input between " + m_clampCheckNumber.min + " & " + m_clampCheckNumber.max + "...");
                        return;
                    }

                    m_checkedNumbers[m_checkedNumber_index] = parsedCommand;
                    m_checkedNumber_index++;

                    if (m_checkedNumber_index < m_checkedNumbers.Length)
                        return;

                    for (int i = 0; i < m_checkedNumbers.Length; i++)
                    {
                        _Log(_PerfectNumberCheck(m_checkedNumbers[i]), i + 1 >= m_checkedNumbers.Length);
                    }

                    _ExitPerfectNumberCheck();
                    break;
            }

        }

        private string _PerfectNumberCheck(int number)
        {
            int sum = 0;
            for (int i = 1; i < number; i++)
            {
                if (number % i == 0) sum += i;
            }

            if (sum == number)
                return number + " is a perfect number.";

            if (sum == number - 1)
                return number + " is a almost perfect number.";

            return number + " is not a perfect number.";
        }

        private void _ExitPerfectNumberCheck(string log = "")
        {
            if (!string.IsNullOrEmpty(log))
                _Log(log, true);

            m_currentPrograms = Programs.None;
        }
        #endregion

        #region OOP
        [SerializeField] TextAsset m_oopEssayTXT;
        private void _ProcessReadingOOPEssay()
        {
            if (!m_oopEssayTXT)
            {
                _Log("Unable to load Essay file, quitting...", true);
                return;
            }

            _Log(m_oopEssayTXT.text, true);
        }
        #endregion

        #region Hack
        private IEnumerator _Hacking()
        {
            m_currentPrograms = Programs.Hacking;

            _Log("Start hacking, please wait...");
            yield return new WaitForSeconds(3.0f);

            _Log("Hacking failed succesfully!");
            yield return new WaitForSeconds(1.0f);

            _Log("Calling guards, closing all access door and intruder will be trapped in this room.");
            yield return new WaitForSeconds(1.0f);

            _Log("Thanks for using our service! ^^");
            yield return new WaitForSeconds(1.0f);

            GameUI.Instance.LoadScene("Gameplay");
        }
        #endregion
    }
}
