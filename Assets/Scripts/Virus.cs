using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Virus : MonoBehaviour
{
    private Dictionary<GameObject, float> distance = new Dictionary<GameObject, float>(3);
    private Renderer[] rd;
    private CapsuleCollider cld;

    private void Start()
    {
        rd = new[]
        {
            transform.GetChild(0).GetComponent<Renderer>(),
            transform.GetChild(1).GetComponent<Renderer>()
        };
        
        cld = GetComponents<CapsuleCollider>()[1];
        name = "Infected";
        
        Get.Healthy--;
        Get.NotKnow++;
        Get.MaxSicked++;
        
        cld.enabled = true;
        foreach (Renderer r in rd)
        {
            r.material.color = Color.yellow;
        }

        StartCoroutine(FindVirus());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name != "Healthy") return;
        distance.Add(other.gameObject, Vector3.Distance(transform.position, other.transform.position)-1);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.name != "Healthy") return;
        
        float dist = Vector3.Distance(transform.position, other.transform.position)-1;
        if (dist < distance[other.gameObject])
        {
            distance[other.gameObject] = dist;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name != "Healthy") return;
        if (!distance.ContainsKey(other.gameObject)) return;
        
        if (Random.Range(0, 100) < 75)
        {
            // Шанс кашля
            float dist = distance[other.gameObject];
            if (Get.IsMask && Random.Range(0, 100) < Get.ChanceMask)
            {
                if (Random.Range(0, 100) < 25) other.GetComponent<Virus>().enabled = true;
            }
            else
            {
                if (Random.Range(0f, 100f) < -5 * dist * dist + 15 * dist + 60)
                    other.GetComponent<Virus>().enabled = true;
            }
        }

        distance.Remove(other.gameObject);
    }

    private IEnumerator FindVirus()
    {
        yield return new WaitForSeconds(Random.Range(Get.DayTime * 2, Get.DayTime * 4));
        
        if (Get.NotKnow < 4 && !Get.IsMask)
        {
            StartCoroutine(FindVirus());
            yield break;
        }
        
        Get.NotKnow--;
        Get.Know++;

        foreach (Renderer r in rd)
        {
            r.material.color = Color.red;
        }

        if (Get.Quarantine) {
            Hospital.ToQuarantine(gameObject);
            cld.enabled = false;
        }
        
        StartCoroutine(Recovery());
    }

    private IEnumerator Recovery()
    {
        yield return new WaitForSeconds(Random.Range(Get.DayTime * 11, Get.DayTime * 15));
        
        if (Random.Range(0, 100) < 2)
        {
            Get.Know--;
            Get.Dead++;
            Dead.Die(gameObject);
        }
        else
        {
            name = "Immunity";
            foreach (Renderer r in rd)
            {
                r.material.color = Color.green;
            }

            Get.Know--;
            Get.Healthy++;
            
            transform.position = Spawn.Pos + new Vector3(Random.Range(0, Spawn.Size.x), 0, Random.Range(0, Spawn.Size.z));
            StartCoroutine(Immunity());
        }
    }

    private IEnumerator Immunity()
    {
        yield return new WaitForSeconds(Random.Range(Get.DayTime * 10, Get.DayTime * 25));
        
        foreach (Renderer r in rd)
        {
            r.material.color = Color.white;
        }

        name = "Healthy";
        GetComponent<Virus>().enabled = false;
    }
}