using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using static UnityEngine.Rendering.GPUSort;

public class IA_Genetique : MonoBehaviour
{
    public int maxCards = 20;
    public List<Deck> GenerateInitialPopulation(int populationSize)
    {
        var population = new List<Deck>();

        for (int i = 0; i < populationSize; i++)
        {
            var deck = new Deck();
            for (int j = 0; j < maxCards; j++)
            {
                var action = (ActionType)Random.Range(0, 5); // Choisit une action aléatoire
                if (action == ActionType.HeavyAttack && i + 1 < maxCards)
                {
                    deck.AddCard(new Card(ActionType.LoadHeavy));
                    deck.AddCard(new Card(action));
                    i++;
                }
                else if (action == ActionType.HeavyAttack || action == ActionType.Heal)
                {
                    deck.AddCard(new Card(ActionType.NormalAttack));
                } else
                {
                    deck.AddCard(new Card(action));
                }
            }
            population.Add(deck);
        }

        return population;
    }

    public float EvaluateFitness(Deck iaDeck, Deck playerDeck)
    {
        var combat = new Combat();
        int turnsSurvived = 0;

        for (int i = 0; i < maxCards; i++)
        {
            combat.ResolveTurn(playerDeck.Cards[i], iaDeck.Cards[i]);
            turnsSurvived++;
            if (combat.IsGameOver())
                break;
        }

        // Calculer la fitness en fonction de la santé restante et des tours joués
        float fitness = (combat.PlayerHealth <= 0 ? 1f : 0f) + (turnsSurvived / maxCards);
        return fitness;
    }

    public List<Deck> SelectParents(List<Deck> population, List<float> fitness)
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

        var card = new Card(ActionType.NormalAttack);
        for (int i = 0; i < maxCards; i++)
        {
            if (Random.value > 0.5)
            {
                if (parent1.Cards[i].Equals(ActionType.Heal) || parent1.Cards[i].Equals(ActionType.HeavyAttack))
                {
                    if (!parent2.Cards[i].Equals(ActionType.Heal) && !parent2.Cards[i].Equals(ActionType.HeavyAttack))
                    {
                        card = parent1.Cards[i];
                    }
                }
                else if (parent2.Cards[i].Equals(ActionType.Heal) || parent2.Cards[i].Equals(ActionType.HeavyAttack))
                {
                    if (!parent1.Cards[i].Equals(ActionType.Heal) && !parent1.Cards[i].Equals(ActionType.HeavyAttack))
                    {
                        card = parent1.Cards[i];
                    }
                }
            }

            child.AddCard(card);
        }

        return child;
    }

    public void Mutate(Deck deck, float mutationRate)
    {
        for (int i = 0; i < maxCards; i++)
        {
            if (Random.value < mutationRate)
            {
                var action = (ActionType)Random.Range(0, 5);
                if (action == ActionType.HeavyAttack && i + 1 < maxCards)
                {
                    deck.Cards[i] = new Card(ActionType.LoadHeavy);
                    deck.Cards[i + 1] = new Card(action);
                    i++;
                }
                else if (action == ActionType.HeavyAttack || action == ActionType.Heal)
                {
                    deck.Cards[i] = new Card(ActionType.NormalAttack);
                }
                else
                {
                    deck.AddCard(new Card(action));
                }
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

    struct FitnessJob : IJobParallelFor
    {
        [ReadOnly] public NativeArray<Card> playerDeck;
        [ReadOnly] public NativeArray<Card> iaDeck;
        [WriteOnly] public NativeArray<float> fitnessResults;
        public int maxCards;

        public void Execute(int index)
        {
            var combat = new Combat();
            int turnsSurvived = 0;

            int deckIndex = index / maxCards; // Which deck in the population
            int cardIndex = index % maxCards; // Which card in the deck


            combat.ResolveTurn(playerDeck[cardIndex], iaDeck[deckIndex * maxCards + cardIndex]);
            turnsSurvived++;

            // Calculate fitness based on health and turns survived
            float fitness = (combat.PlayerHealth <= 0 ? 1f : 0f) + (turnsSurvived / (float)maxCards);
            fitnessResults[index] = fitness;
        }
    }

    public void RunGeneticAlgorithm()
    {
        int populationSize = 10000;
        int maxGenerations = 1000;
        float mutationRate = 0.1f;

        var playerDeck = GameManager.Instance.GetPlayerDeckAsDeck(); // Récupère le deck du joueur
        maxCards = playerDeck.Cards.Count;
        var population = GenerateInitialPopulation(populationSize);

        for (int generation = 0; generation < maxGenerations; generation++)
        {
            // Évaluer chaque deck
            //var fitness = population.Select(deck => EvaluateFitness(deck, playerDeck)).ToList();

            NativeArray<Card> playerDeckNative = new NativeArray<Card>(playerDeck.Cards.ToArray(), Allocator.TempJob);
            NativeArray<float> fitnessResults = new NativeArray<float>(population.Count, Allocator.TempJob);

            var fitnessJob = new FitnessJob
            {
                playerDeck = playerDeckNative,
                iaDeck = new NativeArray<Card>(population.SelectMany(deck => deck.Cards).ToArray(), Allocator.TempJob),
                fitnessResults = fitnessResults,
                maxCards = maxCards
            };

            JobHandle jobHandle = fitnessJob.Schedule(population.Count, 64);  // 64 is the batch size
            jobHandle.Complete();  // Wait for the job to finish

            // Convert fitness results back to a list
            List<float> fitness = fitnessResults.ToList();

            Debug.Log($"Génération {generation} : Meilleure fitness = {fitness.Max()} | Moyenne = {fitness.Average()}");

            // Clean up NativeArrays
            playerDeckNative.Dispose();
            fitnessResults.Dispose();


            // Vérifier si une solution parfaite est trouvée
            if (fitness.Max() >= 1.5f) // Exemple de condition basée sur la fitness
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
