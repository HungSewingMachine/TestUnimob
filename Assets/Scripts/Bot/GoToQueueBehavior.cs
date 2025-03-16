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
    public override void StateInit(FSMC_Controller stateMachine, FSMC_Executer executer)
    {
    
    }
    public override void OnStateEnter(FSMC_Controller stateMachine, FSMC_Executer executer)
    {
        var bot = executer.GetComponent<BotController>();
        Object.FindObjectOfType<Cashier>().Enqueue(bot);
    }

    public override void OnStateUpdate(FSMC_Controller stateMachine, FSMC_Executer executer)
    {
    
    }

    public override void OnStateExit(FSMC_Controller stateMachine, FSMC_Executer executer)
    {
    
    }
}