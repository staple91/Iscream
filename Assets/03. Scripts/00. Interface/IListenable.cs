using UnityEngine;
using KimKyeongHun;
public interface IListenable
{
    public float Loudness { get; set; }
    public Vector3 Pos { get; }

    public Player LoudPlayer { get; set; }
    
}
