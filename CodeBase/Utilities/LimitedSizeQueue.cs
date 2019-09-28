/**
 * Copyright (C) 2019 Dental Stars SRL
 * Copyright (C) 2003-2019 Jordan S. Sparks, D.M.D.
 * 
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program; If not, see <http://www.gnu.org/licenses/>
 */
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace CodeBase
{
    /// <summary>
    /// Creates a thread-safe queue with a size limit. The queue will automatically dequeue any 
    /// items in excess of the size limit when a new item is enqueued.
    /// </summary>
    public class LimitedSizeQueue<T> : IEnumerable<T>
    {
        /// <summary>
        /// The actual queue of items. This queue will only store items up to the value that 
        /// <see cref="Limit"/> was set to. E.g. this queue will automatically dequeue items when
        /// an item attempts to enqueue that would push the size of the queue past Limit.
        /// </summary>
        protected readonly ConcurrentQueue<T> queue = new ConcurrentQueue<T>();

        /// <summary>
        /// The size limitation that will be enforced on the concurrent queue every time an item 
        /// attempts to enqueue.
        /// </summary>
        public int Limit { get; }

        /// <summary>
        /// Creates a concurrent queue that will limit its size to the limit passed in.
        /// </summary>
        public LimitedSizeQueue(int limit) => Limit = limit;
        
        /// <summary>
        /// Adds an item to the queue and automatically dequeues any items in excess of the 
        /// specified size limit. Returns true if successful.
        /// </summary>
        public bool Enqueue(T item)
        {
            if (queue.Contains(item)) return false;
            
            while (queue.Count >= Limit)
            {
                if (!queue.TryDequeue(out _))
                {
                    return false;
                }
            }

            queue.Enqueue(item);
            return true;
        }

        /// <summary>
        /// Adds an item to the queue and automatically dequeues any items in excess of the 
        /// specified size limit. Returns true if successful. If an item is dequeued, returns the 
        /// object as an out parameter.
        /// </summary>
        public bool Enqueue(T item, out T dequeued)
        {
            dequeued = default;
            if (queue.Count >= Limit && !queue.TryDequeue(out dequeued))
            {
                return false;
            }

            queue.Enqueue(item);
            return true;
        }

        /// <summary>
        /// Tries to dequeue an item. Returns true if successful.
        /// </summary>
        public bool Dequeue() => queue.TryDequeue(out _);
        
        /// <summary>
        /// Tries to dequeue an item. Returns true if successful. Returns the dequeued object as an 
        /// out parameter.
        /// </summary>
        public bool Dequeue(out T dequeued) => queue.TryDequeue(out dequeued);

        public bool Contains(T item) => queue.Contains(item);
        
        public List<T> ToList() => queue.ToList();

        public IEnumerator<T> GetEnumerator() => queue.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
