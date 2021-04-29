using ChartsUsingMdx.Models;
using Microsoft.AnalysisServices.AdomdClient;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChartsUsingMdx.Controllers
{

    [Route("[controller]")]
    public class CubesController : Controller
    {
        private string ConnString { get; set; }

        public CubesController()
        {
            this.ConnString = "Data Source=DESKTOP-7FOU4BG; Catalog=Cube Basics 3";
        }

        public IActionResult Index()
        {
            return View();

        }

        [HttpGet]
        [Route("api/getcube")]
        public IActionResult GetCubeList()
        {
            StringBuilder cubeInformation = new StringBuilder();

            AdomdConnection conn = new AdomdConnection(ConnString);

            conn.Open();

            List<DataCube> dataCubes = new List<DataCube>();

            foreach (CubeDef cube in conn.Cubes)
            {
                if (cube.Name.StartsWith('$'))
                    continue;
                DataCube dataCube = new DataCube();
                
                dataCube.Name = cube.Name;
                dataCube.Updated_at = cube.LastUpdated.ToString();

                List<string> kpiList = new List<string>();
                List<string> measureList = new List<string>();
                List<string> dimensionList = new List<string>();

                foreach (Kpi kpi in cube.Kpis)
                {
                    kpiList.Add(kpi.Name.ToString());
                }

                foreach (Measure measure in cube.Measures)
                {
                    measureList.Add(measure.Name.ToString());
                }

                foreach (Dimension dim in cube.Dimensions)
                {
                    dimensionList.Add(dim.Name.ToString());
                }

                dataCube.Kpis = kpiList;
                dataCube.Measures = measureList;
                dataCube.Dimensions = dimensionList;

                dataCubes.Add(dataCube);
            }

            conn.Close();

            return new JsonResult(new { cubes = dataCubes });
        }

        //returns the information of all cubes in the server
        //https://localhost:*portnumber*/api/analytics/cubes
        [HttpGet]
        [Route("api/cubeinfo")]
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


    }
}
