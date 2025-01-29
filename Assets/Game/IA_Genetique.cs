using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class IA_Genetique : MonoBehaviour
{
    public GIFPlayer gifPlayer; // Référence au script qui joue le GIF

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

    public float EvaluateFitness(Deck iaDeck, Deck playerDeck)
    {
        var combat = new Combat();

        // Initialiser les points de vie (valeurs de départ)
        int initialPlayerHealth = 100;
        int initialIAHealth = 100;

        while(iaDeck.Cards.Count <= 19)
        {
            iaDeck.Cards.Add(new Card(ActionType.NormalAttack));
        }

        // Faire le combat entre les decks
        for (int i = 0; i < 20; i++)
        {
            combat.ResolveTurn(playerDeck.Cards[i], iaDeck.Cards[i]); // Résolution du tour

            if (combat.IsGameOver()) // Arrêter si le jeu est fini
                break;
        }

        // Points de vie restants
        int finalPlayerHealth = combat.PlayerHealth;
        int finalIAHealth = combat.IAHealth;

        // Calcul de la fitness
        int playerHealthLost = initialPlayerHealth - finalPlayerHealth; // Points de vie perdus par le joueur
        int iaHealthLost = initialIAHealth - finalIAHealth;            // Points de vie perdus par l'IA

        float fitness = playerHealthLost - iaHealthLost;

        // Log pour debugging
        Debug.Log($"Fitness calculée : {fitness} | Vie joueur : {finalPlayerHealth} | Vie IA : {finalIAHealth}");

        return fitness;
    }

    public List<Deck> SelectParents(List<Deck> population, List<float> fitness)
    {
        var selected = new List<Deck>();

        // Exemple : prendre 50 % des meilleurs decks, mais de manière aléatoire
        var sorted = population.Zip(fitness, (deck, fit) => new { deck, fit })
                               .OrderByDescending(x => x.fit)
                               .Take(population.Count / 2)
                               .ToList();

        // Mélanger les parents sélectionnés
        var randomOrder = sorted.OrderBy(x => Random.value).ToList();
        foreach (var item in randomOrder)
            selected.Add(item.deck);

        return selected;
    }


    public Deck Crossover(Deck parent1, Deck parent2)
    {
        var child = new Deck();

        for (int i = 0; i < 20; i++)
        {
            // Mélanger les cartes avec une chance égale
            if (Random.value > 0.5)
                child.AddCard(parent1.Cards[i]);
            else
                child.AddCard(parent2.Cards[i]);
        }

        return child;
    }

    public void Mutate(Deck deck, float mutationRate)
    {
        int n = Random.Range(0, deck.Cards.Count);
        deck.Cards[n] = new Card((ActionType)Random.Range(0, 5));

        for (int i = 0; i < deck.Cards.Count; i++)
        {
            if (Random.value < mutationRate)
            {
                // Remplacer une carte aléatoire par une nouvelle carte
                var newAction = (ActionType)Random.Range(0, 7); // Inclut toutes les actions possibles
                deck.Cards[i] = new Card(newAction);
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
            var parent1 = parents[0];
            var parent2 = parents[1];
            var child = Crossover(parent1, parent2);
            Mutate(child, mutationRate);
            newGeneration.Add(child);
        }

        return newGeneration;
    }

    public IEnumerator RunGeneticAlgorithm()
    {
        gifPlayer.StartGIF(); // Démarre le GIF

        int populationSize = 100;
        int maxGenerations = 1000;
        float mutationRate = 0.5f;

        var playerDeck = GameManager.Instance.GetPlayerDeckAsDeck(); // Récupère le deck du joueur
        var population = GenerateInitialPopulation(populationSize);

        // Nom du fichier pour stocker les résultats
        string filePath = "Assets/test.txt";

        // S'assurer que le fichier est vide au début
        using (var writer = new StreamWriter(filePath, false))
        {
            writer.WriteLine("Résultats de l'algorithme génétique :");
        }

        for (int generation = 0; generation < maxGenerations; generation++)
        {
            // Évaluer chaque deck
            var fitness = population.Select(deck => EvaluateFitness(deck, playerDeck)).ToList();

            // Identifier le deck avec la meilleure fitness
            float bestFitness = fitness.Max();
            int bestIndex = fitness.IndexOf(bestFitness);
            var bestDeck = population[bestIndex];

            // Simuler le combat pour capturer les PV finaux du joueur et de l'IA
            var combat = new Combat();
            for (int i = 0; i < 20; i++)
            {
                combat.ResolveTurn(playerDeck.Cards[i], bestDeck.Cards[i]);
                if (combat.IsGameOver()) break;
            }
            int finalPlayerHealth = combat.PlayerHealth;
            int finalIAHealth = combat.IAHealth;

            // Écrire les résultats de la génération dans le fichier
            using (var writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine($"Génération {generation}");
                writer.WriteLine($"Meilleure fitness : {bestFitness}");
                writer.WriteLine($"Vie finale du joueur : {finalPlayerHealth}");
                writer.WriteLine($"Vie finale de l'IA : {finalIAHealth}");
                writer.WriteLine("Cartes du meilleur deck :");
                foreach (var card in bestDeck.Cards)
                {
                    writer.WriteLine($"- {card.Action}");
                }
                writer.WriteLine(); // Ligne vide pour séparer les générations
            }

            // Vérifier la condition d'arrêt
            if (finalPlayerHealth <= 0 && finalIAHealth > 0)
            {
                Debug.Log($"Condition d'arrêt atteinte à la génération {generation} !");
                break;
            }

            // Sélectionner les parents et créer la nouvelle génération
            var parents = SelectParents(population, fitness);
            population = CreateNewGeneration(parents, populationSize, mutationRate);

            // Attendre une frame avant de passer à la prochaine génération
            yield return null;
        }

        gifPlayer.StopGIF(); // Arrête le GIF

        Debug.Log($"Les résultats ont été écrits dans : {filePath}");
    }


    // Nouvelle méthode pour vérifier la condition d'arrêt
    private bool CheckStopCondition(float fitness, List<Deck> population, Deck playerDeck)
    {
        foreach (var iaDeck in population)
        {
            var combat = new Combat();
            for (int i = 0; i < 20; i++)
            {
                combat.ResolveTurn(playerDeck.Cards[i], iaDeck.Cards[i]);
                if (combat.PlayerHealth <= 0 && combat.IAHealth > 0)
                {
                    // Si la condition est remplie (joueur à 0 PV et IA > 0 PV), on arrête
                    return true;
                }
            }
        }
        return false; // Sinon, la condition n'est pas atteinte
    }
}
