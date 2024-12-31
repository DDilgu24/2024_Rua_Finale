using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public string[] spriteNames;
    // Start is called before the first frame update
    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        spriteNames = Resources.LoadAll<Sprite>("Characters/Icon").Select(s => s.name).ToArray();
    }

}
