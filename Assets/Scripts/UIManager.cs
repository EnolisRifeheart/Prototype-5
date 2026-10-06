using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public BattleManager bM;

    private bool bmActive;

    [Header("Battle Menu")]
    public GameObject battleMenuPanel;

    public enum BattleMenuType
    {
        Attack,
        Skill,
        Items,
        Guard
    }

    public BattleMenuType bmOption = BattleMenuType.Attack;

    [Header("Cursor")]
    public RectTransform cursorRect;

    [Header("Buttons")]
    public Button attackButton;
    public Button skillButton;
    public Button itemButton;
    public Button guardButton;

    private void Start()
    {
        // Find Battle Manager automatically.
        if (bM == null)
        {
            bM = FindFirstObjectByType<BattleManager>();
        }

        // Buttons call the same battle commands as keyboard input.
        attackButton.onClick.AddListener(SelectAttack);
        skillButton.onClick.AddListener(SelectSkill);
        itemButton.onClick.AddListener(SelectItems);
        guardButton.onClick.AddListener(SelectGuard);

        HideBM();
    }

    private void Update()
    {
        if (bmActive)
        {
            BMInput();
        }
    }

    public void DisplayBM()
    {
        bmActive = true;

        battleMenuPanel.SetActive(true);

        bmOption = BattleMenuType.Attack;

        UpdateCursorPosition();
    }

    public void HideBM()
    {
        bmActive = false;

        battleMenuPanel.SetActive(false);
    }

    private void BMInput()
    {
        if (Keyboard.current == null)
        {
            return;
        }

        // Move down through the battle menu.
        if (Keyboard.current.downArrowKey.wasPressedThisFrame)
        {
            if (bmOption == BattleMenuType.Attack)
                bmOption = BattleMenuType.Skill;

            else if (bmOption == BattleMenuType.Skill)
                bmOption = BattleMenuType.Items;

            else if (bmOption == BattleMenuType.Items)
                bmOption = BattleMenuType.Guard;

            else if (bmOption == BattleMenuType.Guard)
                bmOption = BattleMenuType.Attack;

            UpdateCursorPosition();
        }

        // Move up through the battle menu.
        if (Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            if (bmOption == BattleMenuType.Attack)
                bmOption = BattleMenuType.Guard;

            else if (bmOption == BattleMenuType.Guard)
                bmOption = BattleMenuType.Items;

            else if (bmOption == BattleMenuType.Items)
                bmOption = BattleMenuType.Skill;

            else if (bmOption == BattleMenuType.Skill)
                bmOption = BattleMenuType.Attack;

            UpdateCursorPosition();
        }

        // Select current battle command.
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            SelectCurrentOption();
        }
    }

    private void SelectCurrentOption()
    {
        switch (bmOption)
        {
            case BattleMenuType.Attack:
                SelectAttack();
                break;

            case BattleMenuType.Skill:
                SelectSkill();
                break;

            case BattleMenuType.Items:
                SelectItems();
                break;

            case BattleMenuType.Guard:
                SelectGuard();
                break;
        }
    }

    private void UpdateCursorPosition()
    {
        if (cursorRect == null)
        {
            return;
        }

        float leftOffset = 40f;

        switch (bmOption)
        {
            case BattleMenuType.Attack:
                MoveCursor(attackButton, leftOffset);
                break;

            case BattleMenuType.Skill:
                MoveCursor(skillButton, leftOffset);
                break;

            case BattleMenuType.Items:
                MoveCursor(itemButton, leftOffset);
                break;

            case BattleMenuType.Guard:
                MoveCursor(guardButton, leftOffset);
                break;
        }
    }

    private void MoveCursor(Button button, float leftOffset)
    {
        RectTransform buttonRect =
            button.GetComponent<RectTransform>();

        cursorRect.anchoredPosition =
            buttonRect.anchoredPosition +
            new Vector2(-leftOffset, 0f);
    }

    public void SelectAttack()
    {
        if (!bmActive)
        {
            return;
        }

        HideBM();

        bM.Attack();
    }

    public void SelectSkill()
    {
        if (!bmActive)
        {
            return;
        }

        bM.Skill();
    }

    public void SelectItems()
    {
        if (!bmActive)
        {
            return;
        }

        bM.Items();
    }

    public void SelectGuard()
    {
        if (!bmActive)
        {
            return;
        }

        HideBM();

        bM.Guard();
    }
}