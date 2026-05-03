const countryInput = document.getElementById("country");
const cityInput = document.getElementById("city");
const addressInput = document.getElementById("address");

const acceptCheckbox = document.getElementsByName("placeTypeCheck")[0];
const sortingCheckbox = document.getElementsByName("placeTypeCheck")[1];
const pickupCheckbox = document.getElementsByName("placeTypeCheck")[2];

async function filterPlaces() {
    const country = countryInput.value;
    const city = cityInput.value;
    const address = addressInput.value;

    const acceptCheck = acceptCheckbox.checked;
    const sortingCheck = sortingCheckbox.checked;
    const pickupCheck = pickupCheckbox.checked;

    const responseString = `/Head/FilterPlaces?country=${country}&city=${city}&address=${address}&isAccept=${acceptCheck}&isSorting=${sortingCheck}&isPickup=${pickupCheck}`

    const response = await fetch(responseString);
    const html = await response.text();

    document.getElementById("placesFilteredTable").innerHTML = html;
}

countryInput.addEventListener("input", filterPlaces);
cityInput.addEventListener("input", filterPlaces);
addressInput.addEventListener("input", filterPlaces);

acceptCheckbox.addEventListener("input", filterPlaces);
sortingCheckbox.addEventListener("input", filterPlaces);
pickupCheckbox.addEventListener("input", filterPlaces);