using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    #region Instance
    private static CountDown instance;

    public static CountDown GetInstance()
    {
        return instance;
    }
    public void Awake()
    {
        instance = this;
    }
    #endregion
    public int countdownTime;

    public IEnumerator CountDownToStart()
    {
        GetComponentInChildren<Text>().enabled = true;
        while (countdownTime > 0)
        {
            GetComponentInChildren<Text>().text = countdownTime.ToString();

            yield return new WaitForSeconds(1f);

            countdownTime--;
        }

        GetComponentInChildren<Text>().text = "Go!";

        yield return new WaitForSeconds(1f);

        GetComponentInChildren<Text>().enabled = false;

    }
}
