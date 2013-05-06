using System;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue<T> where T : IEquatable<T> {

	private List<Tuple<float,T>> heap;

	public PriorityQueue() {
		heap = new List<Tuple<float,T>>();
	}
	
	private bool checkHeap() {
		//Checks out
		bool checks = true;
		
		for(int i = 0; i < heap.Count && checks; i++) {
			if(i * 2 + 1 < heap.Count)
				if(heap[i].get1() > heap[i*2+1].get1())
					checks = false;
			if(i * 2 + 2 < heap.Count)
				if(heap[i].get1() > heap[i*2+2].get1())
					checks = false;
		}
		
		return checks;
	}

	public void Enqueue(T item, float priority) {
		bool finished = false;
		int checkPoint = 0;
		//If something's already on the heap, put it there instead of the end
		bool replaced = false;
		for(int i = 0; i < heap.Count && !replaced; i++) {
			if (heap[i].get2().Equals(item)) {
				heap[i] = new Tuple<float, T>(priority, item);
				checkPoint = i;
				replaced = true;
			}
		}
		if(!replaced)
		{
			heap.Add(new Tuple<float, T>(priority, item));
			checkPoint = heap.Count - 1;
		}

		//Now rearrange the heap for things to work
		while(! finished && checkPoint > 0) {
			//Is the parent a higher number than us?
			if(heap[checkPoint].get1() < heap[parent(checkPoint)].get1()) {
				//Switch em
				swap(checkPoint, parent(checkPoint));
				checkPoint = parent(checkPoint);
			} else {
				finished = true;
			}
		}

		if(!this.checkHeap()) {
		}
	}

	public bool IsEmpty() {
		return heap.Count == 0;
	}

	public void UpdatePriority(T item, float newPriority) {
		if(! IsEmpty() ) {
			//Find the item
			bool found = false;
			int i = 0;
			for(i = 0; i < heap.Count && !found; i++) {
				if(heap[i].get2().Equals(item)) {
					//We found it!
					found = true;
				}
			}

			if(!found) //This wasn't in the heap to begin with
				return;

			//Otherwise update the priority and rebuild the heap appropriately
			i--;
			heap[i] = new Tuple<float, T>(newPriority, item);

			bool finished = false;
			int checkPoint = i;
			while(!finished) {
				finished = true;
				//Are we greater than our children, if we have them?
				if(heap.Count > child(checkPoint)) {
					int smallest;
					if(heap.Count > child(checkPoint + 1)) {
						smallest = heap[child(checkPoint)].get1() < heap[child(checkPoint)+1].get1() ?
							child(checkPoint) : child(checkPoint) + 1;
					} else {
						smallest = child(checkPoint);
					}

					//Are we biger than the smallest child?
					if(heap[checkPoint].get1() > heap[smallest].get1()) {
						//Do the switch
						swap(checkPoint, smallest);
						checkPoint = smallest;
						finished = false;
					}
				}

				//Also check if we're smaller than our parent
				if(checkPoint != 0 && heap[parent(checkPoint)].get1() > heap[checkPoint].get1()) {
					//Do the swap
					swap(checkPoint, parent(checkPoint));
					checkPoint = parent(checkPoint);
					finished = false;
				}
			}
		}
	}

	public T Dequeue() {
		if(! IsEmpty() ) {
			T returnee = heap[0].get2();

			bool finished = false;
			int checkPoint = 0;

			//Do the initial replacement
			heap[0] = heap[heap.Count - 1];
			heap.RemoveAt(heap.Count - 1);

			//Rebuild the heap
			while(!finished) {
				//Do we have children?
				if(heap.Count > child(checkPoint)) {
					int smallest;
					if(heap.Count > child(checkPoint) + 1) {
						smallest = heap[child(checkPoint)].get1() < heap[child(checkPoint)+1].get1() ?
							child(checkPoint) : child(checkPoint) + 1;
					} else {
						smallest = child(checkPoint);
					}

					//Are we bigger than the smallest child?
					if (heap[checkPoint].get1() > heap[smallest].get1()) {
						//Do the switch
						swap(checkPoint, smallest);
						checkPoint = smallest;
					} else {
						finished = true;
					}

				} else {
					finished = true;
				}
			}

			return returnee;
		} else {
			return default(T);
		}
	}

	private void swap(int a, int b) {
		Tuple<float, T> swap = heap[a];
		heap[a] = heap[b];
		heap[b] = swap;
	}

	public T ContainsElement(T element) {
		foreach(Tuple<float,T> t in heap) {
			if(t.get2().Equals(element)) return t.get2();
		}

		return default(T);
	}

	public Tuple<float, T> Peek() {
		return heap[0];
	}

	private int child(int spot) {
		return 2*spot + 1;
	}

	private int parent(int spot) {
		return ((spot - 1) / 2);
	}

	public int Count() {
		return heap.Count;
	}
}

public class Tuple<T1, T2> : IEquatable<Tuple<T1, T2>> {
	private T1 t1;
	private T2 t2;

	public Tuple(T1 t1, T2 t2) {
		this.t1 = t1;
		this.t2 = t2;
	}

	public T1 get1() { return t1; }
	public T2 get2() { return t2; }

	public bool Equals(Tuple<T1, T2> other) {
		return other.t1.Equals(this.t1) && other.t2.Equals(this.t2);
	}

	new public int GetHashCode() {
		return t1.GetHashCode() + t2.GetHashCode();
	}
}
