using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roles : MonoBehaviour
{

    public string type { get; set; }
    public int playerID { get; set; }

    public bool saboteur { get; set; }

    public Roles(string type, int playerID, bool saboteur)
    {
        this.type = type;
        this.playerID = playerID;
        this.saboteur = saboteur;

    }

}
