using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileParticles : MonoBehaviour
{
    public enum TileSideType
    {
        Top,
        Bottom,
        Left,
        Right
    };

    // Particle System and tile side
    public TileSideType side;
    public ParticleSystem particles;

    // List of all the particles
    private Tilemap tilemap;
    private List<ParticleSystem> allTheParticles = new List<ParticleSystem>();

    // Used to set the direction
    private int rotationAngle; 
    private float offsetX, offsetY, neighborOffsetX, neighborOffsetY;

    void Start()
    {
        tilemap = GetComponent<Tilemap>();
        side = TileSideType.Top;
        CreateParticles();
    }

    public void RegenParticles(TileSideType newSide)
    {
        side = newSide;
        RemoveParticles();
        CreateParticles();
    }

    public void RegenParticles()
    {
        RemoveParticles();
        CreateParticles();
    }

    void CreateParticles()
    {
        // Figures out the values for transposition and rotation.
        switch (side)
        {
            case TileSideType.Top:
                neighborOffsetX = 0;
                neighborOffsetY = tilemap.cellSize.y;
                offsetX = 0;
                offsetY = tilemap.cellSize.y;
                rotationAngle = 0;
                break;
            case TileSideType.Bottom:
                neighborOffsetX = 0;
                neighborOffsetY = -tilemap.cellSize.y;
                offsetX = tilemap.cellSize.x;
                offsetY = 0;
                rotationAngle = 180;
                break;
            case TileSideType.Left:
                neighborOffsetX = -tilemap.cellSize.x;
                neighborOffsetY = 0;
                offsetX = 0;
                offsetY = 0;
                rotationAngle = 90;
                break;
            case TileSideType.Right:
                neighborOffsetX = tilemap.cellSize.x;
                neighborOffsetY = 0;
                offsetX = tilemap.cellSize.x;
                offsetY = tilemap.cellSize.y;
                rotationAngle = 270;
                break;
            default:
                break;
        }

        foreach (var position in tilemap.cellBounds.allPositionsWithin)
        {
            if (!tilemap.HasTile(position))
            {
                continue;
            }
            if (tilemap.HasTile(new Vector3Int((int)(position.x + neighborOffsetX), (int)(position.y + neighborOffsetY), 0)))
            {
                continue;
            }

            allTheParticles.Add(Instantiate(particles, new Vector3(position.x + offsetX, position.y + offsetY), new Quaternion()));

            
            foreach(ParticleSystem particle in allTheParticles)
            {
                particle.transform.eulerAngles = new Vector3Int(0, 0, rotationAngle);
            }
            
        }
    }

    void RemoveParticles()
    {
        foreach(ParticleSystem particle in allTheParticles)
        {
            Destroy(particle);
        }
    }
}
