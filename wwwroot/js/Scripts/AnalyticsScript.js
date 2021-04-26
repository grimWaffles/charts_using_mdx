//Corresponds to the Analytics/Index.cshtml view
/*How  to do a fetch POST request vanilla js*/

/*
fetch('link here', {
    method: 'POST',
    headers: {
        'Content-Type':'application/json'
    },
    body: {
        JSON.stringify({
            name:'user1'
        })
    }
})
    .then(res => { return res.json() })
    .catch(error=>console.log(error))
*/
function makeChart(data) {
    //data contains two objects of the same type but server different purposes
    //consider making a model class to hold the thing


}

function getDataFromServer() {

    var str;

    fetch('https://localhost:44335/analytics/json')
        .then((res) => res.json())
        .then((data) => {
            console.log(data)
            
            makeChart(data);
        })
        .catch(error => console.log(error))

   
}

//declare variables here

document.getElementById("username").innerHTML = "Wazi"

getDataFromServer();


