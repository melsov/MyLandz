using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// Run a queue of coroutines in series
/// </summary>
namespace MyLandz.WorkQueue
{
    public class WorkQueue : MonoBehaviour
    {
        private bool busy;
        private IncrementalProcess current;

        public int purgeJobsThreshhold = 9999999;

        private Queue<IncrementalProcess> _jobs;
        private Queue<IncrementalProcess> jobs {
            get {
                if(_jobs == null) {
                    _jobs = new Queue<IncrementalProcess>();
                }
                return _jobs;
            }
        }

        public void add(Action increment, Func<int, bool> isDone, float tick, Action clientCallback) {
            IncrementalProcess inc = new IncrementalProcess();
            inc.increment = increment;
            inc.isDone = isDone;
            inc.tick = tick;
            jobs.Enqueue(inc);
            while(jobs.Count > purgeJobsThreshhold) {
                jobs.Dequeue();
            }
            runQueue(clientCallback);
        }

        private void runQueue(Action clientCallback) {
            if(busy) { return; }
            if (jobs.Count > 0) {
                busy = true;
                current = jobs.Dequeue(); //Dequeueing inside of StartCoroutine crashes Unity??
                StartCoroutine(current.run(() => {
                    busy = false;
                    clientCallback.Invoke();
                    runQueue(clientCallback);
                }));
            }
        }
    }

    public struct IncrementalProcess
    {
        public Action increment;
        public float tick;
        public Func<int, bool> isDone;
        private int count;

        public IEnumerator run(Action callback) {
            while (!isDone.Invoke(count++)) {
                if(count > 90000) { Debug.Log("hit test safety"); break; }
                increment.Invoke();
                yield return new WaitForSeconds(tick);
            }
            callback.Invoke();
        }
    }

}
