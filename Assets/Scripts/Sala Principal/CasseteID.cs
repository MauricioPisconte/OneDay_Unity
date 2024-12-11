using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasseteID : MonoBehaviour
{
    [SerializeField] private int casseteID;

    public int ReturnCasseteID()
    {
        return casseteID;
    }
}
