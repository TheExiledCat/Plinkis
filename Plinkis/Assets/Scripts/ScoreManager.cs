using System;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField]
    PlinkoGame pg;
    float score;
    float multiplier=1;
    int lines;
    public event Action<float, float, int> OnAdd;
    private void Awake()
    {
        pg.OnPlinkoCompleted += AddMultiplier;
        Field.Grid.OnTetris += AddLines;
        Field.Grid.OnTetris += AddScore;
    }
    void AddMultiplier(float m)
    {
        multiplier += m;
        OnAdd?.Invoke(score, multiplier, lines);
    }
    void AddScore(int s)
    {
        score += (100 * s) * multiplier;
        OnAdd?.Invoke(score, multiplier, lines);
    }
    void AddLines(int l)
    {
        lines += l;
        OnAdd?.Invoke(score, multiplier, lines);
    }
}
