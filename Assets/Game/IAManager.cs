using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAManager : MonoBehaviour {
	
	public GameObject IAPrefab;
	//public GameObject target;
	
	private bool isTraning = false;
	public int populationSize = 50;
	public float timer = 15f;
	private int generationNumber = 0;
	private int[] layers = new int[] { 6, 5, 4, 2 }; //1 input and 1 output
	private List<NeuralNetwork> nets;
	private List<NeuralNetwork> newNets;
	public List<GameObject> IAList = null;

	public float maxFitness = 0f;
	
	void Timer()
	{
		isTraning = false;
	}
	
	
	void Update ()
	{
		if (isTraning == false)
		{
			if (generationNumber == 0)
			{
				InitIANeuralNetworks();
				CreateIABodies();
				isTraning = true;
			}
			else
			{
				
				isTraning = true;
				
				for(int i=0; i<populationSize; i++)
				{
					SimpleIA script = IAList[i].GetComponent<SimpleIA>();
					float fitness = script.fitness;
					nets[i].SetFitness(fitness);
					Debug.Log(fitness);
				}

				for(int i=0; i<populationSize; i++)
					Debug.Log(nets[i].GetFitness());
				nets.Sort();

				for(int i=0; i<populationSize; i++)
					Debug.Log(nets[i].GetFitness());



				List<NeuralNetwork> newNets = new List<NeuralNetwork>();

				for (int i = 0; i < populationSize/2; i++)
				{
					NeuralNetwork net = new NeuralNetwork(nets[i + (populationSize / 2)]);
					net.Mutate();
					newNets.Add (net);

				}

				for (int i = 0; i < populationSize/2; i++)
				{
					NeuralNetwork net2 = new NeuralNetwork(nets[i+ (populationSize / 2)]);
					
					net2.Mutate();
					net2.Mutate();
					net2.Mutate();
					net2.Mutate();

					newNets.Add (net2);
				}

				nets = newNets;
				//nets.Sort();
				//for (int i = 0; i < populationSize / 2; i++)
				//{
				//	nets[i] = new NeuralNetwork(nets[i+(populationSize / 2)]);
				//	nets[i].Mutate();
				//	
				//	nets[i + (populationSize / 2)] = new NeuralNetwork(nets[i + (populationSize / 2)]); //too lazy to write a reset neuron matrix values method....so just going to make a deepcopy lol
				//}

				//for (int i = 0; i < populationSize; i++)
				//{
				//	nets[i].SetFitness(0f);
				//}
			}
			

			generationNumber++;
			isTraning = true;
			Invoke("Timer",timer);
			CreateIABodies();
		}
		
		//bool isActive = false;
		//for(int i=0; i<populationSize; i++)
		//{
		//	SimpleIA script = IAList[i].GetComponent<SimpleIA>();
		//	if(script.active==true)
		//	{
		//		isActive=true;
		//		break;
		//	}
		//	
		//}
		//if(isActive == false)
		//{
		//	generationNumber++;
		//	isTraning = true;
		//	Timer();
		//	CreateIABodies();
		//}
		//------------------FEEDFORWARD
		for (int i = 0; i < populationSize; i++)
		{
			SimpleIA script = IAList[i].GetComponent<SimpleIA>();

			float[] result;

			PlayerStat stat = script.stat;


			float tInputs = stat.health;
			result = nets[i].FeedForward(tInputs);
			script.results = result;
		}

		if (Input.GetKeyDown("space"))
		{
			generationNumber++;
			isTraning = true;
			Timer ();
			CreateIABodies();
		}
	}
	
	//void sortByFitness(NeuralNetwork net)
	//{
	//	//float[] fitnesses;
	//	//float[] dual;
	//	//for (int i=0; i<populationSize; i++) {
	//	//	dual = new float[]{i, net[i].fitness};
	//	//	fitnesses.Add(dual);
	//	//
	//	//}
	//
	//}

	private void CreateIABodies()
	{
		for (int i = 0; i < IAList.Count; i++)
		{
			Destroy(IAList[i]);
		}

		IAList = new List<GameObject>();
		
		for (int i = 0; i < populationSize; i++)
		{
			GameObject IA = Instantiate(IAPrefab, new Vector3(0f, 0f, 0f),IAPrefab.transform.rotation);
			//IAPrefab.Init(nets[i],hex.transform);
			IAList.Add(IA);
			IAList[i] = IA;
		}
		
	}	

	void InitIANeuralNetworks()
	{
		nets = new List<NeuralNetwork>();

		for (int i = 0; i < populationSize; i++)
		{
			NeuralNetwork net = new NeuralNetwork(layers);
			net.Mutate();
			nets.Add(net);
		}
	}
}
