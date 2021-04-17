using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enums : MonoBehaviour
{
    public enum CardType
    {
        good,
        bad
    }

    public struct EventHandlers
    {
        public System.Action<Player> onAssistantPicked;
        public System.Action<bool> OnVoteEnd;
    }
}
