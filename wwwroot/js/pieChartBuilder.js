const orderCanvas = document.getElementById("ordersPieChart");
const accountsCanvas = document.getElementById("accountsPieChart");
const placesCanvas = document.getElementById("placesPieChart");


new Chart(orderCanvas, {
    type: "pie",
    data: {
        labels: ['Ordinary', 'Express', 'Insured'],
        datasets: [{
            data: [orderData.ordinary, orderData.express, orderData.insured],
            backgroundColor: ['#a32929', '#2958a3', '#29a356']
        }]
    },
    options: {
        responsive: false,
        plugins: {
            legend: {
                labels: {
                    color: '#000'
                }
            }
        }
    }
});

new Chart(accountsCanvas, {
    type: "pie",
    data: {
        labels: ['Head', 'Admin', 'Operator', 'Courier', 'User'],
        datasets: [{
            data: [accountData.headAdmins, accountData.admins, accountData.operators, accountData.couriers, accountData.users],
            backgroundColor: ['#a32929', '#2958a3', '#29a356', '#bf5f15', '#d6ca47', '#47d1d6']
        }]
    },
    options: {
        responsive: false,
        plugins: {
            legend: {
                labels: {
                    color: '#000'
                }
            }
        }
    }
});

new Chart(placesCanvas, {
    type: "pie",
    data: {
        labels: ['Accept point', 'Sorting point', 'Pickup point'],
        datasets: [{
            data: [placesData.accept, placesData.sorting, placesData.pickup],
            backgroundColor: ['#a32929', '#2958a3', '#29a356']
        }]
    },
    options: {
        responsive: false,
        plugins: {
            legend: {
                labels: {
                    color: '#000'
                }
            }
        }
    }
});