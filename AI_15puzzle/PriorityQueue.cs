using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_15puzzle {
    public class PriorityQueue<T> where T : IComparable, IEquatable<T> {
        private readonly List<T> queue = new List<T>();

        public void Enqueue(T item) {
            queue.Add(item);
            queue.Sort();
        }

        public void Enqueue(IEnumerable<T> items) {
            queue.AddRange(items);
            queue.Sort();
        }

        public T Dequeue() {
            T item = queue[0];
            queue.RemoveAt(0);
            return item;
        }

        public bool Contains(T element) {
            return queue.Contains(element);
        }

        public bool isEmpty() {
            return queue.Count == 0;
        }
    }
}
