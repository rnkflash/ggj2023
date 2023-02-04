using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Singleton<Player>
{
    public string map;
    public Vector3? entrancePosition = null;

    public void ResetProgress() {
        map = null;
        entrancePosition = null;
    }
}
