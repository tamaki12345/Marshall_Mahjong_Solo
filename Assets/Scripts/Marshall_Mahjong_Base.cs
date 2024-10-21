using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

namespace Marshall_Mahjong 
{
    public class Hai
    {
        public string id2Str ( int id )
        {
            if ( id == 135 )
            {
                return "haku";
            }
            else
            {
                string type = "";
                string literal = "";

                if ( id < 45 )
                {
                    type = "hiragana";
                }
                else if ( id < 90 )
                {
                    type = "katakana";
                }
                else
                {
                    type = "kanji";
                }

                int ltr_id = id % 45;

                if ( ltr_id < 5 )
                {
                    if ( ltr_id % 5 == 0 )
                    {
                        literal = "A";
                    }
                    else if ( ltr_id % 5 == 1 )
                    {
                        literal = "I";
                    }
                    else if ( ltr_id % 5 == 2 )
                    {
                        literal = "U";
                    }
                    else if ( ltr_id % 5 == 3 )
                    {
                        literal = "E";
                    }
                    else if ( ltr_id % 5 == 4 )
                    {
                        literal = "O";
                    }
                }
                else
                {
                    if ( ltr_id < 10 )
                    {
                        if ( ltr_id % 5 == 0 )
                        {
                            literal = "KA";
                        }
                        else if ( ltr_id % 5 == 1 )
                        {
                            literal = "KI";
                        }
                        else if ( ltr_id % 5 == 2 )
                        {
                            literal = "KU";
                        }
                        else if ( ltr_id % 5 == 3 )
                        {
                            literal = "KE";
                        }
                        else if ( ltr_id % 5 == 4 )
                        {
                            literal = "KO";
                        }
                    }
                    else if ( ltr_id < 15 )
                    {
                        if ( ltr_id % 5 == 0 )
                        {
                            literal = "SA";
                        }
                        else if ( ltr_id % 5 == 1 )
                        {
                            literal = "SI";
                        }
                        else if ( ltr_id % 5 == 2 )
                        {
                            literal = "SU";
                        }
                        else if ( ltr_id % 5 == 3 )
                        {
                            literal = "SE";
                        }
                        else if ( ltr_id % 5 == 4 )
                        {
                            literal = "SO";
                        }
                    }
                    else if ( ltr_id < 20)
                    {
                        if ( ltr_id % 5 == 0 )
                        {
                            literal = "TA";
                        }
                        else if ( ltr_id % 5 == 1 )
                        {
                            literal = "TI";
                        }
                        else if ( ltr_id % 5 == 2 )
                        {
                            literal = "TU";
                        }
                        else if ( ltr_id % 5 == 3 )
                        {
                            literal = "TE";
                        }
                        else if ( ltr_id % 5 == 4 )
                        {
                            literal = "TO";
                        }
                    }
                    else if ( ltr_id < 25 )
                    {
                        if ( ltr_id % 5 == 0 )
                        {
                            literal = "NA";
                        }
                        else if ( ltr_id % 5 == 1 )
                        {
                            literal = "NI";
                        }
                        else if ( ltr_id % 5 == 2 )
                        {
                            literal = "NU";
                        }
                        else if ( ltr_id % 5 == 3 )
                        {
                            literal = "NE";
                        }
                        else if ( ltr_id % 5 == 4 )
                        {
                            literal = "NO";
                        }
                    }
                    else if ( ltr_id < 30 )
                    {
                        if ( ltr_id % 5 == 0 )
                        {
                            literal = "HA";
                        }
                        else if ( ltr_id % 5 == 1 )
                        {
                            literal = "HI";
                        }
                        else if ( ltr_id % 5 == 2 )
                        {
                            literal = "HU";
                        }
                        else if ( ltr_id % 5 == 3 )
                        {
                            literal = "HE";
                        }
                        else if ( ltr_id % 5 == 4 )
                        {
                            literal = "HO";
                        }
                    }
                    else if ( ltr_id < 35 )
                    {
                        if ( ltr_id % 5 == 0 )
                        {
                            literal = "MA";
                        }
                        else if ( ltr_id % 5 == 1 )
                        {
                            literal = "MI";
                        }
                        else if ( ltr_id % 5 == 2 )
                        {
                            literal = "MU";
                        }
                        else if ( ltr_id % 5 == 3 )
                        {
                            literal = "ME";
                        }
                        else if ( ltr_id % 5 == 4 )
                        {
                            literal = "MO";
                        }
                    }
                    else if ( ltr_id < 40 )
                    {
                        if ( ltr_id % 5 == 0 )
                        {
                            literal = "RA";
                        }
                        else if ( ltr_id % 5 == 1 )
                        {
                            literal = "RI";
                        }
                        else if ( ltr_id % 5 == 2 )
                        {
                            literal = "RU";
                        }
                        else if ( ltr_id % 5 == 3 )
                        {
                            literal = "RE";
                        }
                        else if ( ltr_id % 5 == 4 )
                        {
                            literal = "RO";
                        }
                    }
                    else
                    {
                        if ( ltr_id == 40 )
                        {
                            literal = "YA";
                        }
                        else if ( ltr_id == 41 )
                        {
                            literal = "YU";
                        }
                        else if ( ltr_id == 42 )
                        {
                            literal = "YO";
                        }
                        else if ( ltr_id == 43 )
                        {
                            literal = "WA";
                        }
                        else if (ltr_id == 44 )
                        {
                            literal = "WNN";
                        }

                    }
                }
                return type + "_" + literal;
            }
        }

        public void generatePile ( ref int[] pile )
        {
            for (int i=0; i<136; i++)
            {
                pile[i] = i;
            }
            
            if ( pile.Length == 136 )
            {
                int tmp, r;

                for ( int i = 135; i >= 0; i-- )
                {
                    r = UnityEngine.Random.Range(0,i);
                    tmp = pile[r];
                    pile[r] = pile[i];
                    pile[i] = tmp;
                }

                int[] hands = new int[13];
                for( int i = 0; i < 13; i++ )
                {
                    hands[i] = pile[i];
                }

                Array.Sort(hands);

                for( int i = 0; i < 13; i++ )
                {
                    pile[i] = hands[i];
                }

                return;
            }
            else
            {
                Debug.Log("Error! Wrong Length Array");
                return;
            }
        }

    }
}

public class Marshall_Mahjong_Base : MonoBehaviour
{
}
