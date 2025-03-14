using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSMC.Runtime;
using System;
using Entity;
using Object = UnityEngine.Object;

[Serializable]
public class WaitBehavior : FSMC_Behaviour
{
    private BotController bot;
    
    public override void StateInit(FSMC_Controller stateMachine, FSMC_Executer executer)
    {
    
    }
    public override void OnStateEnter(FSMC_Controller stateMachine, FSMC_Executer executer)
    {
        bot = executer.GetComponent<BotController>();
    }

    public override void OnStateUpdate(FSMC_Controller stateMachine, FSMC_Executer executer)
    {
        // check if stack full or not, full -> run to 0,0,0
        if (bot.IsFull)
        {
            executer.SetTrigger("IsFinish");
        }
    }

    public override void OnStateExit(FSMC_Controller stateMachine, FSMC_Executer executer)
    {
        bot.UnregisterTable();
        bot.targetPosition = new Vector3(-10, 0, 0);
    }
}