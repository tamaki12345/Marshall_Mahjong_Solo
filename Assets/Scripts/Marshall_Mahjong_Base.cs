using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                string type, literal = "";

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

                if ( ltr_id >= 5 )
                {
                    if ( ltr_id < 10 )
                    {
                        literal += "k";
                    }
                    else if ( ltr_id < 15 )
                    {
                        literal += "s";
                    }
                    else if ( ltr_id < 20)
                    {
                        literal += "t";
                    }
                    else if ( ltr_id < 25 )
                    {
                        literal += "n";
                    }
                    else if ( ltr_id < 30 )
                    {
                        literal += "h";
                    }
                    else if ( ltr_id < 35 )
                    {
                        literal += "m";
                    }
                    else if ( ltr_id < 40 )
                    {
                        literal += "r";
                    }
                    else if (ltr_id < 43 )
                    {
                        literal += "y";
                    }
                    else
                    {
                        literal += "w";
                    }

                    if ( ltr_id < 40 )
                    {
                        if ( ltr_id % 5 == 0 )
                        {
                            literal += "a";
                        }
                        else if ( ltr_id % 5 == 1 )
                        {
                            literal += "i";
                        }
                        else if ( ltr_id % 5 == 2 )
                        {
                            literal += "u";
                        }
                        else if ( ltr_id % 5 == 3 )
                        {
                            literal += "e";
                        }
                        else if ( ltr_id % 5 == 4 )
                        {
                            literal += "o";
                        }
                    }
                    else
                    {
                        if ( ltr_id == 40 || ltr_id == 43 )
                        {
                            literal += "a";
                        }
                        else if ( ltr_id == 41 )
                        {
                            literal += "u";
                        }
                        else if ( ltr_id == 42 )
                        {
                            literal += "o";
                        }
                        else if (ltr_id == 44 )
                        {
                            literal += "nn";
                        }

                    }
                }
                return type + "_" + literal;
            }
        }

        public void generatePile ( int[] pile )
        {
            if ( pile.Length == 136 )
            {
                int tmp, r;

                for ( int i = 136; i > 0; i-- )
                {
                    r = Random.Range(0,i);
                    tmp = pile[r];
                    pile[r] = pile[i];
                    pile[i] = r;
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
