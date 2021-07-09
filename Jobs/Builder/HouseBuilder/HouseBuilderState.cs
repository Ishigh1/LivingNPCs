namespace LivingNPCs.Jobs.Builder.HouseBuilder
{
	public enum HouseBuilderState
	{
		LookingForNewHouseEmplacement,
		WaitingForCleanSpot,
		SearchingNextTile,
		GoingToNextTile,
		Building,
		Finished
	}
}