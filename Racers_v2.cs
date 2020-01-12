void UpdateEveryRacer(float deltaTimeS,	List<Racer>	racers) 
{
	//	Апдейтим всех гонщиков (проверка IsAlive может быть необязательна, 
	//  если условием смерти является только коллизия с другим гонщиком)
	int racerIndex	= 0;
	for (racerIndex	= 1; racerIndex	<= racers.Count; racerIndex++)
	{
		//  Переводим дельту из милисекунд в секунды
		racers[racerIndex - 1].Update(deltaTimeS * 1000.0f);
	}
}

void CheckCollidesAndRemoveRacers(List<Racer>	racers, List<Racer> racersNeedingRemoved)
{
	//	Проверяем столкнулись ли гонщики
	//  Перебираем все НЕПОВТОРЯЮЩИЕСЯ случае взаимодействия гонщиков
	//  тем самым уменьшая количество проверок в 2 раза
	for (int racerIndex1 = 0; racerIndex1 < racers.Count; racerIndex1++)
	{
		for (int racerIndex2 = racerIndex1 + 1; racerIndex2 < racers.Count; racerIndex2++)
		{
			Racer racer1 = racers[racerIndex1];
			Racer racer2 = racers[racerIndex2];
			if (racer1.IsCollidable()	&&	racer2.IsCollidable()	&&	racer1.CollidesWith(racer2))
			{
				OnRacerExplodes(racer1);
                racers.Remove(racer1);
                racers.Remove(racer2);
			}
		}
	}
}

void UpdateRacers(float deltaTimeS,	List<Racer>	racers)
{
	List<Racer> racersNeedingRemoved = new List<Racer>();
	racersNeedingRemoved.Clear();

	UpdateEveryRacer(deltaTimeS,	racers);
	CheckCollidesAndRemoveRacers(racers, racersNeedingRemoved)
}