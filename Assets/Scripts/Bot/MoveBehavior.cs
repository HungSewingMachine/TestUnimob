using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSMC.Runtime;
using System;
using Entity;
using Object = UnityEngine.Object;

[Serializable]
public class MoveBehavior : FSMC_Behaviour
{
    private Transform myTransform;
    private Table fruitTable;
    private BotController bot;

    public override void StateInit(FSMC_Controller stateMachine, FSMC_Executer executer)
    {
        
    }

    public override void OnStateEnter(FSMC_Controller stateMachine, FSMC_Executer executer)
    {
        fruitTable = Object.FindObjectOfType<Table>();
        bot = executer.GetComponent<BotController>();
        myTransform = executer.transform;
        
        bot.RegisterTable(fruitTable);
    }

    public override void OnStateUpdate(FSMC_Controller stateMachine, FSMC_Executer executer)
    {
        var distance = myTransform.position - bot.TargetPosition;
        if (distance.magnitude < 0.5f)
        {
            executer.SetTrigger("WaitForFruit");
        }
    }

    public override void OnStateExit(FSMC_Controller stateMachine, FSMC_Executer executer)
    {
        fruitTable.RegisterClient(bot);
    }
}