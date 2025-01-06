using VisitAlgorithm;

var start = new TimeSpan(10, 0, 0);
var end = new TimeSpan(15, 0, 0);
var interval = new TimeSpan(1, 0, 0);
var schedule = new List<Appointment>();

for (var i = start; i < end; i += interval)
{
    schedule.Add(new Appointment { Start = i, Stop = i + interval, Status = "free" });
}

ShowTimeTable(schedule);

var newApp1 = new Appointment
{
    Start = new TimeSpan(11, 0, 0),
    Stop = new TimeSpan(12, 15, 0),
    Status = "стрижка 15мин"
};

AddAppointment(schedule, newApp1);

//ShowTimeTable(schedule);

//var app2 = new Appointment
//{
//    Start = new TimeSpan(11, 30, 0),
//    Stop = new TimeSpan(12, 00, 0),
//    Status = "окрашивание 1ч 30мин"
//};

//AddAppointment(schedule, app2);

//ShowTimeTable(schedule);

static void AddAppointment(List<Appointment> schedule, Appointment newApp1)
{
    //var crossingIntervals = schedule.FirstOrDefault(s => s.Start < newApp1.Stop && newApp1.Start < s.Stop);
    var crossingIntervals = schedule.Where(s => s.Start < newApp1.Stop && newApp1.Start < s.Stop).ToList();

    if (crossingIntervals != null)// && crossingIntervals.Status == "free")
    {
        schedule.Add(newApp1);// добавление нового интервала

        //if (crossingIntervals.Stop<newApp1.Start)

        //crossingIntervals.Start = newApp1.Stop;//редактироввание старого интервала
        //schedule.Sort((a, b) => a.Start.CompareTo(b.Start));
    }
    else
    {
        Console.WriteLine("Интервал недоступен");
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