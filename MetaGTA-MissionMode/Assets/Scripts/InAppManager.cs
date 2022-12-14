using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InAppManager : MonoBehaviour
{
    public static InAppManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    async void OnEnable()
    {
        balanceText.text = "";

        if (BCCoreManager.Instance)
        {
            BCCoreManager.Instance.CheckUserBalance();
        }
        SetBalanceText();
    }

    [SerializeField] TMP_Text balanceText;
    public void SetBalanceText()
    {
        balanceText.text = "Balance : " + BCCoreManager.userBalance.ToString();
    }
    public void purchaseCoins(int index)
    {
        BCCoreManager.Instance.CoinBuyOnSendContract(index);
    }

    public void ExchangeCoins(int index)
    {
        int tokenBalance = System.Int32.Parse(BCCoreManager.userTokenBalance);
        if (tokenBalance >= index)
        {
            BCCoreManager.Instance.ExchangeToken(index);
        }
        else
        {
            MessaeBox.insta.showMsg("Not Enough Tokens to exchange", true);
        }
    }

}
