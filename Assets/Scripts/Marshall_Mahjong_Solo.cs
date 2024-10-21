using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Marshall_Mahjong;
using System;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;

public class Marshall_Mahjong_Solo : MonoBehaviour
{
    private int[] pile = new int[136];

    Hai hai = new Marshall_Mahjong.Hai();

    private GameObject[] river = new GameObject[18];
    //private GameObject[] dora = new GameObject[2];
    private GameObject dora;
    private GameObject[] hand = new GameObject[13];
    private GameObject tumo;

    private int tumo_index;
    private int river_index = 0;

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

        {
            dora = GameObject.Find( "Dora" );
        }

        for ( int i = 0; i < river.Length; i++ )
        {
            river[i].GetComponent<SpriteRenderer>().sprite = null;
        }

        for (int i=0; i<13; i++){
            if ( i >= hand.Length ) Debug.Log( "wrong range hand array" );
            SpriteRenderer tmp_ = hand[i].GetComponent<SpriteRenderer>();

            var handle = Addressables.LoadAssetAsync<Sprite>( hai.id2Str( pile[i]) );
            handle.Completed += handle => 
            {
                if ( handle.Result == null ) 
                {
                    Debug.Log( "Load Error" );
                    return;
                }
                tmp_.sprite = handle.Result;
            };

            Addressables.Release(handle);
        }

        {
            SpriteRenderer tmp_ = dora.GetComponent<SpriteRenderer>();

            var handle = Addressables.LoadAssetAsync<Sprite>( hai.id2Str( pile[130]) );
            handle.Completed += handle => 
            {
                if ( handle.Result == null ) 
                {
                    Debug.Log( "Load Error" );
                    return;
                }
                tmp_.sprite = handle.Result;
            };

            Addressables.Release(handle);
        }

        {
            tumo_index = 13;
            Tumo();
        }
    }

    void Tumo()
    {
        SpriteRenderer tmp_ = tumo.GetComponent<SpriteRenderer>();

        var handle = Addressables.LoadAssetAsync<Sprite>( hai.id2Str( pile[tumo_index]) );
        handle.Completed += handle => 
        {
            if ( handle.Result == null ) 
            {
                Debug.Log( "Load Error" );
                return;
            }
            tmp_.sprite = handle.Result;
        };

        Addressables.Release(handle);

        tumo_index++;
    }

    public void Dahai( GameObject obj )
    {
        if ( tumo_index > 13 + 18 )
        {
            Debug.Log( "Over Index Tumo" );
            return;
        }

        SpriteRenderer tmp;
        
        tmp = river[river_index].GetComponent<SpriteRenderer>();
        tmp.sprite = obj.GetComponent<SpriteRenderer>().sprite;

        tmp = obj.GetComponent<SpriteRenderer>();
        var handle = Addressables.LoadAssetAsync<Sprite>( hai.id2Str( pile[tumo_index - 1]) );
        handle.Completed += handle => 
        {
            if ( handle.Result == null ) 
            {
                Debug.Log( "Load Error" );
                return;
            }
            tmp.sprite = handle.Result;
        };

        Tumo();
        river_index++;
    }
}
