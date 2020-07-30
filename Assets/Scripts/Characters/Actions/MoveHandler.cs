using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MoveHandler : MonoBehaviour, IActionHandler
{
    [SerializeField] private CharacterTasksManager taskManager;

    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private float defaultSpeed;

    [HideInInspector] public float speed;

    private Vector2 moveEndPoint;
    private bool isMoving;    

    public void FixedUpdate()
    {
        if (isMoving)
        {
            Vector2 movement = Vector2.MoveTowards(rb.position, moveEndPoint, speed * Time.fixedDeltaTime);
            rb.MovePosition(movement);
        }
    }

    public IEnumerator WaitUntilEndPoint(CharacterActionData actionData)
    {
        yield return new WaitUntil(() => Vector2.Distance(rb.position, moveEndPoint) < 0.1);
        isMoving = false;
        actionData.OnExecute(EndExecutionCondition.Executed);
    }

    public void OnStartAction(CharacterActionData actionData)
    {        
        if (taskManager == actionData.taskManager)
        {
            if (isMoving)
            {
                StopAllCoroutines();
            }
            moveEndPoint = actionData.endPosition;
            speed = defaultSpeed * actionData.stateSkill;
            isMoving = true;
            StartCoroutine(WaitUntilEndPoint(actionData));
        }
    }

    public void OnEndAction(CharacterActionData actionData)
    {
        //throw new System.NotImplementedException();
    }
}

[CustomEditor(typeof(MoveHandler)), CanEditMultipleObjects]
public class MoveHandlerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.FloatField("CurrentSpeed", (target as MoveHandler).speed);
    }
}
