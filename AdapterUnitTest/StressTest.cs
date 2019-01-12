using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using NUnit.Framework;
using AdapterLibrary;
using Environment = System.Environment;

namespace AdapterUnitTest
{
    [TestFixture]
    public class StressComponentBuild
    {
        private KompasConnector _kompasConnector;
        private AdapterParameters _parametrs;
        private AdapterBuilder _builder;
        private readonly StreamWriter _writerCPU;
        private readonly StreamWriter _writerRAM;
        private PerformanceCounter _ramCounter;
        private PerformanceCounter _cpuCounter;

        public StressComponentBuild()
        {
            _writerCPU = new StreamWriter(@"D:\StressTestCPU.txt");
            _writerRAM = new StreamWriter(@"D:\StressTestRAM.txt");
        }

        [Test]
        public void Start()
        {
            // Запуск приложения 
            RunApplication();

            int count = 150;
            int n = 0;
            while (n < count)
            {
                // Поиск процесса САПР 
                var processes = Process.GetProcessesByName("KOMPAS");
                var process = processes.First();

                // При первой итерации проинициализировать объекты, отвечающие за фиксирование нагрузки 
                if (n == 0)
                {
                    _ramCounter = new PerformanceCounter("Process", "Working Set", process.ProcessName);
                    _cpuCounter = new PerformanceCounter("Process", "% Processor Time", process.ProcessName);
                }

                _cpuCounter.NextValue();

                // Построение детали 
                _builder.AdapterBuild(_parametrs);

                // Взятие значений занимаемоей оперативной памяти и загрузки ЦП 
                var ram = _ramCounter.NextValue();
                var cpu = _cpuCounter.NextValue();

                // Запись данных в файл 
                _writerRAM.Write($"{Math.Round(ram / 1024 / 1024)}");
                _writerCPU.Write($"{cpu / 8}");
                _writerRAM.Write(Environment.NewLine);
                _writerCPU.Write(Environment.NewLine);
                _writerCPU.Flush();
                _writerRAM.Flush();
                n += 1;
            }
        }

        private void RunApplication()
        {
            _parametrs = new AdapterParameters(50, 40, 5, 100, (float)1.5, 5);
            _kompasConnector = new KompasConnector();
            _builder = new AdapterBuilder(_kompasConnector);

            _kompasConnector.ConnectKompas();
        }

    }
}