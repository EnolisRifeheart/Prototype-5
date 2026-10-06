using System.Collections;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [Header("UI")]
    public UIManager uiManager;
    [Header("Battlers")]
    public Battler playerBattler;
    private Battler enemyBattler;
    [Header("Player")]
    public Player playerController;

    [Header("Battle")]
    public bool battleStarted;

    private bool playerGuarding;

    public enum BattleState
    {
        Idle,
        PlayerTurn,
        EnemyTurn,
        Victory,
        Defeat
    }

    public BattleState currentBattleState = BattleState.Idle;

    private void Start()
    {
        ChangeBattleState(BattleState.Idle);
    }

    // Call this when the Player enters the scene.
    public void FindPlayer(Player player)
    {
        playerController = player;
        playerBattler = player.GetComponent<Battler>();

        Debug.Log("Player registered with Battle Manager.");
    }

    public bool BeginEncounter(Battler encounteredEnemy)
    {
        if (battleStarted)
        {
            return false;
        }

        if (playerBattler == null || playerController == null)
        {
            Debug.LogError("Battle cannot start. Player has not registered yet.");

            return false;
        }

        if (encounteredEnemy == null)
        {
            Debug.LogError("Battle cannot start. Enemy Battler is missing.");

            return false;
        }

        enemyBattler = encounteredEnemy;

        battleStarted = true;

        // Stopping dungeon movement during battle.
        playerController.enabled = false;

        Debug.Log("Battle Started against " + enemyBattler.gameObject.name);

        ChangeBattleState(BattleState.PlayerTurn);

        return true;
    }

    private void ChangeBattleState(BattleState newState)
    {
        currentBattleState = newState;

        switch (currentBattleState)
        {
            case BattleState.Idle:

                Debug.Log("Exploring Dungeon");

                uiManager.HideBM();

                break;

            case BattleState.PlayerTurn:

                Debug.Log("Player Turn");

                uiManager.DisplayBM();

                break;

            case BattleState.EnemyTurn:

                Debug.Log("Enemy Turn");

                uiManager.HideBM();

                break;

            case BattleState.Victory:

                Debug.Log("Victory");

                uiManager.HideBM();

                StartCoroutine(VictoryRoutine());

                break;

            case BattleState.Defeat:

                Debug.Log("Player Defeated");

                uiManager.HideBM();

                break;
        }
    }

    public void Attack()
    {
        if (currentBattleState != BattleState.PlayerTurn)
        {
            return;
        }

        if (enemyBattler == null)
        {
            return;
        }

        // Attack minus defense, with a minimum of one damage.
        int damage = Mathf.Max( playerBattler.stats.attackDamage - enemyBattler.stats.defense,1 );

        enemyBattler.TakeDamage(damage);

        if (enemyBattler.IsDead())
        {
            ChangeBattleState(BattleState.Victory);

            return;
        }

        StartCoroutine(EnemyTurnRoutine());
    }

    public void Skill()
    {
        if (currentBattleState != BattleState.PlayerTurn)
        {
            return;
        }

        Debug.Log("Skill selected.");

        // Skill submenu will open here later.
    }

    public void Items()
    {
        if (currentBattleState != BattleState.PlayerTurn)
        {
            return;
        }

        Debug.Log("Items selected.");

        // Item submenu will open here later.
    }

    public void Guard()
    {
        if (currentBattleState != BattleState.PlayerTurn)
        {
            return;
        }

        Debug.Log("Player is guarding.");

        playerGuarding = true;

        StartCoroutine(EnemyTurnRoutine());
    }

    private IEnumerator EnemyTurnRoutine()
    {
        ChangeBattleState(BattleState.EnemyTurn);

        // Small delay so the battle does not happen instantly.
        yield return new WaitForSeconds(1f);

        int playerDefense = playerBattler.stats.defense;

        // Guard doubles defense for the next enemy attack.
        if (playerGuarding)
        {
            playerDefense *= 2;
        }

        int damage = Mathf.Max( enemyBattler.stats.attackDamage - playerDefense, 1);

        playerBattler.TakeDamage(damage);

        // Guard only lasts for one enemy attack.
        playerGuarding = false;

        if (playerBattler.IsDead())
        {
            ChangeBattleState(BattleState.Defeat);

            yield break;
        }

        yield return new WaitForSeconds(1f);

        ChangeBattleState(BattleState.PlayerTurn);
    }

    private IEnumerator VictoryRoutine()
    {
        yield return new WaitForSeconds(1f);

        GameObject defeatedEnemy = enemyBattler.gameObject;

        enemyBattler = null;

        Destroy(defeatedEnemy);

        FinishBattle();
    }

    private void FinishBattle()
    {
        battleStarted = false;

        playerGuarding = false;

        // Return control to dungeon exploration.
        playerController.enabled = true;

        ChangeBattleState(BattleState.Idle);
    }
}