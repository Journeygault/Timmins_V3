$(document).ready(function () {
    let config = {
        altInput: true,
        altFormat: "Y-m-d H:i",
        allowInput:true,
        dateFormat: "Y-m-d H:i",
        minDate: new Date(),
        enableTime: true,
        time_24hr: true,
        minTime: "08:00",
        maxTime: "17:30",       

    }
    $("#RequestDatetime").flatpickr(config);
    
    console.log($("#RequestDatetime").val());
});