const trackInputElement = document.getElementById("trackInput");
const addressInputElement = document.getElementById("addressInput");
const ordinaryCheckbox = document.getElementsByName("typeCheck")[0];
const insuredCheckbox = document.getElementsByName("typeCheck")[1];
const expressCheckbox = document.getElementsByName("typeCheck")[2];

async function filter() {
    const ordinaryCheck = ordinaryCheckbox.checked;
    const insuredCheck = insuredCheckbox.checked;
    const expressCheck = expressCheckbox.checked;

    const trackId = document.getElementById("trackInput").value;
    const address = document.getElementById("addressInput").value;
    
    const responseString = `/Operator/FilterRedirectOrders?trackId=${trackId}&address=${address}&ordinary=${ordinaryCheck}&express=${expressCheck}&insured=${insuredCheck}`

    const response = await fetch(responseString);
    const html = await response.text();

    document.getElementById("pendingOrdersTable").innerHTML = html;
}

trackInputElement.addEventListener("input", filter);
addressInputElement.addEventListener("input", filter);

ordinaryCheckbox.addEventListener("input", filter);
insuredCheckbox.addEventListener("input", filter);
expressCheckbox.addEventListener("input", filter);