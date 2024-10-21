using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MessageBehaviour)), CanEditMultipleObjects]
public class MessageBehaviourInspector : Editor
{
#if UNITY_EDITOR
    #region properties
    public SerializedProperty 
        OnEnter,
        OnExit,
        OnTime,
        OnEnterMessage,
        OnExitMessage,
        OnTimeMessage
        ;

    private void OnEnable()
    {
        OnEnter = serializedObject.FindProperty("onEnter");
        OnExit = serializedObject.FindProperty("onExit");
        OnTime = serializedObject.FindProperty("onTime");
        OnEnterMessage = serializedObject.FindProperty("onEnterMessage");
        OnExitMessage = serializedObject.FindProperty("onExitMessage");
        OnTimeMessage = serializedObject.FindProperty("onTimeMessage");
        
        _messageBehaviour = (MessageBehaviour)target;
    }

    private MessageBehaviour _messageBehaviour;
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        GUILayout.BeginHorizontal("box");	
        
        
        if(GUILayout.Button("On Enter"))
        {
            _messageBehaviour.onEnter = !_messageBehaviour.onEnter;
        }
        
        if(GUILayout.Button("On Exit"))
        {
            _messageBehaviour.onExit = !_messageBehaviour.onExit;
        }
        
        if(GUILayout.Button("On Time"))
        {
            _messageBehaviour.onTime = !_messageBehaviour.onTime;
        }
        GUILayout.EndHorizontal();
        
        GUILayout.BeginVertical("box");

        if (_messageBehaviour.onEnter)
        {
            // EditorGUILayout.PropertyField(OnEnterMessage);
            AddOnEnterMessageItem();
        }

        if (_messageBehaviour.onEnter && _messageBehaviour.onExit)
        {
            GUILayout.Space(10);
            GUILayout.Label("------------------------------------------------------------------------");
            GUILayout.Space(10);
        }

        if (_messageBehaviour.onEnter && !_messageBehaviour.onExit && _messageBehaviour.onTime)
        {
            GUILayout.Space(10);
            GUILayout.Label("------------------------------------------------------------------------");
            GUILayout.Space(10);
        }

        if (_messageBehaviour.onExit)
        {
            //EditorGUILayout.PropertyField(OnExitMessage);
            AddOnExitMessageItem();
        }
        
        if (_messageBehaviour.onTime && _messageBehaviour.onExit)
        {
            GUILayout.Space(10);
            GUILayout.Label("------------------------------------------------------------------------");
            GUILayout.Space(10);
        }
        
        if (_messageBehaviour.onTime)
        {
            //EditorGUILayout.PropertyField(OnTimeMessage);
            AddOnTimeMessageItem();
        }
        GUILayout.EndVertical();
        
        serializedObject.ApplyModifiedProperties ();
    }

    private void AddOnEnterMessageItem()
    {
        for (var i = 0; i < OnEnterMessage.arraySize; i++)
        {
            //EditorGUILayout.PropertyField(OnEnterMessage.GetArrayElementAtIndex(i));
            
            
            GUILayout.BeginHorizontal("box");	
           
            
            _messageBehaviour.onEnterMessage[i].message = EditorGUILayout.TextField
                (_messageBehaviour.onEnterMessage[i].message);
            
            var j = EditorGUILayout.EnumPopup(_messageBehaviour.onEnterMessage[i].typeM);
            switch (j)
            {
                case TypeMessage.Void:
                    _messageBehaviour.onEnterMessage[i].typeM = TypeMessage.Void;
                    break;
                case TypeMessage.Bool:
                    _messageBehaviour.onEnterMessage[i].typeM = TypeMessage.Bool;
                    
                    _messageBehaviour.onEnterMessage[i].boolValue = EditorGUILayout.Toggle
                        (_messageBehaviour.onEnterMessage[i].boolValue);
                    break;
                
                case TypeMessage.Int:
                    _messageBehaviour.onEnterMessage[i].typeM = TypeMessage.Int;
                    
                    _messageBehaviour.onEnterMessage[i].intValue = EditorGUILayout.IntField
                        (_messageBehaviour.onEnterMessage[i].intValue);
                    break;
                case TypeMessage.Float:
                    _messageBehaviour.onEnterMessage[i].typeM = TypeMessage.Float;
                    
                    _messageBehaviour.onEnterMessage[i].floatValue = EditorGUILayout.FloatField
                        (_messageBehaviour.onEnterMessage[i].floatValue);
                    break;
                case TypeMessage.String:
                    _messageBehaviour.onEnterMessage[i].typeM = TypeMessage.String;
                    
                    _messageBehaviour.onEnterMessage[i].stringValue = EditorGUILayout.TextField
                        (_messageBehaviour.onEnterMessage[i].stringValue);
                    break;
            }
           GUILayout.EndHorizontal();
           GUILayout.Space(10);
        }
        GUILayout.BeginHorizontal("box");	
        GUILayout.FlexibleSpace();
        if(GUILayout.Button("+"))
        {
            OnEnterMessage.arraySize++;
        }
        if(GUILayout.Button("-"))
        {
            OnEnterMessage.arraySize--;
        }

        GUILayout.EndHorizontal();
    }
    private void AddOnExitMessageItem()
    {
        for (var i = 0; i < OnExitMessage.arraySize; i++)
        {
            //EditorGUILayout.PropertyField(OnEnterMessage.GetArrayElementAtIndex(i));
            
            
            GUILayout.BeginHorizontal("box");	
           
            
            _messageBehaviour.onExitMessage[i].message = EditorGUILayout.TextField
                (_messageBehaviour.onExitMessage[i].message);

            var j = EditorGUILayout.EnumPopup(_messageBehaviour.onExitMessage[i].typeM);
            switch (j)
            {
                case TypeMessage.Void:
                    _messageBehaviour.onExitMessage[i].typeM = TypeMessage.Void;
                    break;
                case TypeMessage.Bool:
                    _messageBehaviour.onExitMessage[i].typeM = TypeMessage.Bool;
                    
                    _messageBehaviour.onExitMessage[i].boolValue = EditorGUILayout.Toggle
                        (_messageBehaviour.onExitMessage[i].boolValue);
                    break;
                
                case TypeMessage.Int:
                    _messageBehaviour.onExitMessage[i].typeM = TypeMessage.Int;
                    
                    _messageBehaviour.onExitMessage[i].intValue = EditorGUILayout.IntField
                        (_messageBehaviour.onExitMessage[i].intValue);
                    break;
                case TypeMessage.Float:
                    _messageBehaviour.onExitMessage[i].typeM = TypeMessage.Float;
                    
                    _messageBehaviour.onExitMessage[i].floatValue = EditorGUILayout.FloatField
                        (_messageBehaviour.onExitMessage[i].floatValue);
                    break;
                case TypeMessage.String:
                    _messageBehaviour.onExitMessage[i].typeM = TypeMessage.String;
                    
                    _messageBehaviour.onExitMessage[i].stringValue = EditorGUILayout.TextField
                        (_messageBehaviour.onExitMessage[i].stringValue);
                    break;
            }
           GUILayout.EndHorizontal();
           GUILayout.Space(10);
        }
        GUILayout.BeginHorizontal("box");	
        GUILayout.FlexibleSpace();
        if(GUILayout.Button("+"))
        {
            OnExitMessage.arraySize++;
        }
        
        if(GUILayout.Button("-"))
        {
            OnExitMessage.arraySize--;
        }
        GUILayout.EndHorizontal();
    }
    private void AddOnTimeMessageItem()
    {
        for (var i = 0; i < OnTimeMessage.arraySize; i++)
        {
            //EditorGUILayout.PropertyField(OnEnterMessage.GetArrayElementAtIndex(i));
            
            
            GUILayout.BeginHorizontal("box");	
           
            
            _messageBehaviour.onTimeMessage[i].message = EditorGUILayout.TextField
                (_messageBehaviour.onTimeMessage[i].message);

            var j = EditorGUILayout.EnumPopup(_messageBehaviour.onTimeMessage[i].typeM);
            switch (j)
            {
                case TypeMessage.Void:
                    _messageBehaviour.onTimeMessage[i].typeM = TypeMessage.Void;
                    break;
                case TypeMessage.Bool:
                    _messageBehaviour.onTimeMessage[i].typeM = TypeMessage.Bool;
                    
                    _messageBehaviour.onTimeMessage[i].boolValue = EditorGUILayout.Toggle
                        (_messageBehaviour.onTimeMessage[i].boolValue);
                    break;
                
                case TypeMessage.Int:
                    _messageBehaviour.onTimeMessage[i].typeM = TypeMessage.Int;
                    
                    _messageBehaviour.onTimeMessage[i].intValue = EditorGUILayout.IntField
                        (_messageBehaviour.onTimeMessage[i].intValue);
                    break;
                case TypeMessage.Float:
                    _messageBehaviour.onTimeMessage[i].typeM = TypeMessage.Float;
                    
                    _messageBehaviour.onTimeMessage[i].floatValue = EditorGUILayout.FloatField
                        (_messageBehaviour.onTimeMessage[i].floatValue);
                    break;
                case TypeMessage.String:
                    _messageBehaviour.onTimeMessage[i].typeM = TypeMessage.String;
                    
                    _messageBehaviour.onTimeMessage[i].stringValue = EditorGUILayout.TextField
                        (_messageBehaviour.onTimeMessage[i].stringValue);
                    break;
            }
                    
            _messageBehaviour.onTimeMessage[i].time = EditorGUILayout.FloatField
                (_messageBehaviour.onTimeMessage[i].time);
            
           GUILayout.EndHorizontal();
           GUILayout.Space(10);
        }
        GUILayout.BeginHorizontal("box");	
        GUILayout.FlexibleSpace();
        if(GUILayout.Button("+"))
        {
            OnTimeMessage.arraySize++;
        }
        
        if(GUILayout.Button("-"))
        {
            OnTimeMessage.arraySize--;
        }
        GUILayout.EndHorizontal();
    }
    #endregion
#endif
}
