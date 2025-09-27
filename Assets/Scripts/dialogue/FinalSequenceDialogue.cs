using DialogueSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FinalSequencceDialogue : DialogueSource
{
    [SerializeField] private Button headShot;
    [SerializeField] private GameObject textbox;

    protected override void OnEnable()
    {
        base.OnEnable();

        endDialogueEvent.AddListener(RevealHeadshot);
    }

    protected override void Awake()
    {
        base.Awake();

        headShot.gameObject.SetActive(false);
        textbox.SetActive(false);
    }

    void Start()
    {
        DialogueUI.instance.StartDialogue(this);
    }

    public void RevealHeadshot()
    {
        headShot.gameObject.SetActive(true);
        textbox.SetActive(true);
        PlayerCamera.instance.LockCamera();
    } 
}