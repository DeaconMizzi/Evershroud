using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuestSystem;


public class PlayerSimple : MonoBehaviour
{
    [Header("Interaction")]
    public float radius;
    

    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("e"))
        {

            Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius, Physics.AllLayers, QueryTriggerInteraction.Collide);
            if(hitColliders.Length > 0)
            {
                for (int i = 0; i < hitColliders.Length; i++)
                {
                    IQuestInteraction[] interactions = hitColliders[i].GetComponents<IQuestInteraction>();
                    if(interactions.Length > 0)
                    {
                        for (int j = 0; j < interactions.Length; j++)
                        {
                            interactions[j].Interact();
                        }
                    }
                }
            }
        }

        
    }


}
