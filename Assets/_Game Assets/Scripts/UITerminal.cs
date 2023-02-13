using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public struct ClampParsedCommand
{
    public int min;
    public int max;
}

public class UITerminal : MonoBehaviour
{
    [SerializeField] TMP_InputField m_inputField;
    [SerializeField] TMP_Text m_consoleText;

    #region Unity's Event
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

        int q = 0;
        if (int.TryParse(command, out q))
        {
            _ProcessPerfectNumberChecking(q);
            return;
        } else if(m_isUsingPerfectNumberChecking)
        {
            m_isUsingPerfectNumberChecking = false;
            _Log("Unable to parse, quitting perfect number checking...", true);
            return;
        }

        // equals oop
        // equals hack
        _Log("Command is not recognizable", true);
    }

    private void _ResetInputField()
    {
        m_inputField.text = "";
        m_inputField.Select();
        m_inputField.ActivateInputField();
    }

    private void _Log(string log, bool isQuitting = false)
    {
        m_consoleText.text += log + "\n" + (isQuitting ? ">" : "") ;
    }

    // todo : pindah ke kelas baru
    [SerializeField] ClampParsedCommand m_clampInputCount;
    [SerializeField] ClampParsedCommand m_clampCheckNumber;
    bool m_isUsingPerfectNumberChecking = false;
    int[] m_checkedNumbers;
    int m_checkedNumber_index = 0;
    private void _ProcessPerfectNumberChecking(int parsedCommand)
    {
        switch(m_isUsingPerfectNumberChecking)
        {
            case false:
                if (!(parsedCommand >= m_clampInputCount.min && parsedCommand <= m_clampInputCount.max))
                {
                    _Log("Q value should be between" + m_clampInputCount.min + " & " + m_clampInputCount.max, true);
                    return;
                }

                m_checkedNumbers = new int[parsedCommand]; 
                m_checkedNumber_index = 0;

                m_isUsingPerfectNumberChecking = true;
                break;
            case true:
                if(m_clampCheckNumber.min>= parsedCommand && parsedCommand <= m_clampCheckNumber.max)
                {
                    _Log("Input between " + m_clampCheckNumber.min + " & " + m_clampCheckNumber.max + "...", true);
                    m_isUsingPerfectNumberChecking = false;
                }

                m_checkedNumbers[m_checkedNumber_index] = parsedCommand;
                m_checkedNumber_index++;

                if (m_checkedNumber_index < m_checkedNumbers.Length)
                    return;

                for(int i=0; i<m_checkedNumbers.Length; i++)
                {
                    _Log(_PerfectNumberCheck(m_checkedNumbers[i]), i+1 >= m_checkedNumbers.Length);
                }

                m_isUsingPerfectNumberChecking = false;
                break;
        }

    }

    private string _PerfectNumberCheck(int number)
    {
        int sum = 0;
        for(int i=1; i<number; i++)
        {
            if (number % i == 0) sum += i;
        }

        if(sum == number)
            return number + " is a perfect number.";

        if (sum == number - 1)
            return number + " is a almost perfect number.";

        return number + " is not a perfect number.";
    }
}
