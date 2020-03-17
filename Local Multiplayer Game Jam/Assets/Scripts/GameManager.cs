using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header ("UI Object")]
    public GameObject endGameUI;

    [Header ("Players")]
    public PlayerHealth player1;
    public PlayerHealth player2;
    public PlayerHealth player3;
    public PlayerHealth player4;

    private List<PlayerHealth> healths = new List<PlayerHealth>();

    private void Awake()
    {
        // Add 4 players to list
        healths.Add(player1);
        healths.Add(player2);
        healths.Add(player3);
        healths.Add(player4);
    }

    private void Update()
    {
        // Remove player from list if their health is 0
        if (healths.Exists(p => p.currentLifeHealth <= 0))
            healths.Remove(healths.Find(p => p.currentLifeHealth <= 0));

        // If only one player remains, end game
        // TODO: Stop players from moving while the game has ended
        if (healths.Count <= 1)
            endGameUI.SetActive(true);
    }
}
