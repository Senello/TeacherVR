﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Tu należy zmienić Events/Basic Event na Events/MojaNazwaEventu Event podczas dziedziczenia
//Dziedziczymy tworząc nowy skrypt i wpisując public class MojaNazwaEventu : Events {}
[CreateAssetMenu(fileName = "New Event", menuName = "Events/Basic Event")]
public class Events : ScriptableObject
{
    //Nazwa eventu
    new public string name = "New Event";

    //Opis eventu
    public string description;

    //Status eventu
    public Status status = Status.Nothing;

    //Poziom trudności eventu, jeśli event nie posiada różnych poziomów powinno być 0 w przeciwnym wypadku >=1
    public int lvl;

    public delegate void AddPointsEventHandler(int numb);

    public delegate void MessageEventHandler(float time, string txt, MessageSystem.ObjectToFollow objectToFollow, MessageSystem.Window window);

    public event AddPointsEventHandler AddPointsEvent;

    public event MessageEventHandler MessageEvent;

    //Funkcja po której wywołaniu startuje event
    //Powinna zapamiętać na starcie parametry zmienianych obiektów
    public virtual void StartEvent()
    {
        Debug.Log("Starting " + name);
        status = Status.Progress;
    }

    //Funkcja która przerywa w dowolnym momencie event i przywraca scenę do stanu z przed eventu
    public virtual void AbortEvent()
    {
        Debug.Log("Aborting " + name);
        status = Status.Abort;
    }

    //Funkcja która poprawnie konczy Event
    public virtual void CompleteEvent()
    {
        Debug.Log("Complete " + name);
        status = Status.Complete;
    }

    public virtual void CallInUpdate()
    {
        Debug.Log("Doing " + name);
    }

    //Status eventu kolejno nie wystartował, jest wykonywany, został przerwany, został poprawnie zakończony
    public enum Status
    {
        Nothing,
        Progress,
        Abort,
        Complete
    }

    //Funkcja dodająca punkty dla gracza
    protected void AddPoints(int pkt)
    { 
        if (AddPointsEvent != null)
        {
            Debug.Log(pkt + " points for " + name);
            AddPointsEvent(pkt);
        }

    }

    //Funkcja wysylajaca wiadomosc do gracza
    protected void Message(float time, string txt, MessageSystem.ObjectToFollow objectToFollow, MessageSystem.Window window)
    {
        if (MessageEvent != null)
        {
            Debug.Log(txt + " for " + time + " from " + name);
            MessageEvent(time, txt, objectToFollow, window);
        }

    }
}