const countryInput = document.getElementById("country");
const cityInput = document.getElementById("city");
const addressInput = document.getElementById("address");

const sortingCheckbox = document.getElementsByName("placeTypeCheck")[1];
const pickupCheckbox = document.getElementsByName("placeTypeCheck")[2];

async function filterRedirectPlaces() {
    const country = countryInput.value;
    const city = cityInput.value;
    const address = addressInput.value;

    const sortingCheck = sortingCheckbox.checked;
    const pickupCheck = pickupCheckbox.checked;

    const responseString = `/Operator/FilterRedirectPlaces?country=${country}&city=${city}&address=${address}&isSorting=${sortingCheck}&isPickup=${pickupCheck}`

    const response = await fetch(responseString);
    const html = await response.text();

    document.getElementById("placesFilteredTable").innerHTML = html;
}

countryInput.addEventListener("input", filterRedirectPlaces);
cityInput.addEventListener("input", filterRedirectPlaces);
addressInput.addEventListener("input", filterRedirectPlaces);

sortingCheckbox.addEventListener("input", filterRedirectPlaces);
pickupCheckbox.addEventListener("input", filterRedirectPlaces);