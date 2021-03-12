using UnityEngine;

/// <summary>
/// handles playing random footstep sound based on terrain texture
/// </summary>
public class Footsteps : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] stoneClips;
    [SerializeField]
    private AudioClip[] sandClips;
    [SerializeField]
    private AudioClip[] grassClips;

    private AudioSource audioSource;
    private TerrainDetector terrainDetector;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        terrainDetector = new TerrainDetector();
    }

    private void Step()
    {
        AudioClip clip = GetRandomClip();
        audioSource.PlayOneShot(clip);
    }

    private AudioClip GetRandomClip()
    {
        int terrainTextureIndex = terrainDetector.GetActiveTerrainTextureIdx(transform.position);

        switch (terrainTextureIndex)
        {
            case 0:
            default:
                return grassClips[UnityEngine.Random.Range(0, grassClips.Length)];
            case 1:
                return sandClips[UnityEngine.Random.Range(0, sandClips.Length)];
            case 2:
            case 3:
                return stoneClips[UnityEngine.Random.Range(0, stoneClips.Length)];
        }
    }
}