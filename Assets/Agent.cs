using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    
    public Species species;
    public float mass;
    AgentState currentState;

    Vector2 direction;
    float reproducingTimer;

    Agent currentTarget;

    float ageTimer;

    // Start is called before the first frame update
    void Start()
    {
        currentState = AgentState.WANDERING;
        direction = new Vector2(0,1);
        mass = species.babySize * Random.Range(0.8f, 1.2f);
        GetComponent<SpriteRenderer>().color = species.color;

        float size = Mathf.Lerp(0.2f, 0.5f, species.maxSize / 270f);
        transform.localScale = new Vector3(size, size, 1f);
        ageTimer = Random.Range(40f, 100f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!WorldManager.instance.isRunning)
        {
            return;
        }
        switch (currentState)
        {
            case AgentState.WANDERING:
            Wander();
            break;
            case AgentState.EATING:
            Eat(currentTarget);
            break;
            case AgentState.DEAD:
            break;
            case AgentState.REPRODUCING:
            Reproducing();
            break;
            case AgentState.CHASING:
            Chasing(currentTarget);
            break;
            default:
            break;
        }

        UpdateState();
    }


    void UpdateState(){
        mass -= Time.deltaTime*0.2f;
        ageTimer -= Time.deltaTime;

        if(mass<species.deathThreshold || ageTimer<0){
            currentState = AgentState.DEAD;
            WorldManager.instance.worldNutrients += mass;
            Destroy(gameObject);
            return;
        }

        if(mass>species.reproducingThreshold && currentState != AgentState.REPRODUCING){
            currentState = AgentState.REPRODUCING;
            reproducingTimer = 1f;
            return;
        }

        

        

        
    }

    void Eat(Agent foodSource){
        if(species.SpeciesType == SpeciesType.PLANT)
        {
            if (mass < species.maxSize && WorldManager.instance.worldNutrients>0)
            {
                mass += species.eatSpeed * Time.deltaTime;
                WorldManager.instance.worldNutrients -= species.eatSpeed * Time.deltaTime*1.1f;
            }
            return;
        }

        if(foodSource != null && mass < species.maxSize && foodSource.mass > 0){
            mass += species.eatSpeed*Time.deltaTime;
            foodSource.mass -= species.eatSpeed * Time.deltaTime;
        }
        else
        {
            currentState = AgentState.WANDERING;
        }
        
    }

    void Wander(){
        if (species.SpeciesType == SpeciesType.PLANT)
        {
            currentState = AgentState.EATING;
            return;
        }
        direction = Quaternion.Euler(0,0,Random.Range(-5f,5f))*direction;
        Vector3 drift = transform.position.sqrMagnitude > WorldManager.instance.WorldRadius * WorldManager.instance.WorldRadius ? -transform.position : Vector3.zero;
        
        transform.position += ((Vector3)direction+drift*0.5f).normalized*Time.deltaTime*species.speed;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, species.lookRange);

        foreach (Collider2D c in colliders)
        {
            Agent a = c.GetComponent<Agent>();
            if (a == null)
            {
                continue;
            }
            bool correctFood = (species.SpeciesType == SpeciesType.HERBIVORE && a.species.SpeciesType == SpeciesType.PLANT)
                || (species.SpeciesType == SpeciesType.CARNIVORE && a.species.SpeciesType == SpeciesType.HERBIVORE);

            if (correctFood)
            {
                currentTarget = a;
                currentState = AgentState.CHASING;
            }
        }
    }

    void Reproducing(){
        if(reproducingTimer<0){
            if (WorldManager.instance.numberOfIndividuals < 300)
            {
                SoundManager.instance.PlayPop();
                GameObject go = Instantiate(gameObject, transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0), Quaternion.identity, this.transform.parent);

            }
            mass -= species.babySize;
            currentState = AgentState.WANDERING;
        }

        reproducingTimer-=Time.deltaTime;
    }

    void Chasing(Agent target){
        if(target == null || target.currentState == AgentState.DEAD)
        {
            currentState = AgentState.WANDERING;
            return;
        }
        direction = (target.transform.position-transform.position);
        transform.position += (Vector3)direction.normalized*Time.deltaTime*species.speed;

        if(direction.sqrMagnitude < 0.2*0.2f){
            target.currentState = AgentState.DEAD;
            currentState = AgentState.EATING;
        }
    }

}

public enum AgentState{
    WANDERING,
    EATING,
    DEAD,
    REPRODUCING,
    CHASING
}
