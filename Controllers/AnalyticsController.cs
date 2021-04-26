using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AnalysisServices.AdomdClient;
using System.Text;
using ChartsUsingMdx.Models;

namespace ChartsUsingMdx.Controllers
{

    [Route("[controller]")]
    public class AnalyticsController : Controller
    {
        private string ConnString { get; set; }
        private string DefaultQuery { get; set; }
        private string QuerySecond { get; set; }
        
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

            this.QuerySecond = "select {[Measures].[Sales Amount]} on columns,[Due Date].[Calendar Year].[Calendar Year] on rows from[Adventure Works DW2016]";

            
        }

        public IActionResult Index()
        {
            return View();
        }

        //returns the information of all cubes in the server
        //https://localhost:*portnumber*/api/analytics/cubes
        [HttpGet]
        [Route("cubes")]
        public string GetCubeInformation()
        {
            StringBuilder cubeInformation = new StringBuilder();

            AdomdConnection conn = new AdomdConnection(ConnString);

            conn.Open();

            //Cube objects are CubeDef here
            foreach (CubeDef cube in conn.Cubes)
            {
                if (cube.Name.StartsWith('$'))
                    continue;
                cubeInformation.Append("Cube Name: " + cube.Name + '\n');
                cubeInformation.Append("Cube KPIs: " + cube.Kpis.Count + '\n');
                cubeInformation.Append("Cube Measures: " + cube.Measures.Count + '\n');
                cubeInformation.Append("Updated at " + cube.LastUpdated + '\n' + "Dimensions: " + '\n');

                foreach (Dimension dim in cube.Dimensions)
                {
                    cubeInformation.AppendLine(dim.Name);
                }

                cubeInformation.Append("\n\n");
            }

            conn.Close();

            return cubeInformation.ToString();
        }

        [HttpGet]
        [Route("json")]
        public IActionResult GetJsonData()
        {
            AdomdConnection conn = new AdomdConnection(ConnString);
            conn.Open();

            string commandText = QuerySecond;

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

                obj.HeaderName = coltuple.Members[0].Caption.ToString();

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

                obj.Values = integerValues;

                dataObjs.Add(obj);

                columnIterator++;
            }

            //Getting the row tuples
            int TupleRowCount = tupleRows[1].Members.Count;

            List<DataObj> dataObjs2 = new List<DataObj>();
            
            for(int columnNumber = 0; columnNumber < tupleRows[0].Members.Count; columnNumber++)
            {
                DataObj obj2 = new DataObj();
                obj2.HeaderName = "Parameter " + columnNumber;

                List<String> stringValues = new List<string>();

                for(int rowNumber = 0; rowNumber < tupleRows.Count; rowNumber++)
                {
                    stringValues.Add(tupleRows[rowNumber].Members[columnNumber].Caption.ToString());
                }

                obj2.Parameters = stringValues;

                dataObjs2.Add(obj2);
            }

            conn.Close();

            return new JsonResult(new { y_values = dataObjs, x_values=dataObjs2 });

        }

        //Fetches data using a cellset from the analysis server
        //GET https://localhost:*portnumber*/api/analytics/raw --is the complete link
        [HttpGet]
        [Route("raw")]
        public string GetStringOutputForQuery([FromQuery] string query)
        {
            StringBuilder result = new StringBuilder();

            AdomdConnection conn = new AdomdConnection(ConnString);
            conn.Open();

            string commandText = query ?? DefaultQuery;

            AdomdCommand adomdCommand = new AdomdCommand(commandText, conn);

            CellSet cs = adomdCommand.ExecuteCellSet();

            //these are the tuples  in the columns i.e first row
            TupleCollection tupleColumns = cs.Axes[0].Set.Tuples;

            //for each of the rows
            TupleCollection tupleRows = cs.Axes[1].Set.Tuples;

            //finding how many row member are there

            int TupleRowCount = tupleRows[1].Members.Count;

            for (int b = 0; b < TupleRowCount; b++)
            {
                //insert a tab into the document
                result.Append('\t');
            }

            //foreach tuple cycle through and append  to result string
            foreach (Microsoft.AnalysisServices.AdomdClient.Tuple colValue in tupleColumns)
            {

                //get the string value of the tuple
                result.Append(colValue.Members[0].Caption + '\t');
            }

            //add blank line after the first row in the string
            result.AppendLine();

            //take each row
            for (int row = 0; row < tupleRows.Count; row++)
            {
                for (int a = 0; a < TupleRowCount; a++)
                {
                    //add the  caption like before
                    result.Append(tupleRows[row].Members[a].Caption + '\t');
                }

                //foreach col in the row append the result
                for (int col = 0; col < tupleColumns.Count; col++)
                {
                    if (cs.Cells[col, row].FormattedValue != null || cs.Cells[col, row].FormattedValue != "")
                    {
                        result.Append(cs.Cells[col, row].FormattedValue + '\t');
                    }
                }
                result.AppendLine();
            }

            conn.Close();

            return result.ToString();
        }

        [HttpGet]
        [Route("formatted")]
        public IActionResult GetArrayFromMdx([FromQuery] string query)
        {
            StringBuilder result = new StringBuilder();

            AdomdConnection conn = new AdomdConnection(ConnString);
            conn.Open();

            string commandText = query ?? DefaultQuery;

            AdomdCommand adomdCommand = new AdomdCommand(commandText, conn);

            CellSet cs = adomdCommand.ExecuteCellSet();

            TupleCollection tupleColumns = cs.Axes[0].Set.Tuples;

            TupleCollection tupleRows = cs.Axes[1].Set.Tuples;

            int TupleRowCount = tupleRows[1].Members.Count;

            result.Append(","); int counter = 0;

            //foreach tuple cycle through and append  to result string
            foreach (Microsoft.AnalysisServices.AdomdClient.Tuple colValue in tupleColumns)
            {
                if (counter != tupleColumns.Count)
                {
                    //get the string value of the tuple
                    result.Append(colValue.Members[0].Caption + ',');
                }
                else
                {
                    //get the string value of the tuple
                    result.Append(colValue.Members[0].Caption);
                }
                counter++;
            }
            result.Remove(result.Length - 1, 1);

            result.AppendLine();

            for (int row = 0; row < tupleRows.Count; row++)
            {
                for (int a = 0; a < TupleRowCount; a++)
                {
                    result.Append(tupleRows[row].Members[a].Caption + ',');
                }

                for (int col = 0; col < tupleColumns.Count; col++)
                {
                    result.Append(cs.Cells[col, row].FormattedValue + ',');
                }

                result.Remove(result.Length - 1, 1);

                result.AppendLine();
            }

            conn.Close();

            return new JsonResult(result.ToString() );
        }

        
    } 
}
