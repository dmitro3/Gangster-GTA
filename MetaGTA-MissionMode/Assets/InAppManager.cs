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

        if (EvmosManager.Instance)
        {
            EvmosManager.Instance.CheckUserBalance();
        }
        SetBalanceText();
    }

    [SerializeField] TMP_Text balanceText;
    public void SetBalanceText()
    {
        balanceText.text = "Balance : " + EvmosManager.userBalance.ToString();
    }
    public void purchaseCoins(int index)
    {
        EvmosManager.Instance.CoinBuyOnSendContract(index);
    }

    public void ExchangeCoins(int index)
    {
        int tokenBalance = System.Int32.Parse(SingletonDataManager.userTokenBalance);
        if (tokenBalance >= index)
        {
            EvmosManager.Instance.ExchangeTokenUI(index);
        }
        else
        {
            MessaeBox.insta.showMsg("Not Enough Tokens to exchange", true);
        }
    }

}
