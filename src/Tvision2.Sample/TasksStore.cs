using Tvision2.Core.Stores;

namespace Tvision2.Sample
{
    public class TasksStore : TvStore<TasksList>
    {
        public TasksStore() : base(new TasksList())
        {
        }
    }
}