/*
Рассмотрим	гипотетическую	гоночную	игру,	в	которой	сотни	автомобилей	мчатся	по	полю.	Метод	updateRacers,	
показанный	ниже,	обновляет	автомобили	и	уничтожает	те,	которые	сталкиваются.

a. Перепишите	метод,	чтобы	улучшить	его	читаемость	и	производительность	без	изменения	его	поведения.	
b. Опишите	дальнейшие	изменения и	их	причины,	которые	вы	бы	сделали,	если	бы	вы	могли	изменить	его	
поведение.	
*/

void UpdateEveryRacer(float deltaTimeS,	List<Racer>	racers) 
{
	//	Апдейтим всех гонщиков (проверка IsAlive может быть необязательна, 
	//  если условием смерти является только коллизия с другим гонщиком)
	int racerIndex	= 0;
	for (racerIndex	= 1; racerIndex	<= racers.Count; racerIndex++)
	{
		if (racers[racerIndex - 1].IsAlive())
		{
			//  Переводим дельту из милисекунд в секунды
			racers[racerIndex - 1].Update(deltaTimeS * 1000.0f);
		}
	}
}

void CheckCollides(List<Racer>	racers, List<Racer> racersNeedingRemoved)
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
                racersNeedingRemoved.Add(racer1);
                racersNeedingRemoved.Add(racer2);
			}
		}
	}
}

void RemoveCollidesRacers(List<Racer>	racers, List<Racer> racersNeedingRemoved)
{
	//	Перебираем всех оставшихся в живых гонщиков
	List<Racer>	newRacerList = new List<Racer>();
	for (racerIndex	= 0; racerIndex != racers.Count; racerIndex++)
	{
        //	проверяем нужно ли убрать гонщика
        if (racersNeedingRemoved.IndexOf(racers[racerIndex]) < 0)
		{
	    	newRacerList.Add(racers[racerIndex]);
		}
	}

	//	Убираем взорвавшихся гонщиков
	for (racerIndex = 0; racerIndex != racersNeedingRemoved.Count; racerIndex++)
	{
		int foundRacerIndex = racers.IndexOf(racersNeedingRemoved[racerIndex]);
		if (foundRacerIndex >= 0)	//	Check	we've	not	removed	this	already!
		{
			racers.Remove(racersNeedingRemoved[racerIndex]);	
		}
	}

	//	Создаём новый список с выжившими
	racers.Clear();
	racersNeedingRemoved.Clear();
	for (racerIndex	= 0; racerIndex < newRacerList.Count; racerIndex++)
	{
		racers.Add(newRacerList[racerIndex]);
	}

	//  ВАЖНАЯ ПОМЕТКА
	//  Весь код данной функции можно заменить на следующий для оптимизации
	//  работы программы, однако это изменит поведение программы
	/*

	for (racerIndex	= 0; racerIndex != racers.Count; racerIndex++)
	{
        //	проверяем нужно ли убрать гонщика
        if (racersNeedingRemoved.IndexOf(racers[racerIndex]) > 0)
		{
	    	racers.Remove(racers[racerIndex]);
		}
	}
	racersNeedingRemoved.Clear();

	*/
}

void UpdateRacers(float deltaTimeS,	List<Racer>	racers)
{
	List<Racer> racersNeedingRemoved = new List<Racer>();
	racersNeedingRemoved.Clear();

	UpdateEveryRacer(deltaTimeS,	racers);
	CheckCollides(racers, 	racersNeedingRemoved)
	RemoveCollidesRacers(racers, 	racersNeedingRemoved)
}

//  Так же можно объединить функции CheckCollides(racersNeedingRemoved) и RemoveCollidesRacers(racers, 	racersNeedingRemoved)
//  Это оптимизирует программу
//  Пример объединения данных функций:
/*

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

*/

//  Для удобства код с полной оптимизацией будет сохранён в файл Racers v2.cs