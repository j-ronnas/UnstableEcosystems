using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldManager : MonoBehaviour
{
    public float WorldRadius;

    [SerializeField]
    Species[] startingSpecies;
    [SerializeField]
    GameObject agentPrefab;

    public static WorldManager instance;

    OptionPanel[] optionPanels;

    public bool isRunning;

    float choiceTimer;

    [SerializeField]
    Text counterText;

    public int numberOfIndividuals;

    public float worldNutrients;
    // Start is called before the first frame update
    void Start()
    {
        isRunning = true;
        optionPanels = FindObjectsOfType<OptionPanel>();
        optionPanels[0].transform.parent.gameObject.SetActive(false);

        worldNutrients = 500f;
        instance = this;
        WorldRadius = 5f;
        GenerateStartingWorld();

        choiceTimer = 10f;

        


    }

    // Update is called once per frame
    void Update()
    {
        if (!isRunning)
        {
            return;
        }
        
        WorldRadius = Mathf.Lerp(3f, 15f, transform.childCount / 250f);
        numberOfIndividuals = transform.childCount;
        counterText.text = "Number of organisms: " + numberOfIndividuals;
        //Camera.main.orthographicSize = WorldRadius;
        choiceTimer -= Time.deltaTime;
        if (choiceTimer < 0)
        {
            isRunning = false;
            CreateChoice();
            choiceTimer = 30f;
        }
    }

    void GenerateStartingWorld()
    {
        int c = 8;
        foreach (Species s in startingSpecies)
        {
            
            SpawnAgents(s, c);
            c = Mathf.RoundToInt(c*0.5f);
        }
    }



    void CreateChoice()
    {
        foreach (OptionPanel op in optionPanels)
        {
            op.gameObject.transform.parent.gameObject.SetActive(true);
            Species s = GenerateRandomSpecies();
            op.SetupOptionPanel(CreateInfo(s));
            op.GetComponentInChildren<Button>().onClick.RemoveAllListeners();

            int count = 8;
            if(s.SpeciesType == SpeciesType.HERBIVORE)
            {
                count = 4;
            }
            else if(s.SpeciesType == SpeciesType.CARNIVORE)
            {
                count = 4;
            }

            op.GetComponentInChildren<Button>().onClick.AddListener(()=> { SpawnAgents(s, count); op.gameObject.transform.parent.gameObject.SetActive(false); isRunning = true; });


        }
    }

    SpeciesInfo CreateInfo(Species species)
    {
        SpeciesInfo info = new SpeciesInfo();

        info.speciesType = species.SpeciesType;
        info.color = species.color;
        info.reproduceRating = species.reproducingThreshold / 150f;
        info.survivabilityRating = (species.maxSize - species.deathThreshold) / 250f;
        info.speed = species.speed / 2f;
        info.size = species.maxSize / 270f;

        return info;
    }

    void SpawnAgents(Species s, int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject go = Instantiate(agentPrefab, new Vector3(Random.Range(-WorldRadius, WorldRadius), Random.Range(-WorldRadius, WorldRadius), 0), Quaternion.identity, this.transform);
            go.GetComponent<Agent>().species = s;
            SoundManager.instance.PlayPop();
            //worldNutrients -= s.babySize;
        }
    }

    Species GenerateRandomSpecies()
    {
        Species species = new Species();

        species.SpeciesType = (SpeciesType)Random.Range(0, 3);

        species.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        species.reproducingThreshold = Random.Range(50f, 150f);
        species.deathThreshold = Random.Range(5f, 20f);
        species.speed = species.SpeciesType == SpeciesType.PLANT ? 0: Random.Range(0.2f, 2f);
        species.maxSize = species.reproducingThreshold * Random.Range(1.1f, 1.8f);
        species.eatSpeed = Random.Range(0.5f, 2f);
        species.babySize = species.maxSize * Random.Range(0.4f, 0.5f);
        species.lookRange = Random.Range(0.2f, 3f);

        return species;
    }
}

public struct SpeciesInfo{
    public SpeciesType speciesType;
    public Color color;
    public float reproduceRating;
    public float survivabilityRating;
    public float speed;
    public float size;
}