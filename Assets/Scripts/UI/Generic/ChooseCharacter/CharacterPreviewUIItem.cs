using UnityEngine;
using System.Collections;
using UnityEngine.Experimental.U2D.Animation;

public class CharacterPreviewUIItem : UIItem<CharacterInitialData>
{
    [SerializeField] private CharacterManager characterPrefab;

    [SerializeField] private Camera previewCamera;

    [HideInInspector] public RenderTexture PreviewRenderTexture;
    [HideInInspector] public CharacterInitialData characterData;

    private Animator animator;
    private CharacterToolsManager toolsManager;
    private SpriteRenderer spriteRenderer;

    private CharacterState previewState;
    private CharacterManager character;

    public Vector3 SpriteSize => spriteRenderer.size;


    public void Awake()
    {
        character = Instantiate(characterPrefab, transform);
        animator = character.GetComponent<Animator>();
        toolsManager = character.GetComponentInChildren<CharacterToolsManager>();
        spriteRenderer = character.GetComponentInChildren<SpriteRenderer>();

    }

    public void GeneratePreviewTexture(int width, int height)
    {
        PreviewRenderTexture = new RenderTexture(width, height, 1);
        previewCamera.gameObject.SetActive(true);
        previewCamera.targetTexture = PreviewRenderTexture;
    }

    public override void Init(CharacterInitialData setupData)
    {
        characterData = setupData;
        character.Init(setupData);
    }

    public void StartPreviewState(CharacterState newState)
    {
        StopPreviewState();

        previewState = Instantiate(newState);

        previewState.InitInstance(animator, toolsManager);

        previewState.UpdateView(StateStage.Start);
    }

    public void StopPreviewState()
    {
        if (previewState != null)
        {
            previewState.UpdateView(StateStage.End);
            previewState = null;
        }
    }
}
