using System.Collections;
using System.Threading.Tasks;

namespace Rossoforge.Pool.Tests
{
    public static class TaskExtentions
    {
        public static IEnumerator AsCoroutine(this Task task)
        {
            while (!task.IsCompleted)
            {
                yield return null;
            }
            task.GetAwaiter().GetResult();
        }
    }
}