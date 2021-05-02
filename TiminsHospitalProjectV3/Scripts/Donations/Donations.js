window.onload = function () {
    var form = document.forms.donationForm;
    form.onsubmit = function () {
        let firstName = document.getElementById("Donation_FistName");

        if (firstName.value === "") {
            let errorMsg = document.getElementById("firstNameError");
            errorMsg.innerHTML = "The First Name field is required";
        }
        return false;
    }
};//Funtion Ends