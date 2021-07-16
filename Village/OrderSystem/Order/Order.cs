namespace LivingNPCs.Village.OrderSystem.Order
{
	public abstract class Order
	{
		private bool _completed;

		public bool Completed
		{
			get => _completed;
			set
			{
				if (value)
					OnCompleted();
				_completed = value;
			}
		}

		public virtual void OnCompleted()
		{
		}

		public bool CheckValidity()
		{
			bool isValid = !Completed && IsValid();
			if (!isValid)
				Completed = true;
			return isValid;
		}

		public abstract bool IsValid();
		public abstract bool IsAvailable();
	}
}