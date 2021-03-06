﻿using System.Collections;
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
    public TileBase backTile;

    // List of all the particles
    private Tilemap tilemap;
    private List<ParticleSystem> allTheParticles = new List<ParticleSystem>();

    // Used to set the direction
    private int rotationAngle;
    private float offsetX, offsetY, neighborOffsetX, neighborOffsetY;

    void Awake()
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

    public void GenParticles(TileSideType newSide)
    {
        side = newSide;
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
            TileBase currentTile = tilemap.GetTile(new Vector3Int(position.x, position.y, 0));
            TileBase neighBorTile = tilemap.GetTile(new Vector3Int((int)(position.x + neighborOffsetX), (int)(position.y + neighborOffsetY), 0));

            if (!tilemap.HasTile(position) || currentTile == backTile)
            {
                continue;
            }
            if (tilemap.HasTile(new Vector3Int((int)(position.x + neighborOffsetX), (int)(position.y + neighborOffsetY), 0)) && neighBorTile != backTile)
            {
                continue;
            }

            ParticleSystem instObj = Instantiate(particles, new Vector3(0, 0, 0), new Quaternion());
            instObj.transform.SetParent(transform.parent);
            instObj.transform.localPosition = new Vector2(position.x + offsetX, position.y + offsetY);
            instObj.transform.eulerAngles = new Vector3Int(0, 0, 0);

            allTheParticles.Add(instObj);
            
        }
    }

    public void RemoveParticles()
    {
        foreach(ParticleSystem particle in allTheParticles)
        {
            Destroy(particle.gameObject);
        }

        allTheParticles.Clear();
    }
}