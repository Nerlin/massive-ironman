using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Timers;
using System.Management;

namespace Core
{
    #region ProcessDetected Event Declaration

    /// <summary>
    /// Тип обнаружения подозрительного процесса.
    /// </summary>
    public enum DetectionType
    {
        /// <summary>
        /// Неверное значение.
        /// </summary>
        Invalid,
        /// <summary>
        /// Предупреждение.
        /// </summary>
        Warning,
        /// <summary>
        /// Критическая остановка работы процесса.
        /// </summary>
        Critical
    }

    /// <summary>
    /// Тип реакции пользователя на обнаружение подозрительного процесса.
    /// </summary>
    public enum UserResponse
    {
        /// <summary>
        /// Неверное значение.
        /// </summary>
        Invalid,
        /// <summary>
        /// Подтверждение еще не получено.
        /// </summary>
        NoResponse,
        /// <summary>
        /// Пропуск обработки найденного процесса.
        /// </summary>
        Skip,
        /// <summary>
        /// Подтверждение обнаружения процесса.
        /// </summary>
        OK,
        /// <summary>
        /// Подтверждение необходимости немедленного завершения процесса.
        /// </summary>
        Kill
    }

    /// <summary>
    /// Класс определяет аргументы события нахождения подозрительного процесса.
    /// </summary>
    public class ProcessDetectedEventArgs: EventArgs
    {
        /// <summary>
        /// Обнаруженный подозрительный процесс.
        /// </summary>
        public String DetectedProcess
        {
            get;
            private set;
        }

        /// <summary>
        /// Количество порожденных процессов.
        /// </summary>
        public int ProcessesCount
        {
            get;
            private set;
        }

        /// <summary>
        /// Тип обнаружения подозрительного процесса.
        /// </summary>
        public DetectionType DetectionType
        {
            get;
            private set;
        }

        public ProcessDetectedEventArgs(String detectedProcess, int processesCount, 
            DetectionType detectionType): base()
        {
            this.DetectedProcess = detectedProcess;
            this.DetectionType = detectionType;
            this.ProcessesCount = processesCount;
        }
    }

    /// <summary>
    /// Делегат обработки события обнаружения подозрительного процесса.
    /// </summary>
    /// <param name="args">Аргументы события.</param>
    /// <returns>Возвращает значение, говорящее о том, нужно ли завершить работу процесса.</returns>
    public delegate UserResponse ProcessDetectedHandler(ProcessDetectedEventArgs args);

    #endregion
    /// <summary>
    /// Класс обеспечивает защиту от fork-приложений.
    /// </summary>
    class ForkProtection
    {
//#warning Const Exception
        const int MinimumWarningProcessesCount = 25;
        const int BaseCriticalProcessesCount = 50;
        const int SuspectProcessesCount = 5;
        const int MaximumProcessesCount = 100;
        /// <summary>
        /// Константа определяет критический скачок количества процессов за интервал времени.
        /// </summary>
        const int CriticalJumpProcessesCount = 25;

        /// <summary>
        /// Уровень защиты
        /// </summary>
        public ProtectionType ProtectionType
        {
            get;
            set;
        }

        private int warningProcessesCount;
        /// <summary>
        /// Получает или задает количество однотипных процессов, при котором будет выдаваться предупреждение,
        /// которое сообщает о большом количестве процессов.
        /// </summary>
        public int WarningProcessesCount
        {
            get { return warningProcessesCount; }
            set
            {
                if (value >= MinimumWarningProcessesCount)
                {
                    if (value <= MaximumProcessesCount)
                    {
                        this.warningProcessesCount = value;

                        if (value > this.CriticalProcessesCount)
                        {
                            this.CriticalProcessesCount = value;
                        }
                    }
                    else
                    {
                        this.WarningProcessesCount = MaximumProcessesCount;

                        throw new Exception("Значение количества однотипных процессов, при котором будет выдаваться предупреждение, " +
                            "не может быть больше " + MaximumProcessesCount + ". " +
                            "Это нужно для нормальной работы системы.");
                    }
                }
                else
                {
                    this.WarningProcessesCount = MinimumWarningProcessesCount;

                    throw new Exception("Значение количества однотипных процессов, при котором будет выдаваться предупреждение, " +
                        "не может быть меньше " + MinimumWarningProcessesCount + ". " +
                        "Это нужно для нормальной работы системы.");
                }
            }
        }


        int criticalProcessesCount;
        /// <summary>
        /// Получает или задает количество однотипных процессов,
        /// при котором будут выполнены критические меры, такие как блокировка и аварийное завершение процессов.
        /// </summary>
        public int CriticalProcessesCount
        {
            get { return criticalProcessesCount; }
            set
            {
                if (value >= this.WarningProcessesCount)
                {
                    if (value <= MaximumProcessesCount)
                    {
                        this.criticalProcessesCount = value;
                    }
                    else
                    {
                        this.CriticalProcessesCount = MaximumProcessesCount;

                        throw new Exception("Критическое количество однотипных процессов, при котором будут приняты критические меры, " +
                            "не может быть больше, чем " + MaximumProcessesCount + ".");
                    }
                }
                else
                {
                    this.CriticalProcessesCount = this.WarningProcessesCount;
                    throw new Exception("Значение количества однотипных процессов, при котором будут приняты критические меры, " +
                        "не может быть меньше, чем значение количества процессов, при котором будет выдано предупреждение.");
                }
            }
        }

        /// <summary>
        /// Событие обнаружения подозрительного процесса.
        /// </summary>
        public event ProcessDetectedHandler ProcessDetected;

        
        Timer DetectionTimer;

        /// <summary>
        /// Получает или задает состояние работы защиты от fork-атак.
        /// </summary>
        private bool Running
        {
            get { return this.DetectionTimer.Enabled; }
            set
            {
                this.DetectionTimer.Enabled = value;
            }
        }

        /// <summary>
        /// Определяет названия процессов-исключений, которые пользователь предпочел не блокировать.
        /// </summary>
        public List<String> ProcessesExclusion
        {
            get;
            set;
        }

        /// <summary>
        /// Определяет названия процессов, которые были заблокированы.
        /// </summary>
        public List<String> BannedProcesses
        {
            get;
            set;
        }

        private Dictionary<string, int> ProcessesCount;

        public ForkProtection(ProtectionType protectionType)
        {
            this.ProtectionType = protectionType;
            this.CriticalProcessesCount = BaseCriticalProcessesCount;
            this.WarningProcessesCount = MinimumWarningProcessesCount;

            this.ProcessesCount = new Dictionary<string, int>();
            this.ProcessesExclusion = new List<string>();
            this.ProcessesExclusion.Add("svchost");

            this.BannedProcesses = new List<string>();

            this.DetectionTimer = new Timer();
            this.DetectionTimer.Interval = 100;
            this.DetectionTimer.Enabled = false;
            this.DetectionTimer.Elapsed += new ElapsedEventHandler(Handler_DetectionTimerTick);
        }


        public void Start()
        {
            this.Running = true;
        }

        public void Stop()
        {
            this.Running = false;
        }

        private void Detection()
        {
//#warning Отладка.
            this.Stop();
            // Посмотреть текущие процессы
            Process[] systemProcesses = Process.GetProcesses();

            

            // Найти и выделить одноименные процессы
            Dictionary<String, List<Process>> sameProcesses = GetSameProcesses(systemProcesses);

            // Исключить из разбора процессы-исключения
            Dictionary<String, List<Process>> processesWithoutExclusion = ExcludeProcesses(sameProcesses);

            // Выбрать подозрительные процессы
            Dictionary<String, List<Process>> suspeciousProcesses = GetSuspiciousProcesses(processesWithoutExclusion);
            
            // С каждым процессом провести следующие действия:
            foreach (var processItem in suspeciousProcesses)
            {   
                // Получить имя процесса
                String processName = processItem.Key;
                // Получить список одноименных процессов
                List<Process> processes = processItem.Value;

                // Если процесс был заблокирован ранее 
                // Или
                // Если количество процессов зашло за критическую отметку,   
                if (ProcessIsBanned(processName) || ProcessesCountIsCritical(processes))
                {
                    // Немедленно завершить работу процесса
                    KillProcesses(processes);
                }
                else
                {
                    // Посмотреть сколько таких процессов в данной итерации
                    int count = processes.Count;
                    // Проверить не получен ли критический скачок количества процессов
                    // Если получен большой скачок количества процессов,
                    if (CriticalJumpObtained(processName, count))
                    {
                        // Немедленно завершить работу процесса
                        KillProcesses(processes);
                    }
                    // Проверить не перешло ли количество процессов отметку, когда необходимо предупредить
                    // пользователя об опасности.
                    else
                    {                        
                        // Проверка осуществляется только если тип защиты - нормальный,
                        if (this.ProtectionType == Core.ProtectionType.Simple)
                        {                           
                            // Если количество процессов зашло за предупредительную отметку,
                            if (WarningIsNeeded(count))
                            {
                                this.Stop();
                                // Спросить у пользователя нужно ли завершить процесс
                                UserResponse userResponse = RaiseProcessDetection(processName, count, DetectionType.Warning);
                                
                                // Если процесс нужно завершить,
                                if (userResponse == UserResponse.Kill)
                                {
                                    // Немедленно завершить работу процесса
                                    KillProcesses(processes);
                                }
                                // Иначе,
                                else if (userResponse == UserResponse.Skip)
                                {
                                    // Добавить процесс в исключения
                                    this.ProcessesExclusion.Add(processName);
                                }

                                this.Start();
                            }
                        }
                        
                    }

                    // Запомнить кол-во процессов на данной итерации
                    SaveProcessCount(processName, count);
                }
            }                               
        }

        /// <summary>
        /// Возвращает значение, говорящее о том, был ли заблокирован процесс ранее.
        /// </summary>
        /// <param name="processName">Название процесса.</param>
        /// <returns></returns>
        private bool ProcessIsBanned(String processName)
        {
            return this.BannedProcesses.Contains(processName);
        }

        /// <summary>
        /// Сохраняет количество процессов с заданным именем.
        /// </summary>
        /// <param name="processName">Название процесса.</param>
        /// <param name="count">Количество процессов.</param>
        private void SaveProcessCount(String processName, int count)
        {
            if (this.ProcessesCount.ContainsKey(processName))
            {
                this.ProcessesCount[processName] = count;
            }
            else
            {
                this.ProcessesCount.Add(processName, count);
            }
        }

        private bool WarningIsNeeded(int count)
        {
            return count >= this.WarningProcessesCount;
        }


        /// <summary>
        /// Немедленно завершает список процессов.
        /// </summary>
        /// <param name="processes">Список процессов, которые необходимо немедленно завершить.</param>
        private void KillProcesses(List<Process> processes)
        {
            String processName = processes.First().ProcessName;

            //BanProcess(processName);
            
            // Завершить все процессы из списка.
            foreach (var process in processes)
            {
                try
                {
                    process.Kill();
                }
                catch
                {
                    continue;
                }
            }

            // Сообщить об успешной ликвидации угрозы.
            this.RaiseProcessDetection(processName, processes.Count, DetectionType.Critical);
        }

        private void BanProcess(String processName)
        {
            if (!this.BannedProcesses.Contains(processName))
            {
                this.BannedProcesses.Add(processName);
            }
        }

        /// <summary>
        /// Возвращает отсортированные по именам процессы.
        /// </summary>
        /// <param name="processesDict">Список процессов.</param>
        /// <returns></returns>
        private Dictionary<string, List<Process>> GetSameProcesses(Process[] processesDict)
        {
            Dictionary<string, List<Process>> sameProcesses = new Dictionary<string, List<Process>>();

            foreach (var process in processesDict)
            {
                if (sameProcesses.ContainsKey(process.ProcessName))
                {
                    List<Process> list = sameProcesses[process.ProcessName];
                    list.Add(process);
                }
                else
                {
                    List<Process> list = new List<Process>();
                    list.Add(process);

                    sameProcesses.Add(process.ProcessName, list);
                }
            }

            return sameProcesses;
        }

        /// <summary>
        /// Исключает из журнала защиты те процессы, которые были помечены как процессы-исключения.
        /// </summary>
        /// <param name="processesDict">Журнал процессов.</param>
        /// <returns></returns>
        private Dictionary<String, List<Process>> ExcludeProcesses(Dictionary<String, List<Process>> processesDict)
        {
            Dictionary<String, List<Process>> result = new Dictionary<string, List<Process>>();

            foreach (var processItem in processesDict)
            {
                if (!(this.ProcessesExclusion.Contains(processItem.Key) && this.ProcessIsService(processItem.Value.First().Id)))
                {
                    result.Add(processItem.Key, processItem.Value);
                }
            }

            return result;
        }

        /// <summary>
        /// Возвращает подозрительные процессы, которые могут представлять угрозу системе.
        /// </summary>
        /// <param name="processesDict"></param>
        /// <returns></returns>
        private Dictionary<String, List<Process>> GetSuspiciousProcesses(Dictionary<String, List<Process>> processesDict)
        {
            //var suspiciousProcesses = from item in processesDict
            //                          where ProcessesAreSuspecious(item.Value)
            //                          select item;

            //Dictionary<String, List<Process>> suspiciousProcesses = new Dictionary<string, List<Process>>();
            List<String> deletingKeys = new List<string>();

            foreach (var processItem in processesDict)
            {
                List<Process> list = processItem.Value;

                if (!ProcessesAreSuspecious(list))
                {
                    deletingKeys.Add(processItem.Key);
                }
            }

            foreach (var key in deletingKeys)
            {
                processesDict.Remove(key);
            }

            return processesDict;  
        }

        /// <summary>
        /// Возвращает значение, говорящее о том, является ли список процессов подозрительным.
        /// </summary>
        /// <param name="list">Список проверяемых процессов.</param>
        /// <returns></returns>
        private bool ProcessesAreSuspecious(List<Process> list)
        {
            return list.Count >= ForkProtection.SuspectProcessesCount;
        }

        /// <summary>
        /// Возвращает значение, говорящее о том, не зашло ли количество процессов за критическую отметку.
        /// </summary>
        /// <param name="processes">Список проверяемых процессов.</param>
        /// <returns></returns>
        private bool ProcessesCountIsCritical(List<Process> processes)
        {
            return processes.Count >= this.CriticalProcessesCount;
        }

        /// <summary>
        /// Возвращает значение, говорящее о том, получен ли критический скачок количества процессов.
        /// </summary>
        /// <param name="processName">Название процесса.</param>
        /// <param name="count">Количество процессов на текущий момент.</param>
        /// <returns>Если получено значение true, значит получен критический скачок количества процессов.</returns>
        private bool CriticalJumpObtained(String processName, int count)
        {
            return this.ProcessesCount.ContainsKey(processName) && count - this.ProcessesCount[processName] >= CriticalJumpProcessesCount;
        }

        /// <summary>
        /// Активирует событие, говорящее об обнаружении подозрительного процесса.
        /// </summary>
        /// <param name="processName">Название процесса.</param>
        /// <param name="processesCount">Количество порожденных процессов.</param>
        /// <param name="detectionType">Тип обнаружения.</param>
        /// <returns>Реакция пользователя на обнаружение процесса.</returns>
        private UserResponse RaiseProcessDetection(String processName, int processesCount, DetectionType detectionType)
        {
            if (this.ProcessDetected != null)
            {
                return this.ProcessDetected(new ProcessDetectedEventArgs(processName, processesCount, detectionType));
            }
            else
            {
                return UserResponse.Kill;
            }
        }

        private bool ProcessIsService(int processID)
        {
            using (ManagementObjectSearcher Searcher = new ManagementObjectSearcher(
                "SELECT * FROM Win32_Service WHERE ProcessId =" + "\"" + processID + "\""))
            {
                foreach (ManagementObject service in Searcher.Get())
                    return true;
            }
            return false;
        }
        #region EventHandlers
        void Handler_DetectionTimerTick(object sender, ElapsedEventArgs e)
        {
            Detection();
        }
        #endregion




    }
}
