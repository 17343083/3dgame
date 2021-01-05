using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ring_of_Saturn : MonoBehaviour
{
	private ParticleSystem particle_system;
	private ParticleSystem.Particle[] particle;
	private Particle[] particles;

	public int count = 10000;
	public float size = 0.03f;
	public float minRadius = 2.0f;
	public float maxRadius = 5.0f;
	public bool flag = true;
	public float speed = 0.5f;
	public float range = 0.01f;

	public class Particle
	{
		public float radius = 0f, angle = 0f, time = 0f;
		public Particle(float x, float y, float z)
		{
			radius = x;
			angle = y;
			time = z;
		}
	}

	void Start()
	{
		particle = new ParticleSystem.Particle[count];
		particles = new Particle[count];

		particle_system = this.GetComponent<ParticleSystem>();
		particle_system.startSpeed = 0;
		particle_system.startSize = size;
		particle_system.loop = false;
		particle_system.maxParticles = count;
		particle_system.Emit(count);
		particle_system.GetParticles(particle);

		init();
	}

	void init()
	{
		for (int i = 0; i < count; ++i)
		{
			float midRadius = (maxRadius + minRadius) / 2;
			float minRate = Random.Range(1.0f, midRadius / minRadius);
			float maxRate = Random.Range(midRadius / maxRadius, 1.0f);
			float radius = Random.Range(minRadius * minRate, maxRadius * maxRate);
			float angle = Random.Range(0.0f, 360.0f);
			float theta = angle / 180 * Mathf.PI;
			float time = Random.Range(0.0f, 180.0f);

			particles[i] = new Particle(radius, angle, time);
			particle[i].position = new Vector3(particles[i].radius * Mathf.Cos(theta), 0f, particles[i].radius * Mathf.Sin(theta));
		}
		particle_system.SetParticles(particle, particle.Length);
	}

	void Update()
	{
		int tier = 10;
		for (int i = 0; i < count; i++)
		{
			if (flag)
				particles[i].angle -= (i % tier + 1) * (speed / particles[i].radius / tier);
			else
				particles[i].angle += (i % tier + 1) * (speed / particles[i].radius / tier);

			particles[i].angle = (360.0f + particles[i].angle) % 360.0f;
			float theta = particles[i].angle / 180 * Mathf.PI;

			particle[i].position = new Vector3(particles[i].radius * Mathf.Cos(theta), 0f, particles[i].radius * Mathf.Sin(theta));
			particles[i].time += Time.deltaTime;
			particles[i].radius += Mathf.PingPong(particles[i].time / minRadius / maxRadius, range) - range / 2.0f;
		}

		particle_system.SetParticles(particle, particle.Length);
	}
}
