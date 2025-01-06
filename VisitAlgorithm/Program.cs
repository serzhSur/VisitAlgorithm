using VisitAlgorithm;

List<Appointment> schedule = CreateBlankTimeTeble();

ShowTimeTable(schedule);

var newApp1 = new Appointment
{
    Start = new TimeSpan(10, 15, 0),
    Stop = new TimeSpan(11, 20, 0),
    Status = "стрижка"
};

AddAppointment(IntervalAvailabilityCheck(schedule, newApp1),schedule, newApp1);

ShowTimeTable(schedule);

var app2 = new Appointment
{
    Start = new TimeSpan(11, 30, 0),
    Stop = new TimeSpan(12, 30, 0),
    Status = "окрашивание"
};

AddAppointment(IntervalAvailabilityCheck(schedule, app2), schedule, app2);

ShowTimeTable(schedule);
static List<Appointment> IntervalAvailabilityCheck (List<Appointment> schedule, Appointment app)
{
    // Проверка на пустой список
    if (schedule == null || !schedule.Any())
    {
        Console.WriteLine("Расписание пустое.");
        return new List<Appointment>(); 
    }

    // Проверка, находится ли новый интервал в пределах существующих
    var minStartValue = schedule.Min(a => a.Start);
    var maxStopValue = schedule.Max(a => a.Stop);
    if (app.Start < minStartValue || app.Stop > maxStopValue)
    {
        Console.WriteLine("Интервал недоступен");
        return new List<Appointment>(); 
    }

    // Находим пересекающиеся интервалы
    var crossingIntervals = schedule.Where(s => s.Start < app.Stop && app.Start < s.Stop).ToList();

    // Проверка, свободны ли все пересекающиеся интервалы
    if (crossingIntervals.All(a => a.Status == "free"))
    {
        return crossingIntervals; // Возвращаем пересекающиеся интервалы
    }
    else
    {
        Console.WriteLine("Некоторые интервалы заняты");
        return new List<Appointment>(); 
    }

}

static void AddAppointment(List<Appointment> crossingIntervals, List<Appointment> schedule, Appointment newApp1)
{
    if (crossingIntervals != null&& crossingIntervals.Any())
    {
        schedule.Add(newApp1);// добавление нового интервала

        //редактироввание старого интервала
        foreach (var appointment in crossingIntervals)
        {
            if (appointment.Start < newApp1.Start && appointment.Stop > newApp1.Stop)
            {
                var newInterval = new Appointment
                {
                    Start = newApp1.Stop,
                    Stop = appointment.Stop,
                    Status = "free"
                };
                appointment.Stop = newApp1.Start;
                schedule.Add(newInterval);
            }
            else if (appointment.Start < newApp1.Start && appointment.Stop < newApp1.Stop)
            {
                appointment.Stop = newApp1.Start;

            }
            else if (appointment.Stop > newApp1.Stop)
            {
                appointment.Start = newApp1.Stop;
            }
            else if (appointment.Start > newApp1.Start && appointment.Stop < newApp1.Stop)
            {
                schedule.Remove(appointment);
            }
        }

        // удалить интервалы меньше минимальной длинны (меньше самого короткого сервиса)
        schedule.Sort((a, b) => a.Start.CompareTo(b.Start));// сортируем расписание
    }
    else
    {
        Console.WriteLine("запись недоступна");
    }
}

static void ShowTimeTable(List<Appointment> schedule)
{
    Console.WriteLine("Расписание:");
    foreach (var v in schedule)
    {
        Console.WriteLine($"{v.Start,8} -- {v.Stop,8} -- {v.Status,8}");
    }
}

static List<Appointment> CreateBlankTimeTeble()
{
    var start = new TimeSpan(10, 00, 0);
    var end = new TimeSpan(15, 00, 0);
    var interval = new TimeSpan(1, 0, 0);
    var schedule = new List<Appointment>();

    for (var i = start; i < end; i += interval)
    {
        schedule.Add(new Appointment { Start = i, Stop = i + interval, Status = "free" });
    }

    return schedule;
}