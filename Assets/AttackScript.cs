using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour
{
    public void Attack() {
        GetComponentInParent<Wormie>()?.Attack();
        GetComponentInParent<Birdie>()?.Attack();
    }
}
