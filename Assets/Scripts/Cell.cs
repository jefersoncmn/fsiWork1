using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe da célula que irá compor o mapa
/// </summary>
public class Cell : MonoBehaviour
{
    public GameObject gameObject;
    public Cell left, right, up, down;
    public AmbientType ambientType;
    public int coins;
    public bool endPoint;

}
