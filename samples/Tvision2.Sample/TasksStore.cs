

using Tvision2.Statex;

namespace Tvision2.Sample
{
    public class TasksStore : TvStore<TasksList>
    {
        public TasksStore() : base(new TasksList())
        {
        }
    }
}