using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_15puzzle {
    public class PriorityQueue<T> where T : IComparable {
        private readonly List<T> queue = new List<T>();

        /*public void Enqueue(T item) {
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
        }*/

        public void Enqueue(T item) {
            queue.Add(item);
            BubbleUp();
        }

        public T Dequeue() {
            var item = queue[0];
            MoveLastItemToTheTop();
            SinkDown();
            return item;
        }
        // Implementation of the Min Heap Bubble Up operation
        private void BubbleUp() {
            var childIndex = queue.Count - 1;
            while (childIndex > 0) {
                var parentIndex = (childIndex - 1) / 2;
                if (queue[childIndex].CompareTo(queue[parentIndex]) >= 0)
                    break;
                Swap(childIndex, parentIndex);
                childIndex = parentIndex;
            }
        }

        private void MoveLastItemToTheTop() {
            var lastIndex = queue.Count - 1;
            queue[0] = queue[lastIndex];
            queue.RemoveAt(lastIndex);
        }
        // Implementation of the Min Heap Sink Down operation
        private void SinkDown() {
            var lastIndex = queue.Count - 1;
            var parentIndex = 0;

            while (true) {
                var firstChildIndex = parentIndex * 2 + 1;
                if (firstChildIndex > lastIndex) {
                    break;
                }
                var secondChildIndex = firstChildIndex + 1;
                if (secondChildIndex <= lastIndex && queue[secondChildIndex].CompareTo(queue[firstChildIndex]) < 0) {
                    firstChildIndex = secondChildIndex;
                }
                if (queue[parentIndex].CompareTo(queue[firstChildIndex]) < 0) {
                    break;
                }
                Swap(parentIndex, firstChildIndex);
                parentIndex = firstChildIndex;
            }
        }

        private void Swap(int index1, int index2) {
            var tmp = queue[index1];
            queue[index1] = queue[index2];
            queue[index2] = tmp;
        }

        public bool Contains(T element) {
            return queue.Contains(element);
        }

        public bool isEmpty() {
            return queue.Count == 0;
        }
    }
}
