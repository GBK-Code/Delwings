const orderTypeRadios = document.querySelectorAll('input[name="orderType"]');
const expressFields = document.getElementById('expressFields');
const insuranceFields = document.getElementById('insuranceFields');
const courierRadios = document.querySelectorAll('input[name="courierId"]')

function updateFields() {
    const ordinaryCheck = document.getElementsByName("orderType")[0].checked;
    const insuredCheck = document.getElementsByName("orderType")[1].checked;
    const expressCheck = document.getElementsByName("orderType")[2].checked;

    console.log(ordinaryCheck, insuredCheck, expressCheck);

    expressFields.classList.add('d-none');
    insuranceFields.classList.add('d-none');

    if (expressCheck) {
        expressFields.classList.remove('d-none');
        courierRadios.forEach(r => r.required = true);

    } else if (insuredCheck) {
        insuranceFields.classList.remove('d-none');
        courierRadios.forEach(r => {
            r.required = false;
            r.checked = false;
        });
    }
}

orderTypeRadios.forEach(radio => {
    radio.addEventListener('change', updateFields);
});

updateFields();