$(document).ready(function () {
    let config = {
        altInput: true,
        altFormat: "Y-m-d H:i",
        dateFormat: "Y-m-d H:i",
        minDate: new Date().fp_incr(2),
        enableTime: true,
        time_24hr: true,
        minTime: "08:00",
        maxTime: "17:30"

    }
    $("#RequestDatetime").flatpickr(config);

});