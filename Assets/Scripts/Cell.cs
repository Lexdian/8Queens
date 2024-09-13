using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;

public class Cell : MonoBehaviour
{

    public Color empty, blocked, queen, error;
    public states state = states.empty;
    private SpriteRenderer spriteRenderer;

    public delegate void mudarState();

    public static event mudarState mudouState;

    public enum states
    {
        empty,
        blocked,
        queen,
        error
    }

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case states.empty:
                spriteRenderer.color = empty; 
                break;
            case states.blocked:
                spriteRenderer.color = blocked;
                break;
            case states.queen:
                spriteRenderer.color = queen;
                break;
            case states.error:
                spriteRenderer.color = error;
                break;
        }
    }

    private void OnMouseDown()
    {
        if (state == states.empty || state == states.blocked)
        {
            state = states.queen;
        }
        else
        {
            state = states.empty;
        }
        mudouState?.Invoke();
    }
}
