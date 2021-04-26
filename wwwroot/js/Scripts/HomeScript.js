document.getElementById("nameTag").innerHTML = "Wazi,"
document.getElementById("descriptionTag").innerHTML = "List of functions is found in root/js/SimpleCharLibrary.js || Functions called from  root/js/SimpleCharLibrary.js";

//Histogram
document.addEventListener("DOMContentLoaded", createHistogram('containerA'));

//Waterfall
document.addEventListener("DOMContentLoaded", createWaterFallChart('containerB'));

//Gantt Chart
document.addEventListener("DOMContentLoaded", createGanttChart('containerC'));

//pie chart
document.addEventListener('DOMContentLoaded', createPieChart('container1'))

//Bar chart
document.addEventListener('DOMContentLoaded', createBarChart('container2'));

//Column Chart
document.addEventListener('DOMContentLoaded', createColumnChart('container3'));

//lineChart
document.addEventListener("DOMContentLoaded", createLineChart('container4'));

//AreaSpline
document.addEventListener("DOMContentLoaded", createAreaSpline('container5'));

//ScatterPlot
document.addEventListener("DOMContentLoaded", createScatterPlot('container6'));

//ErrorGraph
document.addEventListener("DOMContentLoaded", createErrorBar('container7'));

//HeatMap
document.addEventListener("DOMContentLoaded", createHeatMap('container8'));

//Lollipop
document.addEventListener("DOMContentLoaded", createLollipopChart('container9'));

//paretoChart
document.addEventListener("DOMContentLoaded", createParetoChart('container10'));

//Create TimeLine
document.addEventListener("DOMContentLoaded", createTimeLine('container11'));

//Create Variable radius
document.addEventListener("DOMContentLoaded", createVariableRadiusPie('container12'));

//Create x-range chart
document.addEventListener("DOMContentLoaded", createXRangeChart('container13'));