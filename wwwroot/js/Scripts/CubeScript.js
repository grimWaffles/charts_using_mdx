//Global Variables
let cubeData = 1;
const cube = document.getElementById("cubes"); const measure = document.getElementById("measures"); const dimension = document.getElementById("dimensions")

//Event Listeners
cubes.addEventListener('change', () => {

    while (dimensions.options.length > 0) { dimensions.options.remove(0) }
    while (measures.options.length > 0) { measures.options.remove(0) }

    populateOtherSelectors(cubeData);
})

//Functions
function clearSelectors() {
    while (cube.options.length > 0) { cube.options.remove(0) }
    while (dimensions.options.length > 0) { dimensions.options.remove(0) }
    while (measures.options.length > 0) { measures.options.remove(0) }
}

function addOptionsToSelect(selector, optionName) {

    var option = document.createElement('option');
    option.innerHTML = optionName; option.value = optionName;
    selector.add(option);
}

function populateOtherSelectors(cubeData) {

    console.log("From populateOtherSelector"); console.log(cubeData);

    var currentCubeName = cube.value;

    console.log("Current cube name"); console.log(currentCubeName);

    for (var i = 0; i < cubeData.cubes.length; i++) {

        if (cubeData.cubes[i].name == currentCubeName) {
            for (let j = 0; j < cubeData.cubes[i].measures.length; j++) {
                addOptionsToSelect(measure, cubeData.cubes[i].measures[j]);
            }
            for (let j = 0; j < cubeData.cubes[i].dimensions.length; j++) {
                addOptionsToSelect(dimension, cubeData.cubes[i].dimensions[j]);
            }
        }
    }

}

function populateCubeSelector(cubeData) {

    for (var i = 0; i < cubeData.cubes.length; i++) {
        addOptionsToSelect(cube, cubeData.cubes[i].name);
    }

    console.log("From populateCubeSelector"); console.log(cubeData);

    populateOtherSelectors(cubeData);
}

function downloadInformation() {
    fetch('https://localhost:44335/cubes/api/getcube')
        .then(response => {
            return response.json();
        })
        .then(data => {

            console.log("Data for cubes: ")
            console.log(data)

            cubeData = data;
            console.log(cubeData);
            populateCubeSelector(cubeData);
        })
        .catch(error => { console.log(error) })
}

//Main Script start
console.log("The process has started!")

clearSelectors();

downloadInformation();


//Main Script end