using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<GameObject> testPlayerList = new List<GameObject>();
    private List<Player> players = new List<Player>();
    public Helper helpers;
    public Enums enums;
    public RoundManager roundManager;
    public UIManager uIManager;
    public VoteForOrganisers voteForOrganisers;
    public VictoryProgress victoryProgress = new VictoryProgress();
    public CardGeneration cardGeneration;

    void Start()
    {
        roundManager = gameObject.GetComponent<RoundManager>();
        voteForOrganisers = gameObject.GetComponent<VoteForOrganisers>();
        cardGeneration = gameObject.GetComponent<CardGeneration>();
        uIManager.SetGameManager(gameObject);
        uIManager.IniateRoundUI();

    }

    public void OnAllPlayersConnected(List<Player> playerList)
    {
        players = playerList;

        RoleDivider _roleDivider = new RoleDivider();
        _roleDivider.GivePlayersRoles(players);

        roundManager.players = players;
        roundManager.RoundSetUp(gameObject.GetComponent<GameManager>(), uIManager);

    }

    public List<Player> GetPlayers()
    {
        return players;
    }

}
