using ManagementSystemForCourses.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace ManagementSystemForCourses.ViewModel
{
    public class FirstPageViewModel:NotifyBase
    {
        private int instrumentValue;

        public int InstrumentValue
        {
            get { return instrumentValue; }
            set { instrumentValue = value; this.DoNotify(); }
        }

        Random random = new Random();
        bool taskLock = true;
        List<Task> taskPool = new List<Task>();
        public FirstPageViewModel()
        {
            RefreshInstrumentPanelVal();
        }

        private void RefreshInstrumentPanelVal()
        {
            var task = Task.Factory.StartNew(new Action(async() =>
            {
                while (taskLock)
                {
                    InstrumentValue = 
                    random.Next(Math.Max(this.InstrumentValue - 5, -10),
                        Math.Min(this.InstrumentValue + 5, 90));
                    await Task.Delay(1000);
                }
            }
            ));
            taskPool.Add(task);
        }

        public void Dispose() 
        {

            try
            {
                taskLock = false;
                Task.WaitAll(this.taskPool.ToArray());
            }
            catch (Exception) { }
            
        }
    }
}
