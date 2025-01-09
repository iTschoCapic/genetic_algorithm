using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class GeneticAlgorithm
{

    public float fitnessScore = 1.0f;

    public int degatsEffectué = 0;
    public int degatsReçu = 0;
    public bool mort = false;
    public bool ennemieMort = false;

    public List<int> attaquesList;

    //constructeur
    public GeneticAlgorithm(List<int> attaques)
    {
        attaquesList = attaques;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetFitness()
    {
        fitnessScore = degatsEffectué-degatsReçu;
    }
}
