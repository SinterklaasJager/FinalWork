using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<GameObject> testPlayerList = new List<GameObject>();
    private List<Player> players = new List<Player>();
    private Helper _helpers = new Helper();
    private RoleDivider _roleDivider = new RoleDivider();

    // Start is called before the first frame update
    void Start()
    {
        foreach (var player in testPlayerList)
        {
            var PlayerManager = player.AddComponent<PlayerManager>();
            players.Add(PlayerManager.GetPlayerClass());
        }



    }


    // Update is called once per frame
    void Update()
    {

        //Testing

        if (Input.GetKeyDown("a"))
        {
            _roleDivider.GivePlayersRoles(testPlayerList);

        }

        if (Input.GetKeyDown("z"))
        {
            _roleDivider.GivePlayersRoles(testPlayerList);

            foreach (var player in players)
            {
                Debug.Log(player.GetRole());
            }

        }

        if (Input.GetKeyDown("e"))
        {
            _roleDivider.GivePlayersRoles(testPlayerList);

            foreach (var player in testPlayerList)
            {
                Debug.Log(player.GetComponent<PlayerManager>().GetPlayerClass().GetRole());
            }

        }
    }

}
