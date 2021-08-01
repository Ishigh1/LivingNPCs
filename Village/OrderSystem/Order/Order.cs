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
				if (_completed)
					return;
				_completed = value;
				if (value)
					OnCompleted();
			}
		}

		public virtual void OnCompleted()
		{
#if DEBUG
			LivingNPCs.Writer.WriteLine("Completed order " + this);
#endif
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

#if DEBUG
		public override string ToString()
		{
			return Id + " (" + GetType() + ") : ";
		}
#endif
#if DEBUG
		public static int CurrentId;
		public int Id;

		protected Order()
		{
			Id = CurrentId++;
		}
#endif
	}
}