
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace philosopher
{
    class Program
    {
        static Random ran = new Random();//创建random随机数对象
        public static int MAX_PHILOSOPHERS_NUM = 5;//全局变量哲学家数

        static List<Semaphore> forks = new List<Semaphore>();//筷子信号量list

        static List<Thread> philosphers = new List<Thread>();//哲学家进程list


        public static void getleftfork(int number)//获取左筷子
        {
            forks[number].WaitOne();
            Console.WriteLine(number + "philosopher:i get the lift fork");
            Thread.Sleep(TimeSpan.FromSeconds(ran.Next(1, 5)));
        }
        public static void getrightfork(int number)//获取右筷子
        {
            forks[(number+1) % MAX_PHILOSOPHERS_NUM].WaitOne();
            Console.WriteLine(number + "philosopher:i get the right fork");
            Thread.Sleep(TimeSpan.FromSeconds(ran.Next(1, 5)));
        }
        public static void forkrelease(int number)//释放筷子
        {
            forks[(number+1) % MAX_PHILOSOPHERS_NUM].Release();
            forks[number].Release();
        }

       

        
        private static void thinking(int number)//思考
        {
            Console.WriteLine(number + "philosopher:im thinking:" );
            Thread.Sleep(TimeSpan.FromSeconds(ran.Next(1, 5)));
        }

        private static void eating(int number)//吃
        {
            Console.WriteLine(number+"philosopher:im eating:"  );
            Thread.Sleep(TimeSpan.FromSeconds(ran.Next(2, 3)));
        }
        public static void Main(string[] args)
        {
            for (int i = 0; i <= MAX_PHILOSOPHERS_NUM; i++)
            {
                forks.Add(new Semaphore(1, 1));//将信号量加入list
                philosphers.Add(new Thread(philosopher2));//将哲学家进程加入list
            }
            for (int j = 1; j <= MAX_PHILOSOPHERS_NUM; j++)
            {
                philosphers[j].Start(j.ToString() as object);//启动进程
            }
        }
        public static Semaphore pholi = new Semaphore(MAX_PHILOSOPHERS_NUM - 1, MAX_PHILOSOPHERS_NUM - 1);//筷子信号量

        public static void philosopher2(object num)
        {
            int number = int.Parse(num.ToString());//将object转化为int
            while (true)
            {
                pholi.WaitOne();
                getleftfork(number);
                getrightfork(number);
                eating(number);
                forkrelease(number);
                pholi.Release();
                thinking(number);
            }
        }

    }
}
