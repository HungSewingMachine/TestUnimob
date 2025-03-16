using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSMC.Runtime;
using System;
using Entity;
using Object = UnityEngine.Object;

[Serializable]
public class GoToQueueBehavior : FSMC_Behaviour
{
    private BotController bot;
    
    public override void StateInit(FSMC_Controller stateMachine, FSMC_Executer executer)
    {
    
    }
    public override void OnStateEnter(FSMC_Controller stateMachine, FSMC_Executer executer)
    {
        bot = executer.GetComponent<BotController>();
        var cashier = Object.FindObjectOfType<Cashier>();
        
        cashier.AddCustomerToQueue(bot);
        
        var displayer = executer.GetComponent<BotStateDisplayer>();
        displayer.ShowCashier();
    }

    public override void OnStateUpdate(FSMC_Controller stateMachine, FSMC_Executer executer)
    {
        if (bot.HasPay)
        {
            executer.SetTrigger("HasPay");
        }
    }

    public override void OnStateExit(FSMC_Controller stateMachine, FSMC_Executer executer)
    {
        var target = GameConstant.SpawnPosition;
        bot.SetTarget(target, target);
    }
}