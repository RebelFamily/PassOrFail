using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageBehaviour : StateMachineBehaviour
{
    public bool debug;
    public bool NormalizeTime = true;
    
    public MesssageItem[] onEnterMessage;   //Store messages to send it when Enter the animation State
    public MesssageItem[] onExitMessage;    //Store messages to send it when Exit  the animation State
    public MesssageItem[] onTimeMessage;    //Store messages to send on a specific time  in the animation State
    
    public bool onEnter = false;
    public bool onExit= false;
    public bool onTime = false;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Debug.Log(onEnter + " " + onExit + " " + onTime);
        if (onTime)
        {foreach (MesssageItem ontimeM in onTimeMessage)
            {
                //Debug.Log("onEnterM.Active: " + ontimeM.message);
                ontimeM.sent = false;  //Set all the messages Ontime Sent = false when start
            }
        }

        if (onEnter)
        {
            foreach (MesssageItem onEnterM in onEnterMessage)
            {
                //Debug.Log("Messages: " + onEnterM.message);
                if (!string.IsNullOrEmpty(onEnterM.message))
                {
                    //Debug.Log("onEnterM.Active: " + onEnterM.message);
                    onEnterM.DeliverMessage(animator, debug);
                }
            }
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.fullPathHash == animator.GetNextAnimatorStateInfo(layerIndex).fullPathHash) return; //means is transitioning to itself

        foreach (MesssageItem onTimeM in onTimeMessage)
        {
            if (!string.IsNullOrEmpty(onTimeM.message))
            {
                // float stateTime = stateInfo.loop ? stateInfo.normalizedTime % 1 : stateInfo.normalizedTime;
                //Debug.Log("NormalizeTime: " + NormalizeTime);
                float stateTime = NormalizeTime ? stateInfo.normalizedTime % 1 : stateInfo.normalizedTime;

                if (!onTimeM.sent && (stateTime >= onTimeM.time))
                {
                    onTimeM.sent = true;

                    //  Debug.Log(onTimeM.message + ": "+stateTime);

                    onTimeM.DeliverMessage(animator, debug);
                }
            }
        }
    }

     //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (onExit)
        {
            foreach (var onExitM in onExitMessage)
            {
                if (!string.IsNullOrEmpty(onExitM.message))
                {
                    onExitM.DeliverMessage(animator, debug);
                }
            }
        }
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
[Serializable]
public class MesssageItem
{
    public string message;
    public TypeMessage typeM;
    public bool boolValue;
    public int intValue;
    public float floatValue;
    public string stringValue;

    public float time;
    public bool sent;
    
    public void DeliverMessage(Component anim, bool debug = false)
    {
        //Debug.Log("TypeMessage: " + typeM);
         switch (typeM)
        {
            case TypeMessage.Bool:
                SendMessage(anim, message, boolValue);

                break;
            case TypeMessage.Int:
                SendMessage(anim, message, intValue);
                break;
            case TypeMessage.Float:
                SendMessage(anim, message, floatValue);
                break;
            case TypeMessage.String:
                SendMessage(anim, message, stringValue);
                break;
            case TypeMessage.Void:
                SendMessageVoid(anim, message);
                break;
        }

        //if (debug) Debug.Log($"<b>[Send Msg: {message}->] [{typeM}]</b> T:{Time.time:F3}");  //Debug
    }
    private void SendMessage(Component anim, string message, object value)
    {
        anim.SendMessage(message, value, SendMessageOptions.DontRequireReceiver);
    }
    private void SendMessageVoid(Component anim, string message)
    {
        anim.SendMessage(message, SendMessageOptions.DontRequireReceiver);
    }
}

public enum TypeMessage
{
    Void,
    Bool,
    Int,
    Float,
    String
}
