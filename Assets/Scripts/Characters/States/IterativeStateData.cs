using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "IterativeStateData", menuName = "States/IterativeStateData")]
public class IterativeStateData : CharacterStateData
{
    public CharacterActionGameEvent iterationEvent;
    public float iterationsInterval = 1;

    public override IEnumerator Execute(bool isPrevStateTheSame, CharacterActionData actionData, Animator animator, CharacterToolsManager toolsManager, Rotator rotator)
    {
        yield return base.Execute(isPrevStateTheSame, actionData, animator, toolsManager, rotator);
        if (iterationEvent != null)
        {
            while (true)
            {
                yield return new WaitForSeconds(iterationsInterval);
                iterationEvent.Raise(actionData);
            }
        }
        else
        {
            yield break;
        }
    }
}