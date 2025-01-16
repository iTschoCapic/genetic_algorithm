﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IA_Genetique : MonoBehaviour
{
    public List<Deck> GenerateInitialPopulation(int populationSize)
    {
        var population = new List<Deck>();

        for (int i = 0; i < populationSize; i++)
        {
            var deck = new Deck();
            for (int j = 0; j < 20; j++)
            {
                var action = (ActionType)Random.Range(0, 5); // Choisit une action aléatoire
                deck.AddCard(new Card(action));
            }
            population.Add(deck);
        }

        return population;
    }

    public int EvaluateFitness(Deck iaDeck, Deck playerDeck)
    {
        var combat = new Combat();
        for (int i = 0; i < 20; i++)
        {
            combat.ResolveTurn(playerDeck.Cards[i], iaDeck.Cards[i]);
            if (combat.IsGameOver())
                break;
        }

        return combat.PlayerHealth <= 0 ? 1 : 0; // 1 si l'IA gagne, 0 sinon
    }

    public List<Deck> SelectParents(List<Deck> population, List<int> fitness)
    {
        var selected = new List<Deck>();

        // Exemple : on prend les 50 % meilleurs decks
        var sorted = population.Zip(fitness, (deck, fit) => new { deck, fit })
                               .OrderByDescending(x => x.fit)
                               .Take(population.Count / 2)
                               .ToList();

        foreach (var item in sorted)
            selected.Add(item.deck);

        return selected;
    }

    public Deck Crossover(Deck parent1, Deck parent2)
    {
        var child = new Deck();

        for (int i = 0; i < 20; i++)
        {
            var card = Random.value > 0.5 ? parent1.Cards[i] : parent2.Cards[i];
            child.AddCard(card);
        }

        return child;
    }

    public void Mutate(Deck deck, float mutationRate)
    {
        for (int i = 0; i < 20; i++)
        {
            if (Random.value < mutationRate)
            {
                var action = (ActionType)Random.Range(0, 5);
                deck.Cards[i] = new Card(action);
            }
        }
    }

    public List<Deck> CreateNewGeneration(List<Deck> parents, int populationSize, float mutationRate)
    {
        var newGeneration = new List<Deck>();

        // Ajouter les parents directement
        newGeneration.AddRange(parents);

        // Croisements pour générer des enfants
        while (newGeneration.Count < populationSize)
        {
            var parent1 = parents[Random.Range(0, parents.Count)];
            var parent2 = parents[Random.Range(0, parents.Count)];
            var child = Crossover(parent1, parent2);
            Mutate(child, mutationRate);
            newGeneration.Add(child);
        }

        return newGeneration;
    }

    public void RunGeneticAlgorithm()
    {
        int populationSize = 100;
        int maxGenerations = 100;
        float mutationRate = 0.1f;

        var playerDeck = GameManager.Instance.GetPlayerDeckAsDeck(); // Récupère le deck du joueur
        var population = GenerateInitialPopulation(populationSize);

        for (int generation = 0; generation < maxGenerations; generation++)
        {
            // Évaluer chaque deck
            var fitness = population.Select(deck => EvaluateFitness(deck, playerDeck)).ToList();

            // Vérifier si une solution parfaite est trouvée
            if (fitness.Max() == 1)
            {
                Debug.Log($"Solution trouvée à la génération {generation} !");
                break;
            }

            // Sélectionner les parents et créer la nouvelle génération
            var parents = SelectParents(population, fitness);
            population = CreateNewGeneration(parents, populationSize, mutationRate);
        }
    }


}
