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
    private BotStateDisplayer displayer;

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
        
        displayer = executer.GetComponent<BotStateDisplayer>();
        displayer.ShowText();
        displayer.DisplayText(0, bot.MaxCapacity());
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
        bot.OnFruitChanged -= displayer.DisplayText;
        fruitTable.RegisterClient(bot);
    }
}