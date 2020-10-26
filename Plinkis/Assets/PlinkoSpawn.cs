using System;
using UnityEngine;

public class PlinkoSpawn : MonoBehaviour
{
    [SerializeField] int id;
    public event Action<int> OnClick;
    private void OnMouseDown()
    {
        AttemptSpawn();
    }
    void AttemptSpawn()
    {
        OnClick?.Invoke(id);
    }
    public int GetID()
    {
        return id;
    }
}
