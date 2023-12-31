using OxyPlot.Series;
using OxyPlot;
using OxyPlot.Legends;
using System.Diagnostics;
using OxyPlot.Axes;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var pv = new OxyPlot.WindowsForms.PlotView();
            pv.Location = new Point(0, 0);
            pv.Dock = DockStyle.Fill;
            pv.Size = new Size(100, 500);
            this.Controls.Add(pv);

            var model = new PlotModel
            {
                Title = "Efficiency Plot",
                IsLegendVisible = true,

            };

            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "Data Size" });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Execution Time (ms)" });

            model.Legends.Add(new Legend() { LegendPosition = LegendPosition.RightTop, LegendTitle = "Algorithms" });

            int[] dataSizes = { 50, 100, 150, 200, 250, 500 };

            var seriesNames = new List<string> { "BubbleSort", "SelectionSort", "InsertionSort", "QuickSort", "MergeSort", "HeapSort", "RadixSort" };

            var bubbleSortSeries = new AreaSeries { Title = "BubbleSort" };
            var selectionSortSeries = new AreaSeries { Title = "SelectionSort" };
            var insertionSortSeries = new AreaSeries { Title = "InsertionSort" };
            var QuickSortSeries = new AreaSeries { Title = "QuickSort" };
            var MergeSortSeries = new AreaSeries { Title = "MergeSort" };
            var HeapSortSeries = new AreaSeries { Title = "HeapSort" };
            var RadixSortSeries = new AreaSeries { Title = "RadixSort" };

            var stopwatch = new Stopwatch();
            using (StreamWriter writer = new StreamWriter("statistics.txt"))
            {
                foreach (int dataSize in dataSizes)
                {
                    List<int> data = GenerateRandomData(dataSize);

                    foreach (var seriesName in seriesNames)
                    {
                        List<int> clonedData = new List<int>(data);
                        List<int> finalValue = new();

                        stopwatch.Start();

                        switch (seriesName)
                        {
                            case "BubbleSort":
                                finalValue = BubbleSort(clonedData);
                                break;
                            case "SelectionSort":
                                finalValue = SelectionSort(clonedData);
                                break;
                            case "InsertionSort":
                                finalValue = InsertionSort(clonedData);
                                break;
                            case "QuickSort":
                                finalValue = QuickSort(clonedData, 0, clonedData.Count - 1);
                                break;
                            case "MergeSort":
                                finalValue = MergeSort(clonedData, 0, clonedData.Count - 1);
                                break;
                            case "HeapSort":
                                finalValue = HeapSort(clonedData);
                                break;
                            case "RadixSort":
                                finalValue = RadixSort(clonedData);
                                break;
                        }
                        stopwatch.Stop();

                        double executionTime = stopwatch.Elapsed.TotalMilliseconds;
                        string finalValueString = string.Join(", ", finalValue);
                        writer.WriteLine($"{dataSize} set, {seriesName}: {executionTime} ms, answer: {finalValueString}");

                        switch (seriesName)
                        {
                            case "BubbleSort":
                                bubbleSortSeries.Points.Add(new DataPoint(dataSize, executionTime));
                                break;
                            case "SelectionSort":
                                selectionSortSeries.Points.Add(new DataPoint(dataSize, executionTime));
                                break;
                            case "InsertionSort":
                                insertionSortSeries.Points.Add(new DataPoint(dataSize, executionTime));
                                break;
                            case "QuickSort":
                                QuickSortSeries.Points.Add(new DataPoint(dataSize, executionTime));
                                break;
                            case "MergeSort":
                                MergeSortSeries.Points.Add(new DataPoint(dataSize, executionTime));
                                break;
                            case "HeapSort":
                                HeapSortSeries.Points.Add(new DataPoint(dataSize, executionTime));
                                break;
                            case "RadixSort":
                                RadixSortSeries.Points.Add(new DataPoint(dataSize, executionTime));
                                break;
                        }

                        stopwatch.Reset();
                    }
                }
            }

            model.Series.Add(bubbleSortSeries);
            model.Series.Add(selectionSortSeries);
            model.Series.Add(insertionSortSeries);
            model.Series.Add(QuickSortSeries);
            model.Series.Add(MergeSortSeries);
            model.Series.Add(HeapSortSeries);
            model.Series.Add(RadixSortSeries);

            pv.Model = model;
        }

        //algorithms
                static List<int> GenerateRandomData(int size)
        {
            Random random = new Random();
            return Enumerable.Range(1, size).OrderBy(x => random.Next()).ToList();
        }

        static List<int> BubbleSort(List<int> data)
        {
            int n = data.Count;
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    if (data[j] > data[j + 1])
                    {
                        int temp = data[j];
                        data[j] = data[j + 1];
                        data[j + 1] = temp;
                    }
                }
            }

            return data;
        }

        static List<int> SelectionSort(List<int> data)
        {
            int n = data.Count;
            for (int i = 0; i < n - 1; i++)
            {
                int minIndex = i;
                for (int j = i + 1; j < n; j++)
                {
                    if (data[j] < data[minIndex])
                    {
                        minIndex = j;
                    }
                }
                int temp = data[i];
                data[i] = data[minIndex];
                data[minIndex] = temp;
            }

            return data;
        }

        static List<int> InsertionSort(List<int> data)
        {
            int n = data.Count;
            for (int i = 1; i < n; i++)
            {
                int key = data[i];
                int j = i - 1;
                while (j >= 0 && data[j] > key)
                {
                    data[j + 1] = data[j];
                    j--;
                }
                data[j + 1] = key;
            }

            return data;
        }

        static List<int> QuickSort(List<int> data, int low, int high)
        {
            if (low < high)
            {
                int pivot = Partition(data, low, high);
                QuickSort(data, low, pivot - 1);
                QuickSort(data, pivot + 1, high);
            }

            return data;
        }

        static int Partition(List<int> data, int low, int high)
        {
            int pivot = data[high];
            int i = low - 1;
            for (int j = low; j < high; j++)
            {
                if (data[j] < pivot)
                {
                    i++;
                    int temp = data[i];
                    data[i] = data[j];
                    data[j] = temp;
                }
            }
            int temp1 = data[i + 1];
            data[i + 1] = data[high];
            data[high] = temp1;
            return i + 1;
        }

        static List<int> HeapSort(List<int> data)
        {
            int n = data.Count;

            for (int i = n / 2 - 1; i >= 0; i--)
                Heapify(data, n, i);

            for (int i = n - 1; i > 0; i--)
            {
                int temp = data[0];
                data[0] = data[i];
                data[i] = temp;

                Heapify(data, i, 0);
            }
            return data;
        }

        static void Heapify(List<int> data, int n, int i)
        {
            int largest = i;
            int left = 2 * i + 1;
            int right = 2 * i + 2;


            if (left < n && data[left] > data[largest])
                largest = left;

            if (right < n && data[right] > data[largest])
                largest = right;


            if (largest != i)
            {
                int swap = data[i];
                data[i] = data[largest];
                data[largest] = swap;

                Heapify(data, n, largest);
            }
        }

        static List<int> RadixSort(List<int> data)
        {
            int max = data.Max();

            for (int exp = 1; max / exp > 0; exp *= 10)
                CountingSort(data, exp);
            return data;
        }

        static void CountingSort(List<int> data, int exp)
        {
            int n = data.Count;
            int[] output = new int[n];
            int[] count = new int[10];

            for (int i = 0; i < 10; i++)
                count[i] = 0;

            for (int i = 0; i < n; i++)
                count[(data[i] / exp) % 10]++;

            for (int i = 1; i < 10; i++)
                count[i] += count[i - 1];

            for (int i = n - 1; i >= 0; i--)
            {
                output[count[(data[i] / exp) % 10] - 1] = data[i];
                count[(data[i] / exp) % 10]--;
            }

            for (int i = 0; i < n; i++)
                data[i] = output[i];
        }

        static List<int> MergeSort(List<int> data, int left, int right)
        {
            if (left < right)
            {
                int middle = (left + right) / 2;

                MergeSort(data, left, middle);
                MergeSort(data, middle + 1, right);

                Merge(data, left, middle, right);
            }

            return data;
        }

        static void Merge(List<int> data, int left, int middle, int right)
        {
            int n1 = middle - left + 1;
            int n2 = right - middle;

            int[] leftArray = new int[n1];
            int[] rightArray = new int[n2];

            for (int m = 0; m < n1; m++)
                leftArray[m] = data[left + m];

            for (int n = 0; n < n2; n++)
                rightArray[n] = data[middle + 1 + n];

            int k = left;
            int i = 0, j = 0;

            while (i < n1 && j < n2)
            {
                if (leftArray[i] <= rightArray[j])
                {
                    data[k] = leftArray[i];
                    i++;
                }
                else
                {
                    data[k] = rightArray[j];
                    j++;
                }
                k++;
            }

            while (i < n1)
            {
                data[k] = leftArray[i];
                i++;
                k++;
            }

            while (j < n2)
            {
                data[k] = rightArray[j];
                j++;
                k++;
            }
        }
    }
}