using TaskEntity = ModernisationChallenge.DataAccess.Task;

namespace ModernisationChallenge.Website.Models.Vm;

public class TaskVm
{
    public int Id { get; set; }
    public string Details { get; set; }
    public bool Completed { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime DateModified { get; set; }
    public DateTime? DateDeleted { get; set; }

    public TaskVm()
    {
    }

    public TaskVm(TaskEntity task)
    {
        Id = task.Id;
        Details = task.Details;
        DateCreated = task.DateCreated;
        DateModified = task.DateModified;
        DateDeleted = task.DateDeleted;
        Completed = task.Completed;
    }
}