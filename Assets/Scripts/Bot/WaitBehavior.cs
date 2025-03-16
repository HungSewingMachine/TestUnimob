using UnityEngine;
using FSMC.Runtime;
using System;
using Entity;
using Object = UnityEngine.Object;

[Serializable]
public class WaitBehavior : FSMC_Behaviour
{
    private BotController bot;
    private BotStateDisplayer displayer;
    
    public override void StateInit(FSMC_Controller stateMachine, FSMC_Executer executer)
    {
    
    }
    public override void OnStateEnter(FSMC_Controller stateMachine, FSMC_Executer executer)
    {
        bot = executer.GetComponent<BotController>();
        displayer = executer.GetComponent<BotStateDisplayer>();
        bot.OnFruitChanged += displayer.DisplayText;
    }

    public override void OnStateUpdate(FSMC_Controller stateMachine, FSMC_Executer executer)
    {
        if (bot.IsFull)
        {
            executer.SetTrigger("ToQueue");
        }
    }

    public override void OnStateExit(FSMC_Controller stateMachine, FSMC_Executer executer)
    {
        bot.OnFruitChanged -= displayer.DisplayText;
        bot.UnregisterTable();
    }
}