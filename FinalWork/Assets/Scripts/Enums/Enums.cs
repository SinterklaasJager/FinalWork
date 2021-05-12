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

    public enum CardPickerType
    {
        teamLeader,
        assistant
    }

    public enum GameEndReason
    {
        saboteurDead,
        rebelsDead,
        goodGuysDead,
        enoughGoodPoints,
        enoughBadPoints
    }

    public struct EventHandlers
    {
        public System.Action<Player> onAssistantPicked;
        public System.Action<int> onAssistantPickedInt;
        public System.Action<int> onPlayerToKillPicked;
        public System.Action<bool> OnVoteEnd;
        public System.Action<List<Enums.CardType>> OnAssistantCardsPicked;
        public System.Action<Enums.CardType> OnCardSelected;
        public System.Action<string> OnNameEntered;

    }

    public struct AREvents
    {
        public System.Action<GameObject> OnHostGameLocationPicked;
    }
}
