using ChartsUsingMdx.Models;
using Microsoft.AnalysisServices.AdomdClient;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChartsUsingMdx.Controllers
{

    [Route("[controller]")]
    public class AnalyticsController : Controller
    {
        private string ConnString { get; set; }
        private string DefaultQuery { get; set; }
        private string QuerySecond { get; set; }
        private string QueryThird { get; set; }
        private string QueryFourth { get; set; }
        private string QueryFifth { get; set; }

        public AnalyticsController()
        {
            this.ConnString = "Data Source=DESKTOP-7FOU4BG; Catalog=Cube Basics 3";

            //This is one big 11second query
            this.DefaultQuery = "select non empty {[Measures].[Sales Amount],[Measures].[Order Quantity],[Measures].[Tax Amt]} on columns," +

                "( {[Dim Sales Territory 1].[Sales Territory Key].[Sales Territory Key]}," +

                " {[Dim Product 1].[Product Key].[Product Key]}," +

                "{[Due Date].[Calendar Year].&[2014]," +
                "[Due Date].[Calendar Year].&[2013]," +
                "[Due Date].[Calendar Year].&[2012]," +
                "[Due Date].[Calendar Year].&[2011]}," +

                "{[Due Date].[English Month Name].&[January]," +
                "[Due Date].[English Month Name].&[February]," +
                "[Due Date].[English Month Name].&[March]," +
                "[Due Date].[English Month Name].&[April]}" +
                ") on rows " +

                "from[Adventure Works DW2016]; ";

            this.QuerySecond = "select {[Measures].[Sales Amount]} on columns,[Due Date].[Calendar Year].&[2011]:null on rows from[Adventure Works DW2016]";

            this.QueryThird = "select non empty {[Measures].[Total Product Cost],[Measures].[Unit Price]} on columns," +
                                "({[Dim Sales Territory 1].[Sales Territory Key].[Sales Territory Key]}) on rows " +
                                "from[Adventure Works DW2016]";

            this.QueryFourth = "select non empty {[Measures].[Sales Amount]} on columns," + "({[Dim Sales Territory 1].[Sales Territory Country].[Sales Territory Country]}) on rows from [Adventure Works DW2016]";

            this.QueryFifth = "select {[Measures].[Sales Amount]} on columns," + "({[Due Date].[Calendar Year].&[2011]:null},{[Due Date].[English Month Name].[English Month Name]}) on rows from [Adventure Works DW2016]";
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("api/chartdata")]
        public IActionResult GetJsonData([FromQuery] int id)
        {
            AdomdConnection conn = new AdomdConnection(ConnString);
            conn.Open();

            string commandText = "";

            if (id == 1) { commandText = DefaultQuery; }
            else if (id == 2) { commandText = QuerySecond; }
            else if (id == 3) { commandText = QueryThird; }
            else if (id == 4) { commandText = QueryFourth; }
            else if (id == 5) { commandText = QueryFifth; }

            AdomdCommand adomdCommand = new AdomdCommand(commandText, conn);

            CellSet cs = adomdCommand.ExecuteCellSet();

            //width of the cells
            TupleCollection tupleColumns = cs.Axes[0].Set.Tuples;

            //height of the cells
            TupleCollection tupleRows = cs.Axes[1].Set.Tuples;


            //Getting the main values from column tuples and the cellsets
            int columnIterator = 0;

            List<DataObj> dataObjs = new List<DataObj>();

            foreach (Microsoft.AnalysisServices.AdomdClient.Tuple coltuple in tupleColumns)
            {
                DataObj obj = new DataObj();

                obj.Name = coltuple.Members[0].Caption.ToString();

                List<float> integerValues = new List<float>();

                for (int i = 0; i < tupleRows.Count; i++)
                {
                    if (!cs.Cells[columnIterator, i].FormattedValue.Equals("") && !cs.Cells[columnIterator, i].FormattedValue.Equals(null))
                    {
                        integerValues.Add(float.Parse(cs.Cells[columnIterator, i].FormattedValue.ToString()));
                    }

                    else
                    {
                        integerValues.Add((float)0.00);
                    }
                }

                obj.Data = integerValues; obj.Parameters = new List<string>();

                dataObjs.Add(obj);

                columnIterator++;
            }

            //Getting the row tuples
            int TupleRowCount = tupleRows[1].Members.Count;

            List<DataObj> dataObjs2 = new List<DataObj>();

            for (int columnNumber = 0; columnNumber < tupleRows[0].Members.Count; columnNumber++)
            {
                DataObj obj2 = new DataObj();
                obj2.Name = "Parameter " + columnNumber;

                List<String> stringValues = new List<string>();

                for (int rowNumber = 0; rowNumber < tupleRows.Count; rowNumber++)
                {
                    stringValues.Add(tupleRows[rowNumber].Members[columnNumber].Caption.ToString());
                }

                obj2.Parameters = stringValues; obj2.Data = new List<float>();

                dataObjs2.Add(obj2);
            }

            conn.Close();

            return new JsonResult(new { y_values = dataObjs, x_values = dataObjs2 });

        }


    }
}
