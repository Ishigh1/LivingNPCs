using System;
using System.Collections.Generic;
using LivingNPCs.NPCs;
using Microsoft.Xna.Framework;

namespace LivingNPCs.Jobs
{
	public class JobCollection
	{
		public EasierNPC EasierNPC;
		public Job ActiveJob;
		public Dictionary<Type, Job> Jobs;

		public JobCollection()
		{
			Jobs = new Dictionary<Type, Job>();
			ActiveJob = null;
		}

		public void AddJob(Job job)
		{
			Jobs.Add(job.GetType(), job);
		}

		public void SetJobToActive<TJob>() where TJob : Job
		{
			SetJobToActive(typeof(TJob));
		}

		public void SetJobToActive(Type jobType)
		{
			ActiveJob = Jobs[jobType];
			if (ActiveJob.CachedObjective.location != Point.Zero) 
				EasierNPC.SetObjective(ActiveJob.CachedObjective);
		}

		public TJob GetJob<TJob>() where TJob : Job
		{
			return (TJob) Jobs[typeof(TJob)];
		}
	}
}