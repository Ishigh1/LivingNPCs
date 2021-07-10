using System;
using System.Collections.Generic;

namespace LivingNPCs.Jobs
{
	public class JobCollection
	{
		public Dictionary<Type, Job> Jobs;
		public Job ActiveJob;

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
			ActiveJob = Jobs[typeof(TJob)];
		}

		public TJob GetJob<TJob>() where TJob : Job
		{
			return (TJob) Jobs[typeof(TJob)];
		}
	}
}