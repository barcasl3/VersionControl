using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml;
using WebServices.Entities;
using WebServices.MnbServiceReference;

namespace WebServices
{
    public partial class Form1 : Form
    {

        MNBArfolyamServiceSoapClient mnbService = new MNBArfolyamServiceSoapClient();
        BindingList<RateData> Rates = new BindingList<RateData>();
        BindingList<string> Currencies = new BindingList<string>();
        
        public Form1()
        {
            InitializeComponent();
            comboBox1.DataSource = Currencies;
            var request = new GetCurrenciesRequestBody { };
            var response = mnbService.GetCurrencies(request);
            var result = response.GetCurrenciesResult;

            var xml = new XmlDocument();
            xml.LoadXml(result);

            foreach (XmlElement element in xml.DocumentElement)
            {

            }

            refreshData();
        }

        private void init()
        {
            var request = new GetExchangeRatesRequestBody
            {
                currencyNames = "EUR",
                startDate = dateTimePicker1.Value.ToString(),
                endDate = dateTimePicker2.Value.ToString()
            };

            var response = mnbService.GetExchangeRates(request);
            var result = response.GetExchangeRatesResult;

            var xml = new XmlDocument();
            xml.LoadXml(result);

            foreach  (XmlElement element in xml.DocumentElement)
            {
                var rate = new RateData();
                Rates.Add(rate);

                rate.Date = Convert.ToDateTime(element.GetAttribute("date"));

                var child = (XmlElement)element.ChildNodes[0];
                rate.Currency = child.GetAttribute("curr");

                var unit = decimal.Parse(child.GetAttribute("unit"));
                var value = decimal.Parse(child.InnerText);
                if(unit != 0)
                {
                    rate.Value = value / unit;
                }
            }
        }

        private void initChart()
        {
            var series = chartRateData.Series[0];

            series.ChartType = SeriesChartType.Line;
            series.XValueMember = "Date";
            series.YValueMembers = "Value";
            series.BorderWidth = 2;

            var chartArea = chartRateData.ChartAreas[0];
            chartArea.AxisX.MajorGrid.Enabled = false;
            chartArea.AxisY.MajorGrid.Enabled = false;
            chartArea.AxisY.IsStartedFromZero = false;

            var legend = chartRateData.Legends[0];
            legend.Enabled = false;
        }

        private void refreshData()
        {
            init();
            initChart();
            dataGridView1.DataSource = Rates;
            chartRateData.DataSource = Rates;
            Rates.Clear();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            refreshData();
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            refreshData();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            refreshData();
        }
    }
}
