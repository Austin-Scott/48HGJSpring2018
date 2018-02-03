using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardQuickFingers : Card {

        public override IEnumerator Use() {
                holder.IncreaseDexterity(1);
                //TODO: Destroy this card
                return null;
        }
}
