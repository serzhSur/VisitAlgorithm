using VisitAlgorithm;

List<Appointment> schedule = CreateBlankTimeTeble();
ShowTimeTable(schedule);

var newApp1 = new Appointment
{
    Start = new TimeSpan(10, 15, 0),
    Stop = new TimeSpan(11, 20, 0),
    Status = "стрижка"
};
AddAppointmentToSchedule(CheckIntervalAvailability(schedule, newApp1), schedule, newApp1);
RemoveSmalIntervals(schedule, new TimeSpan(0,15,0));
ShowTimeTable(schedule);

var app2 = new Appointment
{
    Start = new TimeSpan(11, 30, 0),
    Stop = new TimeSpan(12, 30, 0),
    Status = "окрашивание"
};
AddAppointmentToSchedule(CheckIntervalAvailability(schedule, app2), schedule, app2);
RemoveSmalIntervals(schedule, new TimeSpan(0, 15, 0));
ShowTimeTable(schedule);


static List<Appointment> CheckIntervalAvailability(List<Appointment> schedule, Appointment app)
{
    // Проверка на пустой список
    if (schedule == null || !schedule.Any())
    {
        Console.WriteLine("No items in List<Appointment> schedule");
        return new List<Appointment>();
    }

    // Проверка, находится ли новый интервал в пределах существующих
    var minStartValue = schedule.Min(a => a.Start);
    var maxStopValue = schedule.Max(a => a.Stop);
    if (app.Start < minStartValue || app.Stop > maxStopValue)
    {
        Console.WriteLine("Time out of range");
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
        Console.WriteLine("Some time-intervals are busy");
        return new List<Appointment>();
    }

}

static void AddAppointmentToSchedule(List<Appointment> crossingIntervals, List<Appointment> schedule, Appointment newApp1)
{
    if (crossingIntervals != null && crossingIntervals.Any())
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
        schedule.Sort((a, b) => a.Start.CompareTo(b.Start));// сортируем расписание
    }
    else
    {
        Console.WriteLine("Schedule not available");
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

static void RemoveSmalIntervals(List<Appointment> schedule, TimeSpan minIntervalSise)
{
    schedule.RemoveAll(appointment=>(appointment.Stop - appointment.Start)<minIntervalSise);
}