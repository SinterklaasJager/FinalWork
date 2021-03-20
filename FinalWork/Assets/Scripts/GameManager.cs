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
        //Add class to playerobjects
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

        if (Input.GetKeyDown("e"))
        {
            _roleDivider.GivePlayersRoles(players);

            foreach (var player in testPlayerList)
            {
                Debug.Log(player.GetComponent<PlayerManager>().GetPlayerClass().GetRole());
                if ((player.GetComponent<PlayerManager>().GetPlayerClass().GetRole() == 0))
                {
                    player.GetComponent<MeshRenderer>().material.color = Color.green;
                }
                else if ((player.GetComponent<PlayerManager>().GetPlayerClass().GetRole() == 1))
                {
                    player.GetComponent<MeshRenderer>().material.color = Color.red;
                }
                else if ((player.GetComponent<PlayerManager>().GetPlayerClass().GetRole() == 2))
                {
                    player.GetComponent<MeshRenderer>().material.color = Color.blue;
                }
            }

        }
    }

}
