using UnityEngine;
using System.Collections;
using UnityEngine.Experimental.U2D.Animation;

public class CharacterPreviewUIItem : UIItem<string>
{
    [SerializeField] private GameObject characterPrefab;

    [SerializeField] private Camera previewCamera;

    private Animator animator;
    private CharacterToolsManager toolsManager;
    private SpriteResolver spriteResolver;
    private SpriteRenderer spriteRenderer;

    private CharacterStateData previewState;

    [HideInInspector] public RenderTexture PreviewRenderTexture;

    public Vector3 SpriteSize => spriteRenderer.size;

    [HideInInspector] public string SpriteName;

    public void Awake()
    {
        GameObject character = Instantiate(characterPrefab, transform);
        animator = character.GetComponent<Animator>();
        toolsManager = character.GetComponent<CharacterToolsManager>();
        spriteResolver = character.GetComponentInChildren<SpriteResolver>();
        spriteRenderer = character.GetComponentInChildren<SpriteRenderer>();

    }

    public void GeneratePreviewTexture(int width, int height)
    {
        PreviewRenderTexture = new RenderTexture(width, height, 1);
        previewCamera.gameObject.SetActive(true);
        previewCamera.targetTexture = PreviewRenderTexture;
    }

    public override void Init(string setupData)
    {
        SpriteName = setupData;
        spriteResolver.SetCategoryAndLabel(setupData, spriteResolver.GetLabel());
    }

    public void StartPreviewState(CharacterStateData newState)
    {
        StopPreviewState();

        previewState = newState;
        
        previewState.UpdateView(CharacterStateData.StateStage.Start, animator, toolsManager);
    }

    public void StopPreviewState()
    {
        if (previewState != null)
        {            
            previewState.UpdateView(CharacterStateData.StateStage.End, animator, toolsManager);
            previewState = null;
        }
    }
}
