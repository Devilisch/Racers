/*
Рассмотрим	гипотетическую	гоночную	игру,	в	которой	сотни	автомобилей	мчатся	по	полю.	Метод	updateRacers,	
показанный	ниже,	обновляет	автомобили	и	уничтожает	те,	которые	сталкиваются.

a. Перепишите	метод,	чтобы	улучшить	его	читаемость	и	производительность	без	изменения	его	поведения.	
b. Опишите	дальнейшие	изменения и	их	причины,	которые	вы	бы	сделали,	если	бы	вы	могли	изменить	его	
поведение.	
*/

void UpdateRacers(float deltaTimeS,	List<Racer>	racers)
{
	List<Racer> racersNeedingRemoved = new List<Racer>();
	racersNeedingRemoved.Clear();
	//	Updates	the	racers	that	are	alive
	int racerIndex	= 0;
	for (racerIndex	= 1; racerIndex	<= 1000; racerIndex++)
	{
		if (racerIndex <= racers.Count)
		{
			if (racers[racerIndex - 1].IsAlive())
			{
				//Racer	update	takes	milliseconds
				racers[racerIndex - 1].Update(deltaTimeS * 1000.0f);
			}
		}
	}
	//	Collides
	for (int racerIndex1 = 0; racerIndex1 < racers.Count; racerIndex1++)
	{
		for (int racerIndex2 = 0; racerIndex2 < racers.Count; racerIndex2++)
		{
			Racer racer1 = racers[racerIndex1];
			Racer racer2 = racers[racerIndex2];
			if (racerIndex1	!=	racerIndex2)
			{
				if (racer1.IsCollidable()	&&	racer2.IsCollidable()	&&	racer1.CollidesWith(racer2))
				{
					OnRacerExplodes(racer1);
                    racersNeedingRemoved.Add(racer1);
                    racersNeedingRemoved.Add(racer2);
				}
			}
		}
	}
	//	Gets	the	racers	that	are	still	alive
	List<Racer>	newRacerList = new List<Racer>();
	for (racerIndex	= 0; racerIndex != racers.Count; racerIndex++)
	{
        //	check	if	this	racer	must	be	removed
        if (racersNeedingRemoved.IndexOf(racers[racerIndex]) < 0)
		{
	    	newRacerList.Add(racers[racerIndex]);
		}
	}
	//	Get	rid	of	all	the	exploded	racers
	for (racerIndex = 0; racerIndex != racersNeedingRemoved.Count; racerIndex++)
	{
		int foundRacerIndex = racers.IndexOf(racersNeedingRemoved[racerIndex]);
		if (foundRacerIndex >= 0)	//	Check	we've	not	removed	this	already!
		{
			racersNeedingRemoved[racerIndex].Destroy();
			racers.Remove(racersNeedingRemoved[racerIndex]);
		}
	}
	//	Builds	the	list	of	remaining	racers
	racers.Clear();
	for (racerIndex	= 0; racerIndex < newRacerList.Count; racerIndex++)
	{
		racers.Add(newRacerList[racerIndex]);
	}
	for (racerIndex = 0; racerIndex < newRacerList.Count; racerIndex++)
	{
		newRacerList.RemoveAt(0);
	}
}