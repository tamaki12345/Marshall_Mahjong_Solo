using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Marshall_Mahjong;
using System;

public class Marshall_Mahjong_Test : MonoBehaviour
{
    private int[] pile = new int[136];

    Hai hai = new Marshall_Mahjong.Hai();

    void Start()
    {
        hai.generatePile(pile);
    }
}
