using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factorial
{
    public delegate void FactorialIsReady(Factorial f);
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
            selector.FactorialIsInvalid += lessZero.FactorialInvalidHandler;

            f.Start();
        }
    }
    public class Factorial
    {
        public delegate void FactorialStartdHandler(Factorial f);
        public event FactorialStartdHandler FactorialInitialized;
        public int Number;
        public int? Result;
        public int Current;
        public Factorial(int n)
        {
            Number = n;
            Current = Number;
            Result = null;
        }
        public void Start()
        {
            FactorialInitialized(this);
        }
    }
    public class Selector
    {
        public delegate void FactorialSelected(Factorial f);
        
        public event FactorialSelected FactorialIsInvalid;
        public event FactorialSelected FactorialIsZero;
        public event FactorialSelected FactorialIsSimple;
        public event FactorialSelected FactorialNeedCalculating;
        
        public void Select(Factorial f)
        {
            if (f.Number < 0)
            {
                FactorialIsInvalid(f);
            }
            else if (f.Number == 0)
            {
                FactorialIsZero(f);
            }
            else if (f.Number > 0 && f.Number <= 2)
            {
                FactorialIsSimple(f);
            }
            else 
            {
                FactorialNeedCalculating(f);
            }
        }
    }
    public class Invalid
    {
        public void FactorialIsInvalid(Factorial f)
        {
            Console.WriteLine("Invalid value for factorial\n");
        }
    }
    public class IsZero
    {
        public event FactorialIsReady IsReady;
        public void FactorialIsZero(Factorial f)
        {
            f.Result = 1;
            IsReady(f);
        }
    }
    public class IsSimple
    {
        public event FactorialIsReady IsReady;
        public void FactorialIsSimple(Factorial f)
        {
            f.Result = f.Number;
            IsReady(f);
        }
    }
    public class NeedCalculating
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
    public class FactorialIteration
    {
        public int Steps;
        public FactorialIteration(Factorial f)
        {
            Steps = f.Number - 2;
        }
    }
}
