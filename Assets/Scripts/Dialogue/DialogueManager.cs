using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    private static DialogueManager _instance;
    public static DialogueManager Instance { get => _instance; }

    [Header("Dialogue Object")]
    public DialogueInfo dialogue;

    [Header("UI Elements")]
    public GameObject dialogueUI;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI characterNameText;
    public Image rightCharacterPortrait;
    public Image leftCharacterPortrait;
    public Button[] choiceButtons;

    private Queue<DialogueInfo.Sentence> _sentences;

    private bool _isWriting = false;
    private bool _waitingForChoice = false;
    private bool _isActive = false;
    private DialogueInfo.Sentence _currentSentence;

    [Header("1 - very slow, 10 - very fast")]
    [SerializeField] private float _textSpeed = 5f;

    //for testing
    public UnityEvent OnReset;

    private void Awake()
    {
        dialogueUI.SetActive(false);

        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Start()
    {
        _sentences = new Queue<DialogueInfo.Sentence>();
        StartDialogue(dialogue);
    }

    public void Update()
    {
        if (!_waitingForChoice && _isActive && Input.GetMouseButtonDown(0))
        {
            DisplayNextSentence();
        }
    }

    public void StartDialogue(DialogueInfo dialogue)
    {
        dialogueUI.SetActive(true);

        _sentences.Clear();

        foreach (DialogueInfo.Sentence node in dialogue.sentences)
        {
            _sentences.Enqueue(node);
        }

        DisplayNextSentence();
        _isActive = true;
    }

    public void DisplayNextSentence()
    {
        if (_isWriting)
        {
            StopAllCoroutines();
            _isWriting = false;
            dialogueText.text = _currentSentence.text;
            return;
        }

        if (_sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        _currentSentence = _sentences.Dequeue();

        characterNameText.text = _currentSentence.character.characterName;
        if (_currentSentence.character.isLeftAligned)
        {
            characterNameText.alignment = TextAlignmentOptions.TopLeft;
            dialogueText.alignment = TextAlignmentOptions.TopLeft;
            leftCharacterPortrait.sprite = _currentSentence.character.sprite;
            leftCharacterPortrait.gameObject.SetActive(true);
            rightCharacterPortrait.gameObject.SetActive(false);
        }
        else
        {
            characterNameText.alignment = TextAlignmentOptions.TopRight;
            dialogueText.alignment = TextAlignmentOptions.TopRight;
            rightCharacterPortrait.sprite = _currentSentence.character.sprite;
            rightCharacterPortrait.gameObject.SetActive(true);
            leftCharacterPortrait.gameObject.SetActive(false);
        }

        if (_currentSentence.isChoice)
        {
            dialogueText.gameObject.SetActive(false);
            characterNameText.gameObject.SetActive(false);
            for (int i = 0; i < choiceButtons.Length; i++)
            {
                choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = _currentSentence.choices[i];
                choiceButtons[i].gameObject.SetActive(true);
            }
            _waitingForChoice = true;
            return;
        }
        
        StartCoroutine(TypeSentence(_currentSentence.text));
    }

    public void DisplayChoice(TextMeshProUGUI choice)
    {
        foreach (Button button in choiceButtons)
        {
            button.gameObject.SetActive(false);
        }
        dialogueText.gameObject.SetActive(true);
        characterNameText.gameObject.SetActive(true);
        _waitingForChoice = false;
        _currentSentence.text = choice.text;
        StartCoroutine(TypeSentence(choice.text));
    }

    IEnumerator TypeSentence(string sentence)
    {
        _isWriting = true;
        dialogueText.text = "";
        foreach (char character in sentence.ToCharArray())
        {
            dialogueText.text += character;
            yield return new WaitForSeconds(0.1f / _textSpeed);
        }
        _isWriting = false;
    }

    void EndDialogue()
    {
        _isActive = false;
        dialogueUI.SetActive(false);

        //for testing
        OnReset?.Invoke();
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene(1);
    }
}
