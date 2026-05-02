const trackInputElement = document.getElementById("trackInput");
const addressInputElement = document.getElementById("addressInput");
const ordinaryCheckbox = document.getElementsByName("typeCheck")[0];
const insuredCheckbox = document.getElementsByName("typeCheck")[1];
const expressCheckbox = document.getElementsByName("typeCheck")[2];

async function filter() {
    const acceptCheck = ordinaryCheckbox.checked;
    const sortingCheck = insuredCheckbox.checked;
    const pickupCheck = expressCheckbox.checked;

    const trackId = document.getElementById("trackInput").value;
    const address = document.getElementById("addressInput").value;
    
    const responseString = `/Operator/FilterRedirectOrders?trackId=${trackId}&address=${address}&accept=${acceptCheck}&sorting=${sortingCheck}&pickup=${pickupCheck}`

    const response = await fetch(responseString);
    const html = await response.text();

    document.getElementById("pendingOrdersTable").innerHTML = html;
}

trackInputElement.addEventListener("input", filter);
addressInputElement.addEventListener("input", filter);

ordinaryCheckbox.addEventListener("input", filter);
insuredCheckbox.addEventListener("input", filter);
expressCheckbox.addEventListener("input", filter);