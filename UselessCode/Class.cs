using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChartsUsingMdx.UselessCode
{
    public class Class
    {


        /*

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
        */
    }
}
