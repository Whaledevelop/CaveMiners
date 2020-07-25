using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.U2D.Animation;

public class ActionsTest : MonoBehaviour
{
    public Animator animator;

    public CharacterToolsManager toolsManager;

    private bool isDigging;
    public void Dig()
    {
        isDigging = !isDigging;
        animator.SetTrigger(isDigging ? "StartDigging" : "StopDigging");
        toolsManager.ChangeToolMode(ToolCode.Shovel, isDigging);        
    }

    private bool isMining;
    public void Mine()
    {
        isMining = !isMining;
        animator.SetTrigger(isMining ? "StartMining" : "StopMining");
        toolsManager.ChangeToolMode(ToolCode.Pick, isMining);
    }

    private bool isWithBag;
    public void TakeBag()
    {
        isWithBag = !isWithBag;
        toolsManager.ChangeToolMode(ToolCode.Bag, isWithBag);
    }

    private bool isWalking;
    public void Walk()
    {
        isWalking = !isWalking;
        animator.SetTrigger(isWalking ? "StartWalking" : "StopWalking");
    }
}
