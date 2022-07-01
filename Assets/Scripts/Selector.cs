using System.Collections.Generic;
using UnityEngine;

public class Selector : MonoBehaviour
{
    [SerializeField] List<SelectorObject> objects;
    private List<int> chance = new List<int>();
    private void Awake()
    {
        int lcm = 1;
        for(int i = 0; i<objects.Count; i++)
        {
            lcm = LCM(lcm, objects[i].denominator);    
        }
        
        //print(lcm);
        for (int i = 0; i < objects.Count; i++)
        {
            int weight = objects[i].numerator *(lcm/objects[i].denominator);
            //print($"Peso do Elemento {i} é {weight}");
            for (int j = 0; j < weight; j++) chance.Add(i);
        }

        if (chance.Count != lcm) Debug.LogError("Soma das porcentagens nao e 1");
    }

    public GameObject GetRandomElement()
    {
        return objects[chance[Random.Range(0, chance.Count)]].myObject;
    }
    
    int GCD(int a, int b)
    {
        if (b == 0)
            return a;
        return GCD(b, a % b);
    }

    // Function to return LCM of two numbers
    int LCM(int a, int b)
    {
        return (a / GCD(a, b)) * b;
    }
}

