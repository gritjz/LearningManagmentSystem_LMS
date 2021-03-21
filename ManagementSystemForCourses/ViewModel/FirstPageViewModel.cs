using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using ManagementSystemForCourses.DataAccess;
using ManagementSystemForCourses.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace ManagementSystemForCourses.ViewModel
{
    public class FirstPageViewModel : NotifyBase
    {
        private int instrumentValue;

        public int InstrumentValue
        {
            get { return instrumentValue; }
            set { instrumentValue = value; this.DoNotify(); }
        }

        private int itemCount;

        public int ItemCount
        {
            get { return itemCount; }
            set { itemCount = value; this.DoNotify(); }
        }


        public ObservableCollection<CourseSeriesModel> CourseSeriesList { get; set; } =
            new ObservableCollection<CourseSeriesModel>();


        private void InitCourseSeries()
        {
            var cList = LocalDataAccess.GetInstance().GetCoursePlayRecord();
            this.ItemCount = cList.Max(c => c.SeriesList.Count);
            foreach (var item in cList)
                this.CourseSeriesList.Add(item);





            //CourseSeriesList.Add(new CourseSeriesModel
            //            {
            //                CourseName = "Java Advanced Practice VIP",
            //                SeriesCollection = new LiveCharts.SeriesCollection
            //                {
            //                 new PieSeries{
            //                        Title="Zhang's",
            //                        Values= new ChartValues<ObservableValue>{ new ObservableValue(123)},
            //                        DataLabels=false},
            //                    new PieSeries
            //                     {
            //                        Title="Zhang's",
            //                        Values= new ChartValues<ObservableValue>{ new ObservableValue(123)},
            //                        DataLabels=false
            //                     }
            //                },
            //                SeriesList = new ObservableCollection<SeriesModel>
            //                {
            //                    new SeriesModel
            //                    {
            //                        SeriesName="Class 1", CurrentViewCount=161, IsGrowing=false, GrowingRate=-75
            //                    },
            //                    new SeriesModel
            //                    {
            //                        SeriesName="Class 2", CurrentViewCount=161, IsGrowing=false, GrowingRate=-75
            //                    },
            //                    new SeriesModel
            //                    {
            //                        SeriesName="Class 3", CurrentViewCount=161, IsGrowing=true, GrowingRate=-75
            //                    },
            //                    new SeriesModel
            //                    {
            //                        SeriesName="Class 4", CurrentViewCount=161, IsGrowing=false, GrowingRate=-75
            //                    },
            //                    new SeriesModel
            //                    {
            //                        SeriesName="Class 5", CurrentViewCount=161, IsGrowing=true, GrowingRate=-75
            //                    }
            //                }
            //            });
            //CourseSeriesList.Add(new CourseSeriesModel
            //{
            //    CourseName = "Java Advanced Practice VIP 2",
            //    SeriesCollection = new LiveCharts.SeriesCollection
            //    {
            //     new PieSeries{
            //            Title="Zhang's",
            //            Values= new ChartValues<ObservableValue>{ new ObservableValue(123)},
            //            DataLabels=false},
            //        new PieSeries
            //         {
            //            Title="Zhang's",
            //            Values= new ChartValues<ObservableValue>{ new ObservableValue(123)},
            //            DataLabels=false
            //         }
            //    },
            //    SeriesList = new ObservableCollection<SeriesModel>
            //    {
            //        new SeriesModel
            //        {
            //            SeriesName="Class 1", CurrentViewCount=161, IsGrowing=false, GrowingRate=-75
            //        },
            //        new SeriesModel
            //        {
            //            SeriesName="Class 2", CurrentViewCount=161, IsGrowing=false, GrowingRate=-75
            //        },
            //        new SeriesModel
            //        {
            //            SeriesName="Class 3", CurrentViewCount=161, IsGrowing=true, GrowingRate=-75
            //        },
            //        new SeriesModel
            //        {
            //            SeriesName="Class 4", CurrentViewCount=161, IsGrowing=false, GrowingRate=-75
            //        },
            //        new SeriesModel
            //        {
            //            SeriesName="Class 5", CurrentViewCount=161, IsGrowing=true, GrowingRate=-75
            //        }
            //    }
            //});


        }


        Random random = new Random();
        bool taskLock = true;
        List<Task> taskPool = new List<Task>();
        public FirstPageViewModel()
        {
            RefreshInstrumentPanelVal();
            InitCourseSeries();
        }

        private void RefreshInstrumentPanelVal()
        {
            var task = Task.Factory.StartNew(new Action(async () =>
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
