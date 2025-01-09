using NUnit.Framework;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Tournament : MonoBehaviour
{
    public int populationSize = 5; // CHANGER EN 20 EN RELEASE
    public int mutationRate; // Chance of a mutation occuring, 0 - 100%

    public List<GeneticAlgorithm> population = new List<GeneticAlgorithm>();

    public int generation = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitialisePopulation();
        print(population);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitialisePopulation()
    {
        for (int i = 0; i < populationSize; i++)
        {
            //choose 20 random attacks for the fighter
            GeneticAlgorithm fighter = new(GenRandomsAttacks());
            
            population.Add(fighter);
        }
    }
    public List<int> GenRandomsAttacks() {
        List<int> Result = new List<int>();
        for (int i = 0; i < 20; i++)
        {
            Result.Add(Random.Range(0, 5));
        }

        return Result;
    }
}
