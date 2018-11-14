using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserManager : MonoBehaviour {

    /* 
     * Diese Klasse hat alle Laser als eine Collection gespeichert und updatet sie - jetzt jeden frame - 
     * kann aber auch nur einige male pro Sekunde passieren um die performance zu verbessern
     */
    HashSet<Laser> lasers;

    [Tooltip("how often are the lasers updatet via raycasts - 1 - means once per second")]
    public float updateInterval = 1 / 60;
    float nextLaserUpdateTime;

    public static LaserManager Instance;

    //Singletoncode
    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(Instance); // es kann passieren wenn wir eine neue Scene laden dass immer noch eine Instanz existiert
        }
        else
        {
            Instance = this;
        }
    }

    void Start ()
    {
        lasers = new HashSet<Laser>();
        nextLaserUpdateTime = Time.time + updateInterval;

    }
	
	// Update is called once per frame
	void Update ()
    {
        if(Time.time> nextLaserUpdateTime)
        {
            UpdateLasers();
            nextLaserUpdateTime = Time.time + updateInterval;
        }
        
	}

    void UpdateLasers()
    {
        foreach(Laser laser in lasers)
        {
            laser.UpdateLaser();
        }
    }

    public void AddLaser(Laser laser)
    {
        lasers.Add(laser);
    }

    //this gets called by a block object which wants to know which lasers are hitting it
    public List<Laser> GetInputLasers(BlockObject blockObject)
    {
        List<Laser> lasersToReturn = new List<Laser>();

        foreach (Laser laser in lasers)
        {
            if(laser.destinationBlock == blockObject)
            {
                lasersToReturn.Add(laser);
            }
        }

        return lasersToReturn;
    }
}
