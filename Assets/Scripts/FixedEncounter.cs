using UnityEngine;

public class FixedEncounter : MonoBehaviour
{
    [Header("Encounter")]
    public BattleManager battleManager;
    public Battler enemyBattler;

    private bool encounterStarted;

    public void StartEncounter()
    {
        if (encounterStarted)
        {
            return;
        }

        encounterStarted = true;

        Debug.Log("You have encountered: " + enemyBattler.gameObject.name);

        battleManager.BeginEncounter(enemyBattler);
    }
}
