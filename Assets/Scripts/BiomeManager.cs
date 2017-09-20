using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomeManager : MonoBehaviour {
    public Biome[] biomes;
    public BiomeMarker[] biomeMarker;
    public Transform playerPos;

    public static Biome currentBiome;

    void Update()
    {
        foreach (var m in biomeMarker)
        {
            if (m.start.position.x < playerPos.position.x &&
                m.end.position.x > playerPos.position.x)
            {
                currentBiome = BiomeIDToBiome(m.biomeId);
                DebugPanel.Log("Current Biome", currentBiome.name);
                return;
            }
        }
        currentBiome = null;
        DebugPanel.Log("Current Biome", "null");
    }

    Biome BiomeIDToBiome(int id)
    {
        foreach (var b in biomes)
        {
            if (b.id == id)
                return b;
        }
        throw new System.ArgumentException();
    }
}

[System.Serializable]
public class Biome
{
    public string name;
    public int id;
    public DroppingItem[] droppingItems;
}

[System.Serializable]
public class DroppingItem
{
    public int itemId;
    public DropChance[] chances;

    [System.Serializable]
    public class DropChance
    {
        public int amount;
        public float chance;
    }
}

[System.Serializable]
public class BiomeMarker
{
    public Transform start;
    public Transform end;
    public int biomeId;
}

