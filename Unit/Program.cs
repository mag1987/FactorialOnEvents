using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unit
{
    class Program
    {
        static void Main(string[] args)
        {
        }
    }
    public interface IA
    {
        void Method1();
        int Method2();
    }
    public class A : IA
    {
        public void Method1() { }
        public virtual int Method2()
        {
            return default(int);
        }
    }
    public class B : A
    {
        public override int Method2()
        {
            return 1;
        }
    }
}
