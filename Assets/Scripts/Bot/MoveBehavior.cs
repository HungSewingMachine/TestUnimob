using UnityEngine;
using FSMC.Runtime;
using System;
using Entity;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

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
        var tables = Object.FindObjectsOfType<Table>();
        fruitTable = tables[Random.Range(0, tables.Length)];
        bot = executer.GetComponent<BotController>();
        myTransform = executer.transform;
        
        bot.Respawn();
        bot.RegisterTablePosition(fruitTable);
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