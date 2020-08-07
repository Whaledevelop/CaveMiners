using UnityEngine;
using System.Collections;
using UnityEngine.Experimental.U2D.Animation;

public class CharacterPreview : UIItem<string>
{
    [SerializeField] private GameObject characterPrefab;

    [SerializeField] private Camera previewCamera;

    private Animator animator;
    private CharacterToolsManager toolsManager;
    private SpriteResolver spriteResolver;
    private SpriteRenderer spriteRenderer;

    private CharacterStateData previewState;

    public Vector3 SpriteSize => spriteRenderer.size;

    public void Awake()
    {
        GameObject character = Instantiate(characterPrefab, transform);
        animator = character.GetComponent<Animator>();
        toolsManager = character.GetComponent<CharacterToolsManager>();
        spriteResolver = character.GetComponentInChildren<SpriteResolver>();
        spriteRenderer = character.GetComponentInChildren<SpriteRenderer>();

    }

    public RenderTexture GetPreviewTexture(int width, int height)
    {
        RenderTexture previewRenderTexture = new RenderTexture(width, height, 1);
        previewCamera.targetTexture = previewRenderTexture;
        return previewRenderTexture;
    }

    public override void Init(string setupData)
    {
        spriteResolver.SetCategoryAndLabel(setupData, spriteResolver.GetLabel());
    }

    public void PreviewState(CharacterStateSkillData stateSkill)
    {
        PreviewState(stateSkill.state);
    }

    public void PreviewState(CharacterStateData newState)
    {
        if (previewState != null)
            previewState.UpdateView(CharacterStateData.StateStage.End, animator, toolsManager);

        previewState = newState;
        previewState.UpdateView(CharacterStateData.StateStage.Start, animator, toolsManager);
    }
}
