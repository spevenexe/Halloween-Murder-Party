using Accusation;
using DialogueSystem;
using UnityEngine;

public class NPC : DialogueSource
{
    [SerializeField] private bool isMonster;
    public bool IsMonster { get => isMonster; }
    public bool canRiskyCheck = false;
    public bool canSafeCheck = false;

    public DialogueCharacter character;

    [SerializeField] private DialogueData riskyCheckSuccessDialogue;
    [SerializeField] private DialogueData riskyCheckFailDialogue;
    [SerializeField] private DialogueData safeCheckDialogue; // the monster is predetermined, so there's no need for two of these

    protected override void Awake()
    {
        base.Awake();

        tag = "NPC";
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        OnHighlight.AddListener(RevealAccusePrompt);
        OnDehighlight.AddListener(HideAccusePrompt);

        startDialogueEvent.AddListener(AdjustInteractCount);
    }

    private void AdjustInteractCount()
    {
        if (!dialogueObject.HasBeenRead)
        {
            InteractionLimitManager.instance.DecreaseInteracts(dialogueObject.InteractionCost);
            dialogueObject.SetRead();
        }
    }

    private void RevealAccusePrompt()
    {
        if (dialogueObject != null && (canRiskyCheck || canSafeCheck))
            AccusationUI.instance.ShowAccuseUI(true, canSafeCheck);
    }

    private void HideAccusePrompt()
    {
        //Hide interaction UI
        AccusationUI.instance.ShowAccuseUI(false);
    }

    private void EvaluateRiskyCheck(PlayerData playerData = null)
    {
        DialogueUI.instance.StopDialogue();
        canRiskyCheck = false;
        canSafeCheck = false;

        // check the flags
        bool success;
        // TODO - Some calculation of aura
        AccusationManager.instance.CalculateAuraPenalty(this, out int auraDelta);
        success = auraDelta > 0;

        // load the accusation DialogueData if successful
        if (success && isMonster)
        {
            SetDialogue(riskyCheckSuccessDialogue);
        }
        // load the failure data if unsucessful
        else
        {
            SetDialogue(riskyCheckFailDialogue);
        }

        Activate();
    }

    private bool EvaluateSafeCheck(PlayerData playerData = null)
    {
        DialogueUI.instance.StopDialogue();
        canRiskyCheck = false;
        canSafeCheck = false;

        // load the dialogue
        SetDialogue(safeCheckDialogue);

        // return whether the character is a monster. This is probably not necessary, since the dialogue should tell you if its the monster
        return isMonster;
    }

    public void EvaluateCheck(PlayerData playerData = null)
    {
        if (canSafeCheck) EvaluateSafeCheck(playerData);
        else if (canRiskyCheck) EvaluateRiskyCheck(playerData);
    }

    protected override void ShowCurrentSentence()
    {
        base.ShowCurrentSentence();
        var sentence = dialogueObject.Sentences[currentSentence];
        switch (sentence.enableCheck)
        {
            case NPC_Centence.EnableCheck.Risky:
                Debug.Log("setting risky check");
                canRiskyCheck = true;
                break;
            case NPC_Centence.EnableCheck.Safe:
                Debug.Log("setting safe check");
                canSafeCheck = true;
                break;
            default:
                break;
        }
    }

    public override void StopDialogue()
    {
        base.StopDialogue();

        if (canSafeCheck || canRiskyCheck)
            AccusationUI.instance.ShowAccuseUI(true, canSafeCheck);
    }

    // combine with a unity event to make it work
    public void CompleteQuest() => canSafeCheck = true;
}