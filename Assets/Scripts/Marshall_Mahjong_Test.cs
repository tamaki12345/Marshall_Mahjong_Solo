using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Marshall_Mahjong;
using System;
using Microsoft.Unity.VisualStudio.Editor;

public class Marshall_Mahjong_Test : MonoBehaviour
{
    private int[] pile = new int[136];

    Hai hai = new Marshall_Mahjong.Hai();

    private GameObject[] river = new GameObject[18];
    private GameObject[] dora = new GameObject[2];
    private GameObject[] hand = new GameObject[13];
    private GameObject tumo;

    private int tumo_index;

    void Start()
    {
        initGame();
    }

    void initGame(){
        hai.generatePile( ref pile );

        tumo = GameObject.Find( "Tumo" );

        for ( int i=0; i<river.Length; i++ )
        {
            river[i] = GameObject.Find( "Tile" + i );
        }

        for ( int i = 0; i < hand.Length; i++ )
        {
            hand[i] = GameObject.Find( "Hand" + i );
        }

        for ( int i = 0; i < dora.Length; i++ )
        {
            dora[i] = GameObject.Find( "Dora" + i );
        }

        for ( int i = 0; i < river.Length; i++ )
        {
            river[i].GetComponent<SpriteRenderer>().sprite = null;
        }

        for (int i=0; i<13; i++){
            if ( i >= hand.Length ) Debug.Log( "wrong range hand array" );
            SpriteRenderer tmp_ = hand[i].GetComponent<SpriteRenderer>();

            Addressables.LoadAssetAsync<Sprite>( hai.id2Str(pile[i]) ).Completed += handle => 
            {
                if ( handle.Result == null ) 
                {
                    Debug.Log( "Load Error" );
                    return;
                }
                tmp_.sprite = handle.Result;
            };
        }

        {
            SpriteRenderer tmp_ = dora[0].GetComponent<SpriteRenderer>();
            dora[0].GetComponent<SpriteRenderer>().sprite = null;

            Addressables.LoadAssetAsync<Sprite>( hai.id2Str(pile[130]) ).Completed += handle => 
            {
                if ( handle.Result == null ) 
                {
                    Debug.Log( "Load Error" );
                    return;
                }
                tmp_.sprite = handle.Result;
            };
        }

        {
            tumo_index = 13;
            Tumo();
        }
    }

    void Tumo()
    {
        SpriteRenderer tmp_ = tumo.GetComponent<SpriteRenderer>();

        Addressables.LoadAssetAsync<Sprite>( hai.id2Str( pile[tumo_index]) ).Completed += handle => 
        {
            if ( handle.Result == null ) 
            {
                Debug.Log( "Load Error" );
                return;
            }
            tmp_.sprite = handle.Result;
        };

        tumo_index++;
    }
}
