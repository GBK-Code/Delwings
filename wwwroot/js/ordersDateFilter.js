document.getElementById("ordersFilterButton").addEventListener("click", async function () {
    const fromDate = document.getElementById("dateFrom").value;
    const toDate = document.getElementById("dateTo").value;

    const ordinaryCheck = document.getElementsByName("ordinaryOrderCheck")[0].checked;
    const insuredCheck = document.getElementsByName("insuredOrderCheck")[0].checked;
    const expressCheck = document.getElementsByName("expressOrderCheck")[0].checked;

    const descendingCheck = document.getElementsByName("descendingCheck")[0].checked;
    const sortedCheck = document.getElementsByName("sortedCheck")[0].checked;

    const responseString = `/Operator/FilterOrders?from=${fromDate}&to=${toDate}&ordinary=${ordinaryCheck}&express=${expressCheck}&insured=${insuredCheck}&sorted=${sortedCheck}&descending=${descendingCheck}`

    const response = await fetch(responseString);
    const html = await response.text();

    document.getElementById("ordersSearchTable").innerHTML = html;
});