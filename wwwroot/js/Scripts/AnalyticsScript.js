//Corresponds to the Analytics/Index.cshtml view
//Custom functions are in wwwroot/js/Scripts/SimpleChartLibrary.js

function getDataFromServer() {

    console.log("The process has started!")  

    fetch('https://localhost:44335/analytics/api/chartdata?id=5')
        .then(response => {
            return response.json();
        })
        .then(data => {

            console.log("Data used for Line chart: ")
            console.log(data)

            createLineChart('container2', data.y_values, data.x_values, 1,'Sales in LifeTime')

        })
        .catch(error => { console.log(error) })

    fetch('https://localhost:44335/analytics/api/chartdata?id=3')
        .then(response => {
            return response.json();
        })
        .then(data => {

            console.log("Data used for Column, Area Spline, Bar chart chart: ")
            console.log(data)

            createColumnChart('container1', data.y_values, data.x_values, 1, 'Product Cost v Unit price in Territories', 'Dollars ($)')
            createAreaSpline('container3', data.y_values, data.x_values, 1, 'Product Cost v Unit price in Territories', 'Dollars ($)')
            createBarChart('container4', data.y_values, data.x_values, 1, 'Product Cost v Unit price in Territories', 'Dollars ($)')
        })
        .catch(error => { console.log(error) })

    fetch('https://localhost:44335/analytics/api/chartdata?id=4')
        .then(response => {
            return response.json();
        })
        .then(data => {

            console.log("Data used for Pie chart: ")
            console.log(data)

            createPieChart('container5', data.y_values, data.x_values, 'Sales by  region')
        })
        .catch(error => { console.log(error) })

    fetch('https://localhost:44335/analytics/api/chartdata?id=5')
        .then(response => {
            return response.json();
        })
        .then(data => {

            console.log("Data for the histogram: ")
            console.log(data)

            createHistogram('container6', data.y_values[0].data, 'Frequency of sales');
        })
        .catch(error => { console.log(error) })

}

//declare variables here

document.getElementById("username").innerHTML = "Wazi"


getDataFromServer();



