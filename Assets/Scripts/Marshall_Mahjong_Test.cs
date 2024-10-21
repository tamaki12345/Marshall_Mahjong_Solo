using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Marshall_Mahjong;
using UnityEngine.SceneManagement;
using System.Net.Sockets;
using System.Net;
using System;
using System.Text;

public class Marshall_Mahjong_Test : MonoBehaviour
{
    //山の配列，intで管理しHaiクラスのid2strを利用ｓ
    private int[] pile = new int[136];

    //雀牌クラスのインスタンス
    Hai hai = new Marshall_Mahjong.Hai();

    //河の配列
    private GameObject[] river = new GameObject[18];

    //ドラのゲーム内実体
    private GameObject dora;

    //手牌のゲーム内実体
    private GameObject[] hand = new GameObject[13];

    //ツモ牌のゲーム内実体
    private GameObject tumo;

    //ツモが開始する山内でのインデックス
    private int tumo_index = 13;

    //河配列内のインデックス
    private int river_index = 0;

    //ゲーム終了時表示UIの実体
    private GameObject finish_overlay;

    //Python間通信インスタンス
    private UdpClient udp;
    const string HOST = "127.0.0.1";
    const int PORT = 50007;

    //Scene開始時
    void Start()
    {
        udp = new UdpClient();
        finish_overlay = GameObject.Find("finish_overlay");
        if( finish_overlay== null )
        {
            Debug.Log(" null object return ");
            return;
        }
        else
        {
            finish_overlay.SetActive(false);
            initGame();
        }
    }

    //ゲーム初期化
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
            Tumo();
        }
    }

    //山からツモ
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

    //手牌と自摸から選択して打牌
    public void Dahai( GameObject obj )
    {
        Sprite tmp;
        
        tmp = obj.GetComponent<SpriteRenderer>().sprite;
        river[river_index].GetComponent<SpriteRenderer>().sprite = tmp;
        obj.GetComponent<SpriteRenderer>().sprite = tumo.GetComponent<SpriteRenderer>().sprite;

        var handle = Addressables.LoadAssetAsync<Sprite>( hai.id2Str( pile[tumo_index]) );
        handle.Completed += handle => 
        {
            if ( handle.Result == null ) 
            {
                Debug.Log( "Load Error" );
                return;
            }
            tumo.GetComponent<SpriteRenderer>().sprite = handle.Result;
        };

        if ( river_index == 17 )
        {
            if( finish_overlay == null )
            {
                Debug.Log(" null object ");
                return;
            }
            else
            {
                tumo.SetActive(false);
                finish_overlay.SetActive(true);
                Debug.Log( "Over Index Tumo" );
                return;
            }
        }

        Tumo();
        river_index++;
    }

    // swap: draged tile, target: drag end point tile
    //手牌の並びを入れ替える
    public void SwapHands( int swap, int target )
    {
        Sprite tmp1, tmp2;

        if( swap > target )
        {
            tmp1 = hand[target].GetComponent<SpriteRenderer>().sprite;
            tmp2 = hand[swap].GetComponent<SpriteRenderer>().sprite;
            hand[target].GetComponent<SpriteRenderer>().sprite = hand[swap].GetComponent<SpriteRenderer>().sprite;
            
            for( int i = target + 1; i < swap + 1; i++ )
            {
                tmp2 = hand[i].GetComponent<SpriteRenderer>().sprite;
                hand[i].GetComponent<SpriteRenderer>().sprite = tmp1;
                tmp1 = tmp2;
            }
        }

        else if( swap < target )
        {
            tmp1 = hand[target].GetComponent<SpriteRenderer>().sprite;
            tmp2 = hand[swap].GetComponent<SpriteRenderer>().sprite;
            hand[target].GetComponent<SpriteRenderer>().sprite = hand[swap].GetComponent<SpriteRenderer>().sprite;

            for( int i = target-1; i > swap-1; i-- )
            {
                tmp2 = hand[i].GetComponent<SpriteRenderer>().sprite;
                hand[i].GetComponent<SpriteRenderer>().sprite = tmp1;
                tmp1 = tmp2;
            }
        }
        SetDeafaultPos();
    }

    //手牌のゲーム内実体を元の位置へ
    public void SetDeafaultPos()
    {
        for( int i = 0; i < 13; i++ )
        {
            hand[i].GetComponent<HaiObject>().SetDeafaultPos();
        }
    }

    //ゲームのSceneを初期化
    public void InitScene()
    {
        tumo.SetActive(true);

        if( finish_overlay== null )
        {
            Debug.Log(" null object return ");
            return;
        }
        else
        {
            finish_overlay.SetActive(false);
            initGame();
        }
    }

    //Pythonへ判定を要求
    private void ReqestJudge()
    {
        IPEndPoint Ep = new IPEndPoint(IPAddress.Parse(HOST), PORT);
        udp.Connect(Ep);

        byte[] message = BitConverter.GetBytes(100);
        var encoding = Encoding.GetEncoding("UTF-8");
        message = encoding.GetBytes("TEST");
        
        udp.Send(message, message.Length, Ep);
    }

    //Pythonから判定を取得しstringへ
    private string GetJudge()
    {
        IPEndPoint senderEP = null;
        byte[] receivedBytes = udp.Receive(ref senderEP);
        var encoding = Encoding.GetEncoding("UTF-8");

        return encoding.GetString(receivedBytes);
    }
}