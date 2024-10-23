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
using TMPro;

public class Marshall_Mahjong_Solo : MonoBehaviour{
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

    //Scene開始時
    void Start()
    {
        udp = new UdpClient(9000);
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
    public void initGame(){
        river_index = 0;
        tumo_index = 13;
        
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

    //プレイヤーが和了を選択したときに発動
    public void HoraButton()
    {
        GameObject textObj = finish_overlay.transform.GetChild(1).gameObject;
        TextMeshProUGUI textUI = textObj.GetComponent<TextMeshProUGUI>();

        tumo.SetActive(false);
        finish_overlay.SetActive(true);
        
        //和了かを判定し，テキストで表示
        string judgeResult = JudgeHora();
        textUI.text = judgeResult;
        
        return;
    }

    //和了かどうかを判定
    private string JudgeHora()
    {
        ReqestJudge();
        string result = GetJudge();

        return result;
    }
    
    
    //Python間通信インスタンス
    private UdpClient udp;
    const string python_address = "127.0.0.1";
    const int python_port = 50000;

    const int my_port = 9000;

    //Pythonへ判定を要求
    private void ReqestJudge()
    {
        //通信のend point
        IPEndPoint Ep = new IPEndPoint(IPAddress.Parse(python_address), python_port);
        
        byte[] message = BitConverter.GetBytes(100);
        var encoding = Encoding.GetEncoding("UTF-8");
        //文字列testを送信
        //本番では手牌+ツモ牌を送信
        message = encoding.GetBytes("TEST");
        
        //end pointへ送信
        udp.SendAsync(message, message.Length, Ep);
    }

    //Pythonから判定を取得しstringへ
    private string GetJudge()
    {
        string result;

        //送信元IPの格納変数
        IPEndPoint Ep = null;
        byte[] receivedBytes = udp.Receive(ref Ep);
        var encoding = Encoding.GetEncoding("UTF-8");

        //受信文字列はrecieveBytesをutf-8でencode
        result = encoding.GetString(receivedBytes);

        return result;
    }
}