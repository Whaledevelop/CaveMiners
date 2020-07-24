using UnityEngine;


[CreateAssetMenu(fileName = "CharacterStateData", menuName = "ScriptableObjects/CharacterStateData")]
public class CharacterStateData : ScriptableObject
{
    public string stateName;
 
    public ToolCode toolCode;
    public LayerMask actionLayerMask;

    public int actionPriority;

    public Color gizmosColor;

    public string animatorTriggerStart;
    public string animatorTriggerEnd;
    public Vector2GameEvent startEvent;
    public GameEvent endEvent;


    public void OnStart(Animator animator, CharacterToolsManager toolsManager, Vector2 characterPosition, Vector2 stateActionPosition)
    {
        if (!string.IsNullOrEmpty(animatorTriggerStart))
            animator.SetTrigger(animatorTriggerStart);

        toolsManager.ApplyTool(toolCode);

        if (startEvent != null)
        {
            startEvent.Raise(stateActionPosition);
        }            
    }
    public void OnEnd(Animator animator, CharacterToolsManager toolsManager)
    {
        if (!string.IsNullOrEmpty(animatorTriggerStart))
            animator.SetTrigger(animatorTriggerStart);

        toolsManager.ApplyTool(toolCode);

        if (endEvent != null)
        {
            endEvent.Raise();
        }
    }

    #region Services

    public bool CompareActionMaskWithLayer(int layer)
    {
        return (int)Mathf.Log(actionLayerMask.value, 2) == layer;
    }

    #endregion
}