using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Pipeline.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public class HaiObject : MonoBehaviour
{
    private bool chosen = false;
    private Vector3 defaultPos;
    void Start()
    {
        defaultPos = this.transform.position;

        EventTrigger trigger = GetComponent<EventTrigger>();

        {
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerUp;
            entry.callback.AddListener((data) => { OnPointerUp( ( PointerEventData ) data ); });
            trigger.triggers.Add(entry);
        }

        {
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.Drag;
            entry.callback.AddListener((data) => { OnDrag( ( PointerEventData ) data ); });
            trigger.triggers.Add(entry);
        }

        if( this.gameObject.tag == "Hand" )
        {
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.EndDrag;
            entry.callback.AddListener((data) => { OnEndDrag_Hand( ( PointerEventData ) data ); });
            trigger.triggers.Add(entry);
        }
        else if( this.gameObject.tag == "Tumo" )
        {
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.EndDrag;
            entry.callback.AddListener((data) => { OnEndDrag_Tumo( ( PointerEventData ) data ); });
            trigger.triggers.Add(entry);
        }
    }

    public void OnPointerUp( PointerEventData data )
    {
        if ( chosen )
        {
            SetDeafaultPos();
            Dahai();
        }
        else if( this.gameObject.transform.position.x == defaultPos.x )
        {
            GameObject sys = GameObject.Find("System");
            Marshall_Mahjong_Test tmp = sys.GetComponent<Marshall_Mahjong_Test>();
            tmp.SetDeafaultPos();

            Vector3 pos = this.transform.position;
            pos.y = -3f;
            this.transform.position = pos;
            chosen = true;
        }
    }
    public void OnDrag( PointerEventData data )
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint ( data.position );
        pos.z = -0.1f;
        this.transform.position = pos;
    }
    public void OnEndDrag_Tumo( PointerEventData data )
    {
        if( this.transform.position.y > -3f )
        {
            SetDeafaultPos();
            Dahai();
        }
        else
        {
            this.transform.position = defaultPos;
        }
    }

    public void OnEndDrag_Hand( PointerEventData data )
    {
        if( this.transform.position.y > -3f )
        {
            SetDeafaultPos();
            Dahai();
        }
        else 
        {
            Vector3 pos = this.transform.position;
            int target_idx = Pos2Idx(pos.x);
            int swap_idx = Pos2Idx(defaultPos.x);
            
            GameObject sys = GameObject.Find("System");
            Marshall_Mahjong_Test tmp = sys.GetComponent<Marshall_Mahjong_Test>();
            tmp.SwapHands( swap_idx, target_idx );
        }
    }

    private void Dahai()
    {
        GameObject sys = GameObject.Find("System");
        Marshall_Mahjong_Test tmp = sys.GetComponent<Marshall_Mahjong_Test>();
        tmp.Dahai( this.gameObject );

        chosen = false;
        tmp.SetDeafaultPos();
    }

    public void SetDeafaultPos()
    {
        this.gameObject.transform.position = defaultPos;
    }

    private int Pos2Idx( float pos )
    {
        if( pos <= -6.6 + 0.55 )
        {
            return 0;
        }
        else if( pos <= -5.5 + 0.55 )
        {
            return 1;
        }
        else if( pos <= -4.4 + 0.55 )
        {
            return 2;
        }
        else if( pos <= -3.3 + 0.55 )
        {
            return 3;
        }
        else if( pos <= -2.2 + 0.55 )
        {
            return 4;
        }
        else if( pos <= -1.1 + 0.55 )
        {
            return 5;
        }
        else if( pos <= 0.55)
        {
            return 6;
        }
        else if( pos <= 1.1 + 0.55 )
        {
            return 7;
        }
        else if( pos <= 2.2 + 0.55 )
        {
            return 8;
        }
        else if( pos <= 3.3 + 0.55 )
        {
            return 9;
        }
        else if( pos <= 4.4 + 0.55 ) 
        {
            return 10;
        }
        else if( pos <= 5.5 + 0.55 )
        {
            return 11;
        }
        else 
        {
            return 12;
        }
    }
}
