using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Planet : MonoBehaviour {
    SpriteRenderer render;
    [SerializeField]
    float mass;
    float radius;
    public float angleToPlayerPlanet;
    PointEffector2D pointEffector;
    // Use this for initialization
    void Start() {
        pointEffector = GetComponent<PointEffector2D>();
        pointEffector.forceMagnitude = mass * -1;
        render = GetComponent<SpriteRenderer>();
        radius = render.size.x / 2;
    }

    public float GetMass()
    {
        return mass;
    }
    public float GetRadius()
    {
        return radius;
    }

    public Planet[] AllPlanets()
    {
        return FindObjectsOfType<Planet>();
    }

    public Planet[] ReachablePlanets()
    {
        List<Planet> planets = new List<Planet>();

        foreach (Planet p in AllPlanets())
        {
            RaycastHit2D[] hit;
            Vector2 dir = p.transform.position - transform.position;
            Ray2D ray = new Ray2D(transform.position, dir);
            hit = Physics2D.RaycastAll(ray.origin, ray.direction);
            CalculateAngle(transform.position, p.transform.position);
            if (hit.Length > 2) // to avoid overflow error which breaks the loop for some reason
            {
                if (hit[2].transform.name == p.name)
                {
                    planets.Add(p);
                }
            }
        }
        IOrderedEnumerable<Planet> orderedPlanets = planets.ToArray().OrderBy(x => x.angleToPlayerPlanet);
        return orderedPlanets.ToArray();
    }

    void CalculateAngle(Vector2 thisPos, Vector2 planetPos)
    {
        Vector2 diff = (planetPos - thisPos);
        float angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        angleToPlayerPlanet = angle;
    }
}
