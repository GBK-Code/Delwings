document.getElementById("ordersFilterButton").addEventListener("click", async function () {
    const fromDate = document.getElementById("dateFrom").value;
    const toDate = document.getElementById("dateTo").value;

    const ordinaryCheck = document.getElementsByName("ordinaryOrderCheck")[0].checked;
    const insuredCheck = document.getElementsByName("insuredOrderCheck")[0].checked;
    const expressCheck = document.getElementsByName("expressOrderCheck")[0].checked;


    const response = await fetch(`/Operator/SearchOrdersByDate?from=${fromDate}&to=${toDate}&ordinary=${ordinaryCheck}&express=${expressCheck}&insured=${insuredCheck}`);
    const html = await response.text();

    document.getElementById("ordersSearchTable").innerHTML = html;
});