using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factorial
{
    class Program
    {
        static void Main(string[] args)
        {
            Factorial f = new Factorial(3);
            Selector selector = new Selector();
            LessZero lessZero = new LessZero();
            IsReady isReady = new IsReady();
            InProgress inProgress = new InProgress();
            inProgress.FactorialStateChanged += selector.Select;
            f.FactorialInitialized += selector.Select;
            selector.FactorialInProgress += inProgress.FactorialInProgressHandler;
            selector.FactorialIsReady += isReady.FactorialIsReadyHandler;
            selector.FactorialInvalid += lessZero.FactorialInvalidHandler;

            f.Initialized();
        }
    }
    public class Factorial
    {
        public delegate void FactorialInitializedHandler(Factorial f);
        public event FactorialInitializedHandler FactorialInitialized;
        public int Number;
        public int? Result;
        public int Current;
        public Factorial(int n)
        {
            Number = n;
            Current = Number;
            Result = null;
        }
        public void Initialized()
        {
            FactorialInitialized(this);
        }
    }
    public class Selector
    {
        public delegate void FactorialSelected(Factorial f);
        
        public event FactorialSelected FactorialInvalid;
        public event FactorialSelected FactorialIsReady;
        public event FactorialSelected FactorialInProgress;
        public void Select(Factorial f)
        {
            if (f.Number < 0)
            {
                FactorialInvalid(f);
            }
            else
            {
                if (f.Current == 0)
                {
                    FactorialIsReady(f);
                }
                else
                {
                    FactorialInProgress(f);
                }
            }
        }
    }
    public class LessZero
    {
        public void FactorialInvalidHandler(Factorial f)
        {
            Console.WriteLine("Invalid value for factorial\n");
        }
    }
    public class IsReady
    {
        public void FactorialIsReadyHandler(Factorial f)
        {
            Console.WriteLine("Factorial({0}) = {1}\n",f.Number, f.Result == null ? 1 : f.Result );
        }
    }
    public class InProgress
    {
        public delegate void FactorialState(Factorial f);
        public event FactorialState FactorialStateChanged;
        public void FactorialInProgressHandler(Factorial f)
        {
            if (f.Result == null)
            {
                f.Result = f.Current;
            }
            else
            {
                f.Result *= f.Current;
            }
            f.Current--;
            Console.WriteLine("In progress");
            FactorialStateChanged(f);
        }
    }
}
