using UnityEngine;
using FSMC.Runtime;
using System;
using Entity;

[Serializable]
public class FinishBehavior : FSMC_Behaviour
{
    private Transform myTransform;
    private BotController bot;
    
    public override void StateInit(FSMC_Controller stateMachine, FSMC_Executer executer)
    {
    
    }
    public override void OnStateEnter(FSMC_Controller stateMachine, FSMC_Executer executer)
    {
        bot = executer.GetComponent<BotController>();
        myTransform = executer.transform;
    }

    public override void OnStateUpdate(FSMC_Controller stateMachine, FSMC_Executer executer)
    {
        var distance = myTransform.position - bot.TargetPosition;
        if (distance.magnitude < 0.5f)
        {
            executer.SetTrigger("IsRespawn");
        }
    }

    public override void OnStateExit(FSMC_Controller stateMachine, FSMC_Executer executer)
    {
    
    }
}